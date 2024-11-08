using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace REE
{
    public class PakFileBuilder
    {
        private readonly Dictionary<string, object> _entries = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, object> Entries => _entries;

        public void AddEntry(string path, byte[] data)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (data == null) throw new ArgumentNullException(nameof(data));

            _entries[path] = data;
        }

        public void AddEntry(string path, string srcPath)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            _entries[path] = srcPath;
        }

        public void AddDirectory(string path, string pakRootPath = "")
        {
            var entries = Directory.GetFiles(path);
            foreach (var entry in entries)
            {
                var pakPath = pakRootPath + '/' + Path.GetFileName(entry);
                AddEntry(pakPath, entry);
            }
            foreach (var directory in Directory.GetDirectories(path))
            {
                var pakPath = pakRootPath + '/' + Path.GetFileName(directory);
                AddDirectory(directory, pakPath);
            }
        }

        public void Save(string path, CompressionType compressionType)
        {
            using var stream = File.OpenWrite(path);
            Save(stream, compressionType);
        }

        public void Save(Stream stream, CompressionType compressionType)
        {
            var header = new PakHeader();
            header.dwMagic = 0x414B504B;
            header.bMajorVersion = 4;
            header.dwTotalFiles = _entries.Count;
            header.dwHash = 0xDEC0ADDE;

            var pakEntries = new PakEntry[header.dwTotalFiles];
            using var bw = new BinaryWriter(stream);

            // Write pak header
            bw.Write(header.dwMagic);
            bw.Write(header.bMajorVersion);
            bw.Write(header.bMinorVersion);
            bw.Write(header.wFeature);
            bw.Write(header.dwTotalFiles);
            bw.Write(header.dwHash);
            bw.Seek(header.dwTotalFiles * 48, SeekOrigin.Current);

            // Write entries
            var index = 0;
            foreach (var entry in _entries)
            {
                var entryPath = entry.Key;
                var entryData = entry.Value is string s ? File.ReadAllBytes(s) : (byte[])entry.Value;

                var pakEntry = new PakEntry();
                string pakEntryPath;
                if (entryPath.Contains("__Unknown"))
                {
                    pakEntryPath = Path.GetFileNameWithoutExtension(entryPath);
                    pakEntry.dwHashName = Convert.ToUInt64(pakEntryPath, 16);
                }
                else
                {
                    pakEntryPath = entryPath.Replace("\\", "/");
                    pakEntry.dwHashNameUpper = PakHash.iGetHash(pakEntryPath.ToUpperInvariant());
                    pakEntry.dwHashNameLower = PakHash.iGetHash(pakEntryPath.ToLowerInvariant());
                }

                pakEntry.dwOffset = bw.BaseStream.Position;
                pakEntry.dwChecksum = PakHash.iGetDataHash(entryData);

                if (entryData.Length >= 8)
                {
                    var dwMagic1 = BitConverter.ToUInt32(entryData, 0);
                    var dwMagic2 = BitConverter.ToUInt32(entryData, 4);

                    // mov, bnk, pck must be uncompressed
                    if (dwMagic1 == 0x75B22630 || dwMagic1 == 0x564D4552 || dwMagic1 == 0x44484B42 || dwMagic1 == 0x4B504B41 || dwMagic2 == 0x70797466)
                    {
                        pakEntry.dwCompressedSize = entryData.Length;
                        pakEntry.dwDecompressedSize = entryData.Length;
                        pakEntry.wCompressionType = CompressionType.NONE;

                        bw.Write(entryData);
                    }
                    else
                    {
                        var lpDstBuffer = new Byte[] { };

                        switch (compressionType)
                        {
                            case CompressionType.INFLATE: lpDstBuffer = INFLATE.iCompress(entryData); break;
                            case CompressionType.ZSTD: lpDstBuffer = ZSTD.iCompress(entryData); break;
                        }

                        pakEntry.dwCompressedSize = lpDstBuffer.Length;
                        pakEntry.dwDecompressedSize = entryData.Length;
                        pakEntry.wCompressionType = compressionType;

                        bw.Write(lpDstBuffer);
                    }
                }
                else
                {
                    pakEntry.dwCompressedSize = entryData.Length;
                    pakEntry.dwDecompressedSize = entryData.Length;
                    pakEntry.wCompressionType = CompressionType.NONE;

                    bw.Write(entryData);
                }

                pakEntries[index] = pakEntry;
                index++;
            }

            // Write entry table
            bw.Seek(16, SeekOrigin.Begin);
            var sortedPakEntries = pakEntries.OrderBy(m_Entry => m_Entry.dwHashName).ToArray();
            foreach (var pakEntry in sortedPakEntries)
            {
                bw.Write(pakEntry.dwHashName);
                bw.Write(pakEntry.dwOffset);
                bw.Write(pakEntry.dwCompressedSize);
                bw.Write(pakEntry.dwDecompressedSize);
                bw.Write((ulong)pakEntry.wCompressionType);
                bw.Write(pakEntry.dwChecksum);
            }
        }

        public byte[] ToByteArray()
        {
            var ms = new MemoryStream();
            Save(ms, REE.CompressionType.ZSTD);
            return ms.ToArray();
        }
    }
}

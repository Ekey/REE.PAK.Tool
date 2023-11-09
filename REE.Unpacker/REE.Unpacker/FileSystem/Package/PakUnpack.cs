using System;
using System.IO;
using System.Collections.Generic;

namespace REE.Unpacker
{
    class PakUnpack
    {
        private static List<PakEntry> m_EntryTable = new List<PakEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TPakStream = File.OpenRead(m_Archive))
            {
                if (TPakStream.Length <= 16)
                {
                    Utils.iSetError("[ERROR]: Empty PAK archive file");
                    return;
                }

                var m_Header = new PakHeader();

                m_Header.dwMagic = TPakStream.ReadUInt32(); // KPKA
                m_Header.bMajorVersion = TPakStream.ReadByte(); // 4
                m_Header.bMinorVersion = TPakStream.ReadByte(); // 0
                m_Header.wFeature = TPakStream.ReadInt16(); // 0
                m_Header.dwTotalFiles = TPakStream.ReadInt32();
                m_Header.dwHash = TPakStream.ReadUInt32();

                if (m_Header.dwMagic != 0x414B504B)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of PAK archive file");
                    return;
                }

                if (m_Header.bMajorVersion != 4 || m_Header.bMinorVersion != 0)
                {
                    Utils.iSetError("[ERROR]: Invalid version of PAK archive file -> " + m_Header.bMajorVersion.ToString() + "." + m_Header.bMinorVersion.ToString() + ", expected 4.0");
                    return;
                }

                m_EntryTable.Clear();
                var lpTable = TPakStream.ReadBytes(m_Header.dwTotalFiles * 48);

                if (m_Header.wFeature != 0)
                {
                    var lpBlobHash = TPakStream.ReadBytes(128);

                    lpTable = PakCipher.iDecryptData(lpTable, lpBlobHash);
                }

                using (var TEntryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        UInt32 dwHashNameLower = TEntryReader.ReadUInt32();
                        UInt32 dwHashNameUpper = TEntryReader.ReadUInt32();
                        Int64 dwOffset = TEntryReader.ReadInt64();
                        Int64 dwCompressedSize = TEntryReader.ReadInt64();
                        Int64 dwDecompressedSize = TEntryReader.ReadInt64();
                        Int64 wCompressionType = TEntryReader.ReadInt64();
                        UInt64 dwDependencyHash = TEntryReader.ReadUInt64();

                        var TEntry = new PakEntry
                        {
                            dwHashNameLower = dwHashNameLower,
                            dwHashNameUpper = dwHashNameUpper,
                            dwOffset = dwOffset,
                            dwCompressedSize = dwCompressedSize,
                            dwDecompressedSize = dwDecompressedSize,
                            wCompressionType = PakUtils.iGetCompressionType(wCompressionType),
                            dwDependencyHash = dwDependencyHash,
                        };

                        m_EntryTable.Add(TEntry);
                    }
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = PakHashList.iGetNameFromHashList(m_Entry.dwHashNameLower, m_Entry.dwHashNameUpper);
                    String m_FullPath = m_DstFolder + m_FileName.Replace("/", @"\");

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TPakStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    if (m_Entry.wCompressionType == PakFlags.NONE)
                    {
                        var lpBuffer = TPakStream.ReadBytes(PakUtils.iGetSize(m_Entry.dwCompressedSize, m_Entry.dwDecompressedSize));
                        m_FullPath = PakUtils.iDetectFileType(m_FullPath, lpBuffer);

                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                    else if (m_Entry.wCompressionType == PakFlags.DEFLATE || m_Entry.wCompressionType == PakFlags.ZSTD)
                    {
                        var lpSrcBuffer = TPakStream.ReadBytes((Int32)m_Entry.dwCompressedSize);
                        
                        if (m_Entry.wCompressionType == PakFlags.DEFLATE)
                        {
                            var lpDstBuffer = DEFLATE.iDecompress(lpSrcBuffer);
                            m_FullPath = PakUtils.iDetectFileType(m_FullPath, lpDstBuffer);
                            
                            File.WriteAllBytes(m_FullPath, lpDstBuffer);
                        }
                        else if (m_Entry.wCompressionType == PakFlags.ZSTD)
                        {
                            var lpDstBuffer = ZSTD.iDecompress(lpSrcBuffer);
                            m_FullPath = PakUtils.iDetectFileType(m_FullPath, lpDstBuffer);
                            
                            File.WriteAllBytes(m_FullPath, lpDstBuffer);
                        }
                    }
                    else
                    {
                        Utils.iSetError("[ERROR]: Unknown compression id detected -> " + m_Entry.wCompressionType.ToString());
                        return;
                    }
                }

                TPakStream.Dispose();
            }
        }
    }
}

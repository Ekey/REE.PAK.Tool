using System;
using System.IO;
using System.Collections.Generic;

namespace REE.Unpacker
{
    class PakUnpack
    {
        static List<PakEntry> m_EntryTable = new List<PakEntry>();
        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TFileStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TFileStream.ReadBytes(16);
                var m_Header = new PakHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwMagic = THeaderReader.ReadUInt32(); // KPKA
                    m_Header.dwVersion = THeaderReader.ReadInt32(); // 4
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();
                    m_Header.dwHash = THeaderReader.ReadUInt32();

                    if (m_Header.dwMagic != 0x414B504B)
                    {
                        Utils.iSetError("[ERROR]: Invalid magic of PAK archive file");
                        return;
                    }

                    if (m_Header.dwVersion != 4)
                    {
                        Utils.iSetError("[ERROR]: Invalid version of PAK archive file -> " + m_Header.dwVersion.ToString() + ", expected 4");
                        return;
                    }

                    THeaderReader.Dispose();
                }

                m_EntryTable.Clear();
                var lpTable = TFileStream.ReadBytes(m_Header.dwTotalFiles * 48);
                using (var TMemoryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        UInt32 dwHashNameLower = TMemoryReader.ReadUInt32();
                        UInt32 dwHashNameUpper = TMemoryReader.ReadUInt32();
                        Int64 dwOffset = TMemoryReader.ReadInt64();
                        Int64 dwCompressedSize = TMemoryReader.ReadInt64();
                        Int64 dwDecompressedSize = TMemoryReader.ReadInt64();
                        Int64 wCompressionType = TMemoryReader.ReadInt64();
                        UInt32 dwCRC = TMemoryReader.ReadUInt32();
                        UInt32 dwUnknown = TMemoryReader.ReadUInt32();

                        var Entry = new PakEntry
                        {
                            dwHashNameLower = dwHashNameLower,
                            dwHashNameUpper = dwHashNameUpper,
                            dwOffset = dwOffset,
                            dwCompressedSize = dwCompressedSize,
                            dwDecompressedSize = dwDecompressedSize,
                            wCompressionType = PakUtils.iGetCompressionType(wCompressionType),
                            dwCRC = dwCRC,
                            dwUnknown = dwUnknown,
                        };

                        m_EntryTable.Add(Entry);
                    }
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = PakHashList.iGetNameFromHashList(m_Entry.dwHashNameLower);
                    String m_FullPath = m_DstFolder + m_FileName.Replace("/", @"\");

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TFileStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    if (m_Entry.wCompressionType == PakFlags.NONE)
                    {
                        var lpBuffer = TFileStream.ReadBytes(PakUtils.iGetSize(m_Entry.dwCompressedSize, m_Entry.dwDecompressedSize));
                        m_FullPath = PakUtils.iDetectFileType(m_FullPath, lpBuffer);

                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                    else if (m_Entry.wCompressionType == PakFlags.DEFLATE || m_Entry.wCompressionType == PakFlags.ZSTD)
                    {
                        var lpSrcBuffer = TFileStream.ReadBytes((Int32)m_Entry.dwCompressedSize);
                        
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
            }
        }
    }
}

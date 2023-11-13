using System;
using System.IO;
using System.Collections.Generic;

namespace REE.Unpacker
{
    class PakUnpack40
    {
        private static List<PakEntry40> m_EntryTable = new List<PakEntry40>();

        public static void iDoIt(FileStream TPakStream, PakHeader m_Header, String m_DstFolder)
        {
            var m_SubHeader = new PakSubHeader40();

            m_SubHeader.dwTotalFiles = TPakStream.ReadInt32();
            m_SubHeader.dwHash = TPakStream.ReadUInt32();

            m_EntryTable.Clear();
            var lpTable = TPakStream.ReadBytes(m_SubHeader.dwTotalFiles * 48);

            if (m_Header.wFeature == 8)
            {
                var lpBlobHash = TPakStream.ReadBytes(128);

                lpTable = PakCipher.iDecryptData(lpTable, lpBlobHash);
            }

            using (var TEntryReader = new MemoryStream(lpTable))
            {
                for (Int32 i = 0; i < m_SubHeader.dwTotalFiles; i++)
                {
                    UInt32 dwHashNameLower = TEntryReader.ReadUInt32();
                    UInt32 dwHashNameUpper = TEntryReader.ReadUInt32();
                    Int64 dwOffset = TEntryReader.ReadInt64();
                    Int64 dwCompressedSize = TEntryReader.ReadInt64();
                    Int64 dwDecompressedSize = TEntryReader.ReadInt64();
                    Int64 wCompressionType = TEntryReader.ReadInt64();
                    UInt64 dwChecksum = TEntryReader.ReadUInt64();

                    var TEntry = new PakEntry40
                    {
                        dwHashNameLower = dwHashNameLower,
                        dwHashNameUpper = dwHashNameUpper,
                        dwOffset = dwOffset,
                        dwCompressedSize = dwCompressedSize,
                        dwDecompressedSize = dwDecompressedSize,
                        wCompressionType = PakUtils.iGetCompressionType(wCompressionType),
                        dwChecksum = dwChecksum,
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
        }
    }
}

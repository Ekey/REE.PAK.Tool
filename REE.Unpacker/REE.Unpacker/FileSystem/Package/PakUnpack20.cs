using System;
using System.IO;
using System.Collections.Generic;

namespace REE.Unpacker
{
    class PakUnpack20
    {
        private static List<PakEntry20> m_EntryTable = new List<PakEntry20>();

        public static void iDoIt(FileStream TPakStream, PakHeader m_Header, String m_DstFolder)
        {
            var m_SubHeader = new PakSubHeader20();

            m_SubHeader.dwTotalFiles = TPakStream.ReadInt64();
            m_SubHeader.dwBaseOffset = TPakStream.ReadInt64();
            m_SubHeader.dwUnknown = TPakStream.ReadInt64();

            m_EntryTable.Clear();
            for (Int32 i = 0; i < m_SubHeader.dwTotalFiles; i++)
            {
                UInt32 dwHashNameLower = TPakStream.ReadUInt32();
                UInt32 dwHashNameUpper = TPakStream.ReadUInt32();
                Int64 dwOffset = TPakStream.ReadInt64();
                Int64 dwSize = TPakStream.ReadInt64();

                var TEntry = new PakEntry20
                {
                    dwHashNameLower = dwHashNameLower,
                    dwHashNameUpper = dwHashNameUpper,
                    dwOffset = dwOffset,
                    dwSize = dwSize,
                };

                m_EntryTable.Add(TEntry);
            }

            foreach (var m_Entry in m_EntryTable)
            {
                String m_FileName = PakHashList.iGetNameFromHashList((UInt64)m_Entry.dwHashNameUpper << 32 | m_Entry.dwHashNameLower);
                String m_FullPath = m_DstFolder + m_FileName.Replace("/", @"\");

                Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                Utils.iCreateDirectory(m_FullPath);

                TPakStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                var lpBuffer = TPakStream.ReadBytes((Int32)m_Entry.dwSize);
                m_FullPath = PakUtils.iDetectFileType(m_FullPath, lpBuffer);

                File.WriteAllBytes(m_FullPath, lpBuffer);
            }
        }
    }
}

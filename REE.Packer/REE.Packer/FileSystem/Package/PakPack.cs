using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace REE.Packer
{
    class PakPack
    {
        private static readonly List<PakEntry> m_EntryTable = new List<PakEntry>();

        public static void iDoIt(String m_Archive, String m_SrcFolder, Compression wCompressionType)
        {
            var m_Header = new PakHeader();

            m_Header.dwMagic = 0x414B504B;
            m_Header.dwVersion = 4;
            m_Header.dwTotalFiles = Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories).Length;
            m_Header.dwFingerprint = 0xDEC0ADDE;

            Byte[] lpEntryTable = Enumerable.Repeat((Byte)0, m_Header.dwTotalFiles * 48).ToArray();

            m_EntryTable.Clear();
            using (BinaryWriter TPakWriter = new BinaryWriter(File.Open(m_Archive, FileMode.Create)))
            {
                TPakWriter.Write(m_Header.dwMagic);
                TPakWriter.Write(m_Header.dwVersion);
                TPakWriter.Write(m_Header.dwTotalFiles);
                TPakWriter.Write(m_Header.dwFingerprint);
                TPakWriter.Write(lpEntryTable);

                Int32 dwCounter = 0;
                foreach (String m_File in Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories))
                {
                    var m_Entry = new PakEntry();

                    Console.Title = Program.m_Title + " - " + PakUtils.iPrintInfo(dwCounter++, (Int32)m_Header.dwTotalFiles);

                    String m_FileName = null;
                    if (!m_File.Contains("__Unknown"))
                    {
                        m_FileName = m_File.Replace(m_SrcFolder, "").Replace(@"\", "/");
                        m_Entry.dwHashName = (UInt64)PakHash.iGetStringHash(m_FileName.ToUpper()) << 32 | PakHash.iGetStringHash(m_FileName.ToLower());
                    }
                    else
                    {
                        m_FileName = Path.GetFileNameWithoutExtension(m_File);
                        m_Entry.dwHashName = Convert.ToUInt64(m_FileName, 16);
                    }

                    Console.WriteLine("[PACKING]: {0}", m_FileName);

                    var lpSrcBuffer = File.ReadAllBytes(m_File);

                    m_Entry.dwOffset = TPakWriter.BaseStream.Position;
                    m_Entry.dwChecksum = PakHash.iGetDataHash(lpSrcBuffer);

                    if (lpSrcBuffer.Length >= 8)
                    {
                        UInt32 dwMagic1 = BitConverter.ToUInt32(lpSrcBuffer, 0);
                        UInt32 dwMagic2 = BitConverter.ToUInt32(lpSrcBuffer, 4);

                        // mov, bnk, pck must be uncompressed
                        if (dwMagic1 == 0x75B22630 || dwMagic1 == 0x564D4552 || dwMagic1 == 0x44484B42 || dwMagic1 == 0x4B504B41 || dwMagic2 == 0x70797466)
                        {
                            m_Entry.dwCompressedSize = lpSrcBuffer.Length;
                            m_Entry.dwDecompressedSize = lpSrcBuffer.Length;
                            m_Entry.dwAttributes = (Int64)Compression.NONE;

                            TPakWriter.Write(lpSrcBuffer);
                        }
                        else
                        {
                            var lpDstBuffer = new Byte[] { };

                            switch (wCompressionType)
                            {
                                case Compression.INFLATE: lpDstBuffer = INFLATE.iCompress(lpSrcBuffer); break;
                                case Compression.ZSTD: lpDstBuffer = ZSTD.iCompress(lpSrcBuffer); break;
                            }

                            m_Entry.dwCompressedSize = lpDstBuffer.Length;
                            m_Entry.dwDecompressedSize = lpSrcBuffer.Length;
                            m_Entry.dwAttributes = (Int64)wCompressionType;

                            TPakWriter.Write(lpDstBuffer);
                        }
                    }
                    else
                    {
                        m_Entry.dwCompressedSize = lpSrcBuffer.Length;
                        m_Entry.dwDecompressedSize = lpSrcBuffer.Length;
                        m_Entry.dwAttributes = (Int64)Compression.NONE;

                        TPakWriter.Write(lpSrcBuffer);
                    }

                    m_EntryTable.Add(m_Entry);
                }

                List<PakEntry> m_EntryTableSorted = m_EntryTable.OrderBy(m_Entry => m_Entry.dwHashName).ToList();

                TPakWriter.Seek(16, SeekOrigin.Begin);
                foreach (var m_Entry in m_EntryTableSorted)
                {
                    TPakWriter.Write(m_Entry.dwHashName);
                    TPakWriter.Write(m_Entry.dwOffset);
                    TPakWriter.Write(m_Entry.dwCompressedSize);
                    TPakWriter.Write(m_Entry.dwDecompressedSize);
                    TPakWriter.Write(m_Entry.dwAttributes);
                    TPakWriter.Write(m_Entry.dwChecksum);
                }

                Console.Title = Program.m_Title;

                TPakWriter.Dispose();
            }
        }
    }
}

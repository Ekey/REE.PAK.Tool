using System;
using System.Collections.Generic;
using System.IO;

namespace REE.Unpacker
{
    class PakChunkEntry
    {
        public UInt64 dwChunkOffset { get; set; }
        public UInt32 dwChunkSize { get; set; }
    }

    class PakChunks
    {
        public static List<PakChunkEntry> lpMapTable = new List<PakChunkEntry>();

        //Based on https://github.com/eigeen/ree-pak-rs
        public static void iReadMapTable(Stream TPakStream)
        {
            Int32 dwMaxBlockSize = TPakStream.ReadInt32();
            Int32 dwChunksCount = TPakStream.ReadInt32();

            var dwOffsets = new UInt32[dwChunksCount];
            var dwSizes = new UInt32[dwChunksCount];

            for (int i = 0; i < dwChunksCount; i++)
            {
                dwOffsets[i] = TPakStream.ReadUInt32();
                dwSizes[i] = TPakStream.ReadUInt32();
            }

            UInt64 dwHigh = 0;
            UInt32 dwPrevOffset = 0;

            lpMapTable.Clear();
            for (Int32 i = 0; i < dwChunksCount; i++)
            {
                if (i > 0 && dwOffsets[i] < dwPrevOffset)
                {
                    dwHigh += 1UL << 32;
                }

                var m_ChunkEntry = new PakChunkEntry();

                m_ChunkEntry.dwChunkOffset = dwHigh | dwOffsets[i];
                m_ChunkEntry.dwChunkSize = dwSizes[i] >> 10;

                lpMapTable.Add(m_ChunkEntry);

                dwPrevOffset = dwOffsets[i];
            }
        }

        public static Byte[] iUnwrapChunks(FileStream TPakStream, PakEntry m_Entry)
        {
            const Int32 dwMaxChunkSize = 524288;
            Int64 dwRemainSize = m_Entry.dwCompressedSize;
            Int32 dwChunkId = (Int32)m_Entry.dwOffset;

            using (var TMemoryStream = new MemoryStream())
            {
                while (dwRemainSize > 0)
                {
                    Int64 dwChunkOffset = (Int64)lpMapTable[dwChunkId].dwChunkOffset;
                    Int32 dwChunkSize = (Int32)lpMapTable[dwChunkId].dwChunkSize;

                    TPakStream.Seek(dwChunkOffset, SeekOrigin.Begin);
                    var lpSrcBuffer = TPakStream.ReadBytes(dwChunkSize);

                    if (dwChunkSize == dwMaxChunkSize)
                    {
                        TMemoryStream.Write(lpSrcBuffer, 0, lpSrcBuffer.Length);
                    }
                    else
                    {
                        var lpDstBuffer = ZSTD.iDecompress(lpSrcBuffer);
                        TMemoryStream.Write(lpDstBuffer, 0, lpDstBuffer.Length);
                    }

                    dwChunkId++;
                    dwRemainSize -= dwChunkSize;
                }

                TMemoryStream.SetLength((Int32)m_Entry.dwDecompressedSize);

                return TMemoryStream.ToArray();
            }
        }
    }
}
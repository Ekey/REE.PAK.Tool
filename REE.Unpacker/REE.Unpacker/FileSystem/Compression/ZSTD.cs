using System;
using System.IO;
using System.IO.Compression;

using Zstandard.Net;

namespace REE.Unpacker
{
    class ZSTD
    {
        public static Byte[] iDecompress(Byte[] lpSrcBuffer)
        {
            Byte[] lpDstBuffer;
            using (MemoryStream TSrcStream = new MemoryStream(lpSrcBuffer))
            {
                using (var TZstandardStream = new ZstandardStream(TSrcStream, CompressionMode.Decompress))
                using (var TDstStream = new MemoryStream())
                {
                    TZstandardStream.CopyTo(TDstStream);
                    lpDstBuffer = TDstStream.ToArray();
                }
            }
            return lpDstBuffer;
        }
    }
}

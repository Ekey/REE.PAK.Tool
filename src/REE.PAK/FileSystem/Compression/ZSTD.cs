using System;
using System.IO;
using ZstdSharp;

namespace REE
{
    class ZSTD
    {
        public static Byte[] iCompress(Byte[] lpBuffer)
        {
            var TInputMemoryStream = new MemoryStream(lpBuffer);
            using (var TOutputMemoryStream = new MemoryStream())
            using (var TZstandardStream = new CompressionStream(TOutputMemoryStream))
            {
                TInputMemoryStream.CopyTo(TZstandardStream);
                TZstandardStream.Close();
                return TOutputMemoryStream.ToArray();
            }
        }

        public static Byte[] iDecompress(Byte[] lpSrcBuffer)
        {
            Byte[] lpDstBuffer;
            using (MemoryStream TSrcStream = new MemoryStream(lpSrcBuffer))
            {
                using (var TZstandardStream = new DecompressionStream(TSrcStream))
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

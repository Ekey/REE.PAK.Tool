using System;
using System.IO;
using System.IO.Compression;

using Zstandard.Net;

namespace REE.Packer
{
    class ZSTD
    {
        public static Byte[] iCompress(Byte[] lpBuffer)
        {
            var TInputMemoryStream = new MemoryStream(lpBuffer);
            using (var TOutputMemoryStream = new MemoryStream())
            using (var TZstandardStream = new ZstandardStream(TOutputMemoryStream, CompressionMode.Compress))
            {
                TInputMemoryStream.CopyTo(TZstandardStream);
                TZstandardStream.Close();
                return TOutputMemoryStream.ToArray();
            }
        }
    }
}

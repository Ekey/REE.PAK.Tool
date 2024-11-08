using System;
using System.IO;
using System.IO.Compression;

namespace REE
{
    class INFLATE
    {
        public static Byte[] iCompress(Byte[] lpBuffer)
        {
            var TInputMemoryStream = new MemoryStream(lpBuffer);
            using (var TOutputMemoryStream = new MemoryStream())
            using (var TDeflateStream = new DeflateStream(TOutputMemoryStream, CompressionMode.Compress))
            {
                TInputMemoryStream.CopyTo(TDeflateStream);
                TDeflateStream.Close();
                return TOutputMemoryStream.ToArray();
            }
        }
    }
}

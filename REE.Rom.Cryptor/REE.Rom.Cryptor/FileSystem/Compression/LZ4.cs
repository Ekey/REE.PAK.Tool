using System;
using LZ4Sharp;

namespace REE.Rom.Cryptor
{
    class LZ4
    {
        public static byte[] iDecompress(Byte[] lpBuffer)
        {
            LZ4Decompressor64 TLZ4Decompressor64 = new LZ4Decompressor64();
            var lpDstBuffer = TLZ4Decompressor64.Decompress(lpBuffer);

            return lpDstBuffer;
        }
    }
}

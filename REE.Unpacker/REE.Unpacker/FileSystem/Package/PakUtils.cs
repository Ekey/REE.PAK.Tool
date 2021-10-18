using System;

namespace REE.Unpacker
{
    class PakUtils
    {
        public static Int32 iGetSize(Int64 dwCompressedSize, Int64 dwDecompressedSize)
        {
            return Convert.ToInt32(Math.Max(dwCompressedSize, dwDecompressedSize));
        }

        public static PakFlags iGetCompressionType(Int64 dwFlag)
        {
            if (Convert.ToInt32(dwFlag & 0xF) == 1)
            {
                if (dwFlag >> 16 > 0) { return PakFlags.NONE; } else { return PakFlags.DEFLATE; }
            }
            else if (Convert.ToInt32(dwFlag & 0xF) == 2)
            {
                if (dwFlag >> 16 > 0) { return PakFlags.NONE; } else { return PakFlags.ZSTD; }
            }
            else
            {
                return PakFlags.NONE;
            }
        }
    }
}

using System;

namespace REE
{
    [Flags]
    public enum CompressionType : Byte
    {
        NONE = 0,
        DEFLATE = 1,
        INFLATE = 1,
        ZSTD = 2,
    }
}

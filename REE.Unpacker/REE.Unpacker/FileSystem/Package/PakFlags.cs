using System;

namespace REE.Unpacker
{
    [Flags]
    public enum CompressionType : Byte
    {
        NONE = 0,
        DEFLATE = 1,
        ZSTD = 2,
    }
}

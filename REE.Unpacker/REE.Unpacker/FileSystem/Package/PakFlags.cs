using System;

namespace REE.Unpacker
{
    [Flags]
    public enum PakFlags : Int64
    {
        NONE = 0,
        DEFLATE = 1,
        ZSTD = 2
    }
}

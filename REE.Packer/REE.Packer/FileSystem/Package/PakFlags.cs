using System;

namespace REE.Packer
{
    [Flags]
    public enum PakFlags : Int64
    {
        NONE = 0,
        INFLATE = 1,
        ZSTD = 2,
    }
}

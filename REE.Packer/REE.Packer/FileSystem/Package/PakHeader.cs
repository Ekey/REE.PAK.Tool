using System;

namespace REE.Packer
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } //0x414B504B (KPKA)
        public Int32 dwVersion { get; set; } //2, 4
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwHash { get; set; }
    }
}

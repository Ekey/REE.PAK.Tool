using System;

namespace REE
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } //0x414B504B (KPKA)
        public Byte bMajorVersion { get; set; } // 2 (Kitchen Demo PS4), 4
        public Byte bMinorVersion { get; set; } // 0
        public Int16 wFeature { get; set; } // 0, 8 (Encrypted -> Monster Hunter Rise & Monster Hunter Rise - Sunbreak Demo)
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwHash { get; set; }
    }
}

using System;

namespace REE.Unpacker
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } //0x414B504B (KPKA)
        public Int32 bMajorVersion { get; set; } // 2 (Kitchen Demo PS4), 4
        public Int32 bMinorVersion { get; set; } // 0
        public Int16 wFeature { get; set; } // 0, 8 (Encrypted -> Monster Hunter Rise & Monster Hunter Rise - Sunbreak Demo)
    }

    class PakSubHeader20
    {
        public Int64 dwTotalFiles { get; set; }
        public Int64 dwBaseOffset { get; set; }
        public Int64 dwUnknown { get; set; } // 450 (Kitchen Demo PS4)
    }

    class PakSubHeader40
    {
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwHash { get; set; }
    }
}

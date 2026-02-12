using System;
using System.Collections.Generic;

namespace REE.Unpacker
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } //0x414B504B (KPKA)
        public Int32 bMajorVersion { get; set; } // 2 (Kitchen Demo PS4), 4
        public Int32 bMinorVersion { get; set; } // 0, 1, 2
        public Features wFeature { get; set; } // 0, 8 (Encrypted -> PKC), 24 (empty integer value), 40 (Mapped table)
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwFingerprint { get; set; }
    }
}

using System;

namespace REE.Packer
{
    class PakEntry
    {
        public UInt64 dwHashName { get; set; } // Lower & Upper case
        public Int64 dwOffset { get; set; }
        public Int64 dwCompressedSize { get; set; }
        public Int64 dwDecompressedSize { get; set; }
        public PakFlags wCompressionType { get; set; }
        public UInt64 dwChecksum { get; set; }
    }
}

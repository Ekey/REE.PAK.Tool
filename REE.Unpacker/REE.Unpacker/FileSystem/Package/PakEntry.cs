using System;

namespace REE.Unpacker
{
    class PakEntry
    {
        public UInt32 dwHashNameLower { get; set; }
        public UInt32 dwHashNameUpper { get; set; }
        public Int64 dwOffset { get; set; }
        public Int64 dwCompressedSize { get; set; }
        public Int64 dwDecompressedSize { get; set; }
        public PakFlags wCompressionType { get; set; }
        public UInt64 dwChecksum { get; set; }
    }
}

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
        public Int64 dwAttributes { get; set; }
        public UInt64 dwChecksum { get; set; }
        public Compression wCompressionType { get; set; }
        public Encryption wEncryptionType { get; set; }
    }
}

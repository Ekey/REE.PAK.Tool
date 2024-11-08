using System;

namespace REE
{
    class PakEntry
    {
        public UInt32 dwHashNameLower { get; set; }
        public UInt32 dwHashNameUpper { get; set; }
        public Int64 dwOffset { get; set; }
        public Int64 dwCompressedSize { get; set; }
        public Int64 dwDecompressedSize { get; set; }
        public CompressionType wCompressionType { get; set; }
        public Int32 wCompressionFlags { get; set; }
        public Int32 wEncryptionType { get; set; }
        public Int32 wEncryptionFlags { get; set; }
        public Int32 dwReserved { get; set; }
        public UInt64 dwChecksum { get; set; }

        public UInt64 dwHashName
        {
            get => ((ulong)dwHashNameUpper << 32) | dwHashNameLower;
            set
            {
                dwHashNameLower = (uint)(value & 0xFFFFFFFF);
                dwHashNameUpper = (uint)(value >> 32);
            }
        }
    }
}

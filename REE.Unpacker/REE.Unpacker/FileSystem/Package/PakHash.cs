using System;
using System.IO;
using System.Text;

namespace REE.Unpacker
{
    class PakHash
    {
        private static UInt32 MurMur3Hash(Stream stream)
        {
            const UInt32 c1 = 0xcc9e2d51;
            const UInt32 c2 = 0x1b873593;
            const UInt32 seed = 0xffffffff;

            UInt32 h1 = seed;
            UInt32 k1 = 0;
            UInt32 streamLength = 0;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                Byte[] chunk = reader.ReadBytes(4);
                while (chunk.Length > 0)
                {
                    streamLength += (UInt32)chunk.Length;
                    switch (chunk.Length)
                    {
                        case 4:
                            k1 = (UInt32)(chunk[0] | chunk[1] << 8 | chunk[2] << 16 | chunk[3] << 24);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            h1 = rotl32(h1, 13);
                            h1 = h1 * 5 + 0xe6546b64;
                            break;
                        case 3:
                            k1 = (UInt32)(chunk[0] | chunk[1] << 8 | chunk[2] << 16);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        case 2:
                            k1 = (UInt32)(chunk[0] | chunk[1] << 8);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        case 1:
                            k1 = (UInt32)(chunk[0]);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;

                    }
                    chunk = reader.ReadBytes(4);
                }
            }

            h1 ^= streamLength;
            h1 = fmix(h1);

            unchecked
            {
                return h1;
            }
        }

        private static UInt32 rotl32(UInt32 x, Byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static UInt32 fmix(UInt32 h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }

        public static UInt32 iGetHash(String m_String)
        {
            UInt32 dwHash = 0;
            Byte[] lpBuffer = Encoding.Unicode.GetBytes(m_String);

            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer))
            {
                dwHash = MurMur3Hash(TMemoryStream);
            }
            return dwHash;
        }
    }
}

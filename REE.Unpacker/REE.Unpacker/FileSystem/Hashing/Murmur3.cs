using System;
using System.IO;

namespace REE.Unpacker
{
    class Murmur3
    {
        public static UInt32 HashCore32(Stream stream, UInt32 seed)
        {
            const UInt32 c1 = 0xcc9e2d51;
            const UInt32 c2 = 0x1b873593;

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
            h1 = fmix32(h1);

            unchecked
            {
                return h1;
            }
        }

        public static UInt64[] HashCore64(Byte[] lpBuffer, UInt32 seed)
        {
            const UInt64 c1 = 0x87c37b91114253d5;
            const UInt64 c2 = 0x4cf5ad432745937f;

            UInt64 k1 = 0;
            UInt64 k2 = 0;

            UInt64 h1 = seed;
            UInt64 h2 = seed;

            Int32 nblocks = (Int32)lpBuffer.Length / 16;

            for (Int32 i = 0; i < nblocks;)
            {
                k1 = BitConverter.ToUInt64(lpBuffer, i++ * 8);
                k2 = BitConverter.ToUInt64(lpBuffer, i++ * 8);

                k1 *= c1;
                k1 = rotl64(k1, 31);
                k1 *= c2;
                h1 ^= k1;

                h1 = rotl64(h1, 27);
                h1 += h2;
                h1 = h1 * 5 + 0x52dce729;

                k2 *= c2;
                k2 = rotl64(k2, 33);
                k2 *= c1;
                h2 ^= k2;

                h2 = rotl64(h2, 31);
                h2 += h1;
                h2 = h2 * 5 + 0x38495ab5;
            }

            k1 = 0;
            k2 = 0;

            switch (lpBuffer.Length % 16)
            {
                case 15:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 14]) << 48;
                    goto case 14;
                case 14:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 13]) << 40;
                    goto case 13;
                case 13:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 12]) << 32;
                    goto case 12;
                case 12:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 11]) << 24;
                    goto case 11;
                case 11:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 10]) << 16;
                    goto case 10;
                case 10:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 9]) << 8;
                    goto case 9;
                case 9:
                    k2 ^= ((UInt64)lpBuffer[nblocks * 16 + 8]) << 0;
                    k2 *= c2;
                    k2 = rotl64(k2, 33);
                    k2 *= c1;
                    h2 ^= k2;
                    goto case 8;
                case 8:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 7]) << 56;
                    goto case 7;
                case 7:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 6]) << 48;
                    goto case 6;
                case 6:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 5]) << 40;
                    goto case 5;
                case 5:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 4]) << 32;
                    goto case 4;
                case 4:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 3]) << 24;
                    goto case 3;
                case 3:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 2]) << 16;
                    goto case 2;
                case 2:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16 + 1]) << 8;
                    goto case 1;
                case 1:
                    k1 ^= ((UInt64)lpBuffer[nblocks * 16]) << 0;
                    k1 *= c1;
                    k1 = rotl64(k1, 31);
                    k1 *= c2; h1 ^= k1;
                    break;
            };

            h1 ^= (UInt32)lpBuffer.Length;
            h2 ^= (UInt32)lpBuffer.Length;

            h1 += h2;
            h2 += h1;

            h1 = fmix64(h1);
            h2 = fmix64(h2);

            h1 += h2;
            h2 += h1;
            return new[] { h1, h2 };
        }

        private static UInt32 rotl32(UInt32 x, Byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static UInt64 rotl64(UInt64 x, Byte r)
        {
            return (x << r) | (x >> (64 - r));
        }

        private static UInt32 fmix32(UInt32 h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        private static UInt64 fmix64(UInt64 k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;

            return k;
        }

        public static UInt32 iGetHash32(Byte[] lpBuffer, UInt32 dwSeed = 0xFFFFFFFF)
        {
            UInt32 dwHash = 0;

            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer))
            {
                dwHash = HashCore32(TMemoryStream, dwSeed);
            }
            return dwHash;
        }

        public static UInt64[] iGetHash64(Byte[] lpBuffer, UInt32 dwSeed = 0xFFFFFFFF)
        {
            return HashCore64(lpBuffer, dwSeed);
        }
    }
}

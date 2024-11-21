using System;

namespace REE.Packer
{
    class xxHash
    {
        private static readonly UInt32 PRIME32_1 = 0x9E3779B1;
        private static readonly UInt32 PRIME32_2 = 0x85EBCA77;
        private static readonly UInt32 PRIME32_3 = 0xC2B2AE3D;
        private static readonly UInt32 PRIME32_4 = 0x27D4EB2F;
        private static readonly UInt32 PRIME32_5 = 0x165667B1;

        private static readonly UInt64 PRIME64_1 = 0x9E3779B185EBCA87;
        private static readonly UInt64 PRIME64_2 = 0xC2B2AE3D27D4EB4F;
        private static readonly UInt64 PRIME64_3 = 0x165667B19E3779F9;
        private static readonly UInt64 PRIME64_4 = 0x85EBCA77C2B2AE63;
        private static readonly UInt64 PRIME64_5 = 0x27D4EB2F165667C5;

        private static UInt32 rotl32(UInt32 x, Byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static UInt64 rotl64(UInt64 x, Byte r)
        {
            return (x << r) | (x >> (64 - r));
        }

        private static UInt32 round32(UInt32 dwAccumulator, UInt32 dwValue)
        {
            dwAccumulator += dwValue * PRIME32_2;
            dwAccumulator = rotl32(dwAccumulator, 13);
            dwAccumulator *= PRIME32_1;

            return dwAccumulator;
        }

        private static UInt64 round64(UInt64 dwAccumulator, UInt64 dwValue)
        {
            dwAccumulator += dwValue * PRIME64_2;
            dwAccumulator = rotl64(dwAccumulator, 31);
            dwAccumulator *= PRIME64_1;

            return dwAccumulator;
        }

        private static UInt64 merge64(UInt64 dwAccumulator, UInt64 dwValue)
        {
            dwValue = round64(0, dwValue);
            dwAccumulator ^= dwValue;
            dwAccumulator = dwAccumulator * PRIME64_1 + PRIME64_4;
            return dwAccumulator;
        }

        private static UInt32 HashCore32(Byte[] lpBuffer, UInt32 dwSeed, UInt32 dwHash = 0)
        {
            Int32 dwOffset = 0;
            Int32 dwEnd = lpBuffer.Length;

            if (lpBuffer.Length < 16)
            {
                dwHash = dwSeed + PRIME32_5;
            }
            else
            {
                UInt32 v1 = dwSeed + PRIME32_1 + PRIME32_2;
                UInt32 v2 = dwSeed + PRIME32_2;
                UInt32 v3 = dwSeed + 0;
                UInt32 v4 = dwSeed - PRIME32_1;

                while (dwOffset <= dwEnd - 16)
                {
                    v1 = round32(v1, BitConverter.ToUInt32(lpBuffer, dwOffset)); dwOffset += 4;
                    v2 = round32(v2, BitConverter.ToUInt32(lpBuffer, dwOffset)); dwOffset += 4;
                    v3 = round32(v3, BitConverter.ToUInt32(lpBuffer, dwOffset)); dwOffset += 4;
                    v4 = round32(v4, BitConverter.ToUInt32(lpBuffer, dwOffset)); dwOffset += 4;
                }

                dwHash = rotl32(v1, 1) + rotl32(v2, 7) + rotl32(v3, 12) + rotl32(v4, 18);
            }

            dwHash += (UInt32)lpBuffer.Length;

            while (dwOffset + 4 <= dwEnd)
            {
                dwHash += BitConverter.ToUInt32(lpBuffer, dwOffset) * PRIME32_3;
                dwHash = rotl32(dwHash, 17) * PRIME32_4; dwOffset += 4;
            }

            while (dwOffset < dwEnd)
            {
                dwHash += (UInt32)(lpBuffer[dwOffset] & 0xFF) * PRIME32_5;
                dwHash = rotl32(dwHash, 11) * PRIME32_1; dwOffset += 1;
            }

            dwHash ^= dwHash >> 15;
            dwHash *= PRIME32_2;
            dwHash ^= dwHash >> 13;
            dwHash *= PRIME32_3;
            dwHash ^= dwHash >> 16;

            return dwHash;
        }

        private static UInt64 HashCore64(Byte[] lpBuffer, UInt64 dwSeed, UInt64 dwHash = 0)
        {
            Int32 dwOffset = 0;
            Int32 dwEnd = lpBuffer.Length;

            if (lpBuffer.Length < 32)
            {
                dwHash = dwSeed + PRIME64_5;
            }
            else
            {
                UInt64 v1 = dwSeed + PRIME64_1 + PRIME64_2;
                UInt64 v2 = dwSeed + PRIME64_2;
                UInt64 v3 = dwSeed + 0;
                UInt64 v4 = dwSeed - PRIME64_1;

                while (dwOffset <= dwEnd - 32)
                {
                    v1 = round64(v1, BitConverter.ToUInt64(lpBuffer, dwOffset)); dwOffset += 8;
                    v2 = round64(v2, BitConverter.ToUInt64(lpBuffer, dwOffset)); dwOffset += 8;
                    v3 = round64(v3, BitConverter.ToUInt64(lpBuffer, dwOffset)); dwOffset += 8;
                    v4 = round64(v4, BitConverter.ToUInt64(lpBuffer, dwOffset)); dwOffset += 8;
                }

                dwHash = rotl64(v1, 1) + rotl64(v2, 7) + rotl64(v3, 12) + rotl64(v4, 18);

                dwHash = merge64(dwHash, v1);
                dwHash = merge64(dwHash, v2);
                dwHash = merge64(dwHash, v3);
                dwHash = merge64(dwHash, v4);
            }

            dwHash += (UInt64)lpBuffer.Length;

            while (dwOffset + 8 <= dwEnd)
            {
                dwHash ^= round64(0, BitConverter.ToUInt64(lpBuffer, dwOffset));
                dwHash = rotl64(dwHash, 27) * PRIME64_1 + PRIME64_4; dwOffset += 8;
            }

            if (dwOffset + 4 <= dwEnd)
            {
                dwHash ^= BitConverter.ToUInt32(lpBuffer, dwOffset) * PRIME64_1;
                dwHash = rotl64(dwHash, 23) * PRIME64_2 + PRIME64_3; dwOffset += 4;
            }

            while (dwOffset < dwEnd)
            {
                dwHash ^= (UInt64)(lpBuffer[dwOffset] & 0xFF) * PRIME64_5;
                dwHash = rotl64(dwHash, 11) * PRIME64_1; dwOffset += 1;
            }

            dwHash ^= dwHash >> 33;
            dwHash *= PRIME64_2;
            dwHash ^= dwHash >> 29;
            dwHash *= PRIME64_3;
            dwHash ^= dwHash >> 32;

            return dwHash;
        }


        public static UInt32 iGetHash32(Byte[] lpBuffer, UInt32 dwSeed = 0xFFFFFFFF)
        {
            return HashCore32(lpBuffer, dwSeed);
        }

        public static UInt64 iGetHash64(Byte[] lpBuffer, UInt64 dwSeed = 0xFFFFFFFF)
        {
            return HashCore64(lpBuffer, dwSeed);
        }
    }
}

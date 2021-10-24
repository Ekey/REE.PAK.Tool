using System;

namespace REE.FontsCryptor
{
    class FontCipher
    {
        public static Byte[] iCryptData(Byte[] lpBuffer)
        {
            UInt64 dwSeed = 1;
            UInt64 dwDelta = 0xAE6E39B58A355F45u;

            Int32 dwSize = lpBuffer.Length & 0x3F;
            if (dwSize > 0)
            {
                for (Int32 i = 0; i < dwSize; i++)
                {
                    dwSeed = 2 * dwSeed + 1;
                }
            }

            UInt64 dwKey = (dwDelta >> dwSize) | ((dwSeed & dwDelta) << (64 - (Byte)dwSize));

            if (lpBuffer.Length > 0)
            {
                Byte[] lpKey = BitConverter.GetBytes(dwKey);
                for (Int32 i = 0; i < lpBuffer.Length; i++)
                {
                    lpBuffer[i] ^= lpKey[i % 8];
                }
            }

            return lpBuffer;
        }
    }
}

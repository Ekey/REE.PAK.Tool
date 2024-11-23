using System;

namespace REE.Rom.Cryptor
{
    class MameCipher
    {
        public static Byte[] iCryptData(Byte[] lpBuffer, String m_NativePath)
        {
            Byte[] lpKey = new Byte[8];

            lpKey[0] = (Byte)m_NativePath[3];
            lpKey[2] = (Byte)m_NativePath[1];
            lpKey[4] = (Byte)m_NativePath[0];
            lpKey[6] = (Byte)m_NativePath[2];
            lpKey[1] = 0xB5;
            lpKey[3] = 0x29;
            lpKey[5] = 0x6C;
            lpKey[7] = 0x96;

            Int32 j = 0;
            for (Int32 i = 4; i < m_NativePath.Length; i++, j++)
            {
                lpKey[j % 8] ^= (Byte)m_NativePath[i];
            }

            j = 0;
            for (Int32 i = 0; i < lpBuffer.Length; i++, j++)
            {
                lpBuffer[i] ^= lpKey[j % 8];
            }

            return lpBuffer;
        }
    }
}

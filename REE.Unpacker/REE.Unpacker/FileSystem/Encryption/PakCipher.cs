using System;
using System.Security.Cryptography;

namespace REE.Unpacker
{
    class PakCipher
    {
        //MONSTER HUNTER RISE
        private static readonly Byte[] m_K0 = new Byte[] {
            0xEF, 0x7C, 0x6C, 0xE3, 0x6C, 0xA0, 0x44, 0x35, 0xB9, 0x9C, 0x8E, 0x15, 0x66, 0xBD, 0xFF, 0x9D,
            0xE1, 0x7E, 0x51, 0xF8, 0x1F, 0x06, 0xE4, 0x32, 0x3E, 0x5E, 0xFF, 0xDE, 0x7E, 0x50, 0x6B, 0x97
        };

        private static readonly Byte[] m_K1 = new Byte[] {
            0x3B, 0x60, 0x52, 0xF8, 0xD0, 0x22, 0x95, 0x3A, 0xEC, 0xC5, 0x7A, 0x6F, 0x27, 0x8D, 0xDF, 0x7B,
            0xBD, 0xD2, 0x90, 0x38, 0x3D, 0x3A, 0x41, 0xA9, 0xFE, 0x85, 0xDA, 0xE0, 0x23, 0xFC, 0x4E, 0xCA,
        };

        //MONSTER HUNTER RISE: SUNBREAK DEMO
        private static readonly Byte[] m_K2 = new Byte[] {
            0x4D, 0xED, 0x87, 0xB8, 0x97, 0x01, 0x0C, 0x45, 0xC2, 0x07, 0xED, 0xD1, 0xC2, 0x38, 0xF4, 0x4E,
            0x21, 0x7A, 0x5F, 0xBE, 0xB0, 0x33, 0xB7, 0xF0, 0x57, 0x88, 0x1E, 0x68, 0x25, 0xB9, 0x78, 0x86
        };

        public static Byte[] iGetEncryptionKey(Byte[] lpBlobHash)
        {
            SHA1 TSHA1 = new SHA1CryptoServiceProvider();

            var lpHash = TSHA1.ComputeHash(lpBlobHash);

            switch (PakUtils.iGetStringFromBytes(lpHash))
            {
                //MONSTER HUNTER RISE -> re_chunk_000.pak
                case "149194BE6355C5FB7C35557596BB69F46E8F8994": return m_K0;
                //MONSTER HUNTER RISE -> re_chunk_000.pak.patch_001.pak
                case "3A493CC9974657CA7C1CA51BC8C88431BEFAE056": return m_K1;
                //MONSTER HUNTER RISE: SUNBREAK DEMO -> re_chunk_000.pak
                case "ED7B8EEE0AA3C6674736A05698D2CF870B81C99D": return m_K2;
                default: break;
            }

            return new Byte[0];
        }

        public static Byte[] iDecryptData(Byte[] lpBuffer, Byte[] lpBlobHash)
        {
            var lpKey = iGetEncryptionKey(lpBlobHash);

            if (lpKey.Length > 0)
            {
                for (Int32 i = 0; i < lpBuffer.Length; i++)
                {
                    lpBuffer[i] ^= (Byte)(i + lpKey[i % 32] * lpKey[i % 29]);
                }
            }

            return lpBuffer;
        }
    }
}

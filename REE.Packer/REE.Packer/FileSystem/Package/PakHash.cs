using System;
using System.IO;
using System.Text;

namespace REE.Packer
{
    class PakHash
    {
        public static UInt32 iGetStringHash(String m_String, UInt32 dwSeed = 0xFFFFFFFF)
        {
            UInt32 dwHash = 0;
            Byte[] lpBuffer = Encoding.Unicode.GetBytes(m_String);

            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer))
            {
                dwHash = Murmur3.HashCore32(TMemoryStream, dwSeed);
            }
            return dwHash;
        }

        public static UInt64 iGetDataHash(Byte[] lpBuffer, UInt32 dwSeed = 0xFFFFFFFF)
        {
            UInt64 dwHash64 = xxHash.iGetHash64(lpBuffer, dwSeed);
            UInt64 dwHash32 = xxHash.iGetHash32(BitConverter.GetBytes(dwHash64), dwSeed);

            return (UInt64)dwHash64 << 32 | dwHash32;
        }
    }
}

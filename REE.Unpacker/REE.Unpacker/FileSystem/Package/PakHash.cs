using System;
using System.IO;
using System.Text;

namespace REE.Unpacker
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
    }
}

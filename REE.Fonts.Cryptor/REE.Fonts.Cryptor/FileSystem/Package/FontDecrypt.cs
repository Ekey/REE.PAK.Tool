using System;
using System.IO;

namespace REE.Fonts.Cryptor
{
    class FontDecrypt
    {
        public static void iDoIt(FileStream TFontsReader, String m_DstFile)
        {
            var lpBuffer = TFontsReader.ReadBytes((Int32)TFontsReader.Length - 4);
            lpBuffer = FontCipher.iCryptData(lpBuffer);

            File.WriteAllBytes(m_DstFile, lpBuffer);
            
            TFontsReader.Dispose();
        }
    }
}

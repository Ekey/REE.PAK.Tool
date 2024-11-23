using System;
using System.IO;

namespace REE.Fonts.Cryptor
{
    class FontEncrypt
    {
        public static void iDoIt(FileStream TFontsReader, String m_DstFile)
        {
            TFontsReader.Seek(0, SeekOrigin.Begin);
            var lpBuffer = TFontsReader.ReadBytes((Int32)TFontsReader.Length);
            lpBuffer = FontCipher.iCryptData(lpBuffer);

            using (BinaryWriter TFontsWriter = new BinaryWriter(File.Open(m_DstFile, FileMode.Create)))
            {
                TFontsWriter.Write(0x4F464246);
                TFontsWriter.Write(lpBuffer);
                TFontsWriter.Dispose();
            }

            TFontsReader.Dispose();
        }
    }
}

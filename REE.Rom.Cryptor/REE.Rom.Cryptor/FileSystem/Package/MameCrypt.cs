using System;
using System.IO;

namespace REE.Rom.Cryptor
{
    class MameCrypt
    {
        public static void iDoIt(String m_SrcFile, String m_DstFile)
        {
            String m_NativePath = String.Empty;
            String m_FileName = Path.GetFileName(m_SrcFile);

            if (!m_FileName.Contains("_d."))
            {
                m_NativePath = "natives/STM/streaming/Roms/" + Path.GetFileName(m_SrcFile);
            }
            else
            {
                m_NativePath = "natives/STM/streaming/Roms/DecryptionRom/" + Path.GetFileName(m_SrcFile).Replace("_d", "_D");
            }

            var lpBuffer = File.ReadAllBytes(m_SrcFile);

            MameCipher.iCryptData(lpBuffer, m_NativePath);

            if (lpBuffer[0] >> 4 <= 0xF)
            {
                var lpTemp = LZ4.iDecompress(lpBuffer);
                File.WriteAllBytes(m_DstFile, lpTemp);
            }
            else
            {
                Utils.iSetError("[ERROR]: Unable to decrypt data - " + Path.GetFileName(m_SrcFile) + " is not a ROM file");
                return;
            }
        }
    }
}

﻿using System;
using System.IO;

namespace REE.Rom.Cryptor
{
    class MameCrypt
    {
        public static void iDoIt(String m_SrcFile, String m_DstFile)
        {
            String m_NativePath = "natives/STM/streaming/Roms/" + Path.GetFileName(m_SrcFile);

            var lpBuffer = File.ReadAllBytes(m_SrcFile);

            MameCipher.iCryptData(lpBuffer, m_NativePath);

            Int32 bMagic = lpBuffer[0];

            if (bMagic >> 4 == 0xF)
            {
                File.WriteAllBytes(m_DstFile, lpBuffer);
            }
            else
            {
                Utils.iSetError("[ERROR]: Unable to decrypt data - " + Path.GetFileName(m_SrcFile) + " is not a ROM file");
                return;
            }
        }
    }
}
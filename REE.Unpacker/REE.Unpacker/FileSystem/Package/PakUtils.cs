using System;

namespace REE.Unpacker
{
    class PakUtils
    {
        public static Int32 iGetSize(Int64 dwCompressedSize, Int64 dwDecompressedSize)
        {
            return Convert.ToInt32(Math.Max(dwCompressedSize, dwDecompressedSize));
        }

        public static PakFlags iGetCompressionType(Int64 dwFlag)
        {
            if (Convert.ToInt32(dwFlag & 0xF) == 1)
            {
                if (dwFlag >> 16 > 0) { return PakFlags.NONE; } else { return PakFlags.DEFLATE; }
            }
            else if (Convert.ToInt32(dwFlag & 0xF) == 2)
            {
                if (dwFlag >> 16 > 0) { return PakFlags.NONE; } else { return PakFlags.ZSTD; }
            }
            else
            {
                return PakFlags.NONE;
            }
        }

        public static String iDetectFileType(String m_FileName, Byte[] lpBuffer)
        {
            if (m_FileName.Contains(@"__Unknown"))
            {
                if (lpBuffer.Length >= 4)
                {
                    UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, 0);
                    switch (dwMagic)
                    {
                        case 0x1D8: return m_FileName + ".motlist";
                        case 0x444F4C: return m_FileName + ".lod";
                        case 0x424650: return m_FileName + ".pfb";
                        case 0x464453: return m_FileName + ".mmtr";
                        case 0x46444D: return m_FileName + ".mdf2";
                        case 0x4E4353: return m_FileName + ".scn";
                        case 0x504D4C: return m_FileName + ".lmp";
                        case 0x535353: return m_FileName + ".sss";
                        case 0x530040: return m_FileName + ".wel";
                        case 0x584554: return m_FileName + ".tex";
                        case 0x525355: return m_FileName + ".user";
                        case 0x5A5352: return m_FileName + ".wcc";
                        case 0x20464544: return m_FileName + ".def";
                        case 0x4252504E: return m_FileName + ".nprb";
                        case 0x44484B42: return m_FileName + ".bnk";
                        case 0x75B22630: return m_FileName + ".mov";
                        case 0x4853454D: return m_FileName + ".mesh";
                        case 0x4B504B41: return m_FileName + ".pck";
                        case 0x50534552: return m_FileName + ".spmdl";
                        case 0x54564842: return m_FileName + ".fsmv2";
                        case 0x4C4F4352: return m_FileName + ".rcol";
                        case 0x5556532E: return m_FileName + ".uvs";
                        case 0x4C494643: return m_FileName + ".cfil";
                        case 0x54414D43: return m_FileName + ".cmat";
                        case 0x44545254: return m_FileName + ".trtd";
                        case 0x50494C43: return m_FileName + ".clip";
                        case 0x564D4552: return m_FileName + ".mov";
                        case 0x72786665: return m_FileName + ".efx";
                        case 0x54435846: return m_FileName + ".fxct";
                        case 0x58455452: return m_FileName + ".rtex";
                        case 0x4F464246: return m_FileName + ".oft";
                        case 0x4C4F434D: return m_FileName + ".mcol";
                        case 0x46454443: return m_FileName + ".cdef";
                        case 0x504F5350: return m_FileName + ".psop";
                        case 0x454D414D: return m_FileName + ".mame";
                        case 0x43414D4D: return m_FileName + ".mameac";
                        case 0x544C5346: return m_FileName + ".fslt";
                        case 0x64637273: return m_FileName + ".srcd";
                        case 0x68637273: return m_FileName + ".asrc";
                        case 0x4034B50: return m_FileName + ".zip";
                    }
                }

                if (lpBuffer.Length >= 8)
                {
                    UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, 4);
                    switch (dwMagic)
                    {
                        case 0x47534D47: return m_FileName + ".msg";
                        case 0x52495547: return m_FileName + ".gui";
                        case 0x47464347: return m_FileName + ".gcfg";
                        case 0x72617675: return m_FileName + ".uvar";
                        case 0x544E4649: return m_FileName + ".ifnt";
                        case 0x20746F6D: return m_FileName + ".mot";
                        case 0x70797466: return m_FileName + ".mpg";
                        case 0x4E414554: return m_FileName + ".tean";
                    }
                }

                return m_FileName;
            }
            else
            {
                return m_FileName;
            }
        }
    }
}

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
                        case 0x424956: return m_FileName + ".vib";
                        case 0x444957: return m_FileName + ".wid";
                        case 0x444F4C: return m_FileName + ".lod";
                        case 0x444252: return m_FileName + ".rbd";
                        case 0x4C4452: return m_FileName + ".rdl";
                        case 0x424650: return m_FileName + ".pfb";
                        case 0x464453: return m_FileName + ".mmtr";
                        case 0x46444D: return m_FileName + ".mdf2";
                        case 0x4C4F46: return m_FileName + ".fol";
                        case 0x4E4353: return m_FileName + ".scn";
                        case 0x4F4C43: return m_FileName + ".clo";
                        case 0x504D4C: return m_FileName + ".lmp";
                        case 0x535353: return m_FileName + ".sss";
                        case 0x534549: return m_FileName + ".ies";
                        case 0x530040: return m_FileName + ".wel";
                        case 0x584554: return m_FileName + ".tex";
                        case 0x525355: return m_FileName + ".user";
                        case 0x5A5352: return m_FileName + ".wcc";
                        case 0x4034B50: return m_FileName + ".zip";
                        case 0x54464453: return m_FileName + ".sdft";
                        case 0x44424453: return m_FileName + ".sdbd";
                        case 0x52554653: return m_FileName + ".sfur";
                        case 0x464E4946: return m_FileName + ".finf";
                        case 0x4D455241: return m_FileName + ".arem";
                        case 0x21545353: return m_FileName + ".sst";
                        case 0x204D4252: return m_FileName + ".rbm";
                        case 0x4D534648: return m_FileName + ".hfsm";
                        case 0x59444F42: return m_FileName + ".rdd";
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
                        case 0x54504E47: return m_FileName + ".gnpt";
                        case 0x54414D43: return m_FileName + ".cmat";
                        case 0x44545254: return m_FileName + ".trtd";
                        case 0x50494C43: return m_FileName + ".clip";
                        case 0x564D4552: return m_FileName + ".mov";
                        case 0x414D4941: return m_FileName + ".aimapattr";
                        case 0x504D4941: return m_FileName + ".aimp";
                        case 0x72786665: return m_FileName + ".efx";
                        case 0x736C6375: return m_FileName + ".ucls";
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
                        case 0x4F525541: return m_FileName + ".auto";
                        case 0x7261666C: return m_FileName + ".lfar";
                        case 0x52524554: return m_FileName + ".terr";
                        case 0x736E636A: return m_FileName + ".jcns";
                        case 0x6C626C74: return m_FileName + ".tmlbld";
                        case 0x54455343: return m_FileName + ".cset";
                        case 0x726D6565: return m_FileName + ".eemr";
                        case 0x434C4244: return m_FileName + ".dblc";
                        case 0x384D5453: return m_FileName + ".stmesh";
                        case 0x32736674: return m_FileName + ".tmlfsm2";
                        case 0x45555141: return m_FileName + ".aque";
                        case 0x46554247: return m_FileName + ".gbuf";
                        case 0x4F4C4347: return m_FileName + ".gclo";
                        case 0x44525453: return m_FileName + ".srtd";
                        case 0x544C4946: return m_FileName + ".filt";
                    }
                }

                if (lpBuffer.Length >= 8)
                {
                    UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, 4);
                    switch (dwMagic)
                    {
                        case 0x766544: return m_FileName + ".dev";
                        case 0x6E616863: return m_FileName + ".chain";
                        case 0x6E6C6B73: return m_FileName + ".fbxskel";
                        case 0x47534D47: return m_FileName + ".msg";
                        case 0x52495547: return m_FileName + ".gui";
                        case 0x47464347: return m_FileName + ".gcfg";
                        case 0x72617675: return m_FileName + ".uvar";
                        case 0x544E4649: return m_FileName + ".ifnt";
                        case 0x20746F6D: return m_FileName + ".mot";
                        case 0x70797466: return m_FileName + ".mov";
                        case 0x6D61636D: return m_FileName + ".mcam";
                        case 0x6572746D: return m_FileName + ".mtre";
                        case 0x6D73666D: return m_FileName + ".mfsm";
                        case 0x74736C6D: return m_FileName + ".motlist";
                        case 0x6B6E626D: return m_FileName + ".motbank";
                        case 0x3273666D: return m_FileName + ".motfsm2";
                        case 0x74736C63: return m_FileName + ".mcamlist";
                        case 0x70616D6A: return m_FileName + ".jmap";
                        case 0x736E636A: return m_FileName + ".jcns";
                        case 0x4E414554: return m_FileName + ".tean";
                        case 0x61646B69: return m_FileName + ".ikda";
                        case 0x736C6B69: return m_FileName + ".ikls";
                        case 0x72746B69: return m_FileName + ".iktr";
                        case 0x326C6B69: return m_FileName + ".ikl2";
                        case 0x72686366: return m_FileName + ".fchr";
                        case 0x544C5346: return m_FileName + ".fslt";
                        case 0x6B6E6263: return m_FileName + ".cbnk";
                        case 0x30474154: return m_FileName + ".havokcl";
                        case 0x52504347: return m_FileName + ".gcpr";
                        case 0x74646366: return m_FileName + ".fcmndatals";
                        case 0x67646C6A: return m_FileName + ".jointlodgroup";
                        case 0x444E5347: return m_FileName + ".gsnd";
                        case 0x59545347: return m_FileName + ".gsty";
                        case 0x3267656C: return m_FileName + ".leg2";
                    }
                }

                return m_FileName;
            }
            else
            {
                return m_FileName;
            }
        }

        public static String iGetStringFromBytes(Byte[] m_Bytes)
        {
            Char[] lpBytesHex = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            Char[] lpHexString = new Char[m_Bytes.Length * 2];
            Int32 dwIndex = 0;

            foreach (Byte bByte in m_Bytes)
            {
                lpHexString[dwIndex++] = lpBytesHex[bByte >> 4];
                lpHexString[dwIndex++] = lpBytesHex[bByte & 0x0F];
            }

            return new String(lpHexString);
        }
    }
}

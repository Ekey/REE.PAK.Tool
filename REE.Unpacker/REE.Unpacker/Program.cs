using System;
using System.IO;

namespace REE.Unpacker
{
    class Program
    {
        private static String m_Title = "RE Engine PAK Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2023 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    REE.Unpacker <m_ProjectFile> <m_File> <m_Directory>\n");
                Console.WriteLine("    m_ProjectFile - Project file (Tag) with filenames (file must be in Projects folder)");
                Console.WriteLine("    m_File - Source of PAK archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    REE.Unpacker MHR_PC_DEMO E:\\Games\\MHR\\re_chunk_000.pak D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_ListFile = args[0];
            String m_PakFile = args[1];
            String m_Output = Utils.iCheckArgumentsPath(args[2]);

            if (!File.Exists("Zstandard.Net.dll") || !File.Exists("libzstd.dll"))
            {
                Utils.iSetError("[ERROR]: Unable to find ZSTD modules");
                return;
            }

            if (!File.Exists(m_PakFile))
            {
                Utils.iSetError("[ERROR]: Input PAK file -> " + m_PakFile + " <- does not exist");
                return;
            }

            PakHashList.iLoadProject(m_ListFile);

            using (FileStream TPakStream = File.OpenRead(m_PakFile))
            {
                if (TPakStream.Length <= 16)
                {
                    Utils.iSetError("[ERROR]: Empty PAK archive file");
                    return;
                }

                var m_Header = new PakHeader();

                m_Header.dwMagic = TPakStream.ReadUInt32();
                m_Header.bMajorVersion = TPakStream.ReadByte();
                m_Header.bMinorVersion = TPakStream.ReadByte();
                m_Header.wFeature = TPakStream.ReadInt16();
                m_Header.dwTotalFiles = TPakStream.ReadInt32();
                m_Header.dwHash = TPakStream.ReadUInt32();

                if (m_Header.dwMagic != 0x414B504B)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of PAK archive file");
                    return;
                }

                if (m_Header.bMajorVersion != 2 && m_Header.bMajorVersion != 4 || m_Header.bMinorVersion != 0)
                {
                    Utils.iSetError("[ERROR]: Invalid version of PAK archive file -> " + m_Header.bMajorVersion.ToString() + "." + m_Header.bMinorVersion.ToString() + ", expected 2.0 & 4.0");
                    return;
                }

                switch (m_Header.bMajorVersion)
                {
                    case 2: PakUnpack20.iDoIt(TPakStream, m_Header, m_Output); break;
                    case 4: PakUnpack40.iDoIt(TPakStream, m_Header, m_Output); break;
                    default: break;
                }

                TPakStream.Dispose();
            }
        }
    }
}

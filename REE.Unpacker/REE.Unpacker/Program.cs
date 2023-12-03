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

            PakList.iLoadProject(m_ListFile);
            PakUnpack.iDoIt(m_PakFile, m_Output);
        }
    }
}

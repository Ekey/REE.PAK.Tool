using System;
using System.IO;

namespace REE.Packer
{
    class Program
    {
        public static String m_Title = "RE Engine PAK Packer";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2023 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    REE.Packer <m_File> <m_Directory>\n");
                Console.WriteLine("    m_Directory - Source directory");
                Console.WriteLine("    m_File - Destination of PAK archive file\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    REE.Packer D:\\Unpacked\\re_chunk_000 D:\\re_chunk_000.pak");
                Console.WriteLine("    REE.Packer D:\\MyMod\\ABCDE D:\\re_chunk_000.pak.patch_001.pak");
                Console.WriteLine("    REE.Packer D:\\MyMod\\ABCDE D:\\re_dlc_stm_0000000.pak");
                Console.WriteLine("    REE.Packer D:\\MyMod\\ABCDE D:\\re_dlc_000.pak\n");
                Console.ResetColor();
                return;
            }

            String m_InputDirectory = Utils.iCheckArgumentsPath(args[0]);
            String m_PakFile = args[1];

            if (!File.Exists("Zstandard.Net.dll") || !File.Exists("libzstd.dll"))
            {
                Utils.iSetError("[ERROR]: Unable to find ZSTD modules");
                return;
            }

            PakPack.iDoIt(m_PakFile, m_InputDirectory, Compression.INFLATE);
        }
    }
}

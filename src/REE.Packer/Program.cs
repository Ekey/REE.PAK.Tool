using System;
using System.Reflection;

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
            Console.WriteLine("(c) 2023 Ekey (h4x0r) / v{0}\n", iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    REE.Packer <m_Directory> <m_File>\n");
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

            String m_InputDirectory = iCheckArgumentsPath(args[0]);
            String m_PakFile = args[1];

            var pakFileBuilder = new PakFileBuilder();
            pakFileBuilder.AddDirectory(m_InputDirectory);
            pakFileBuilder.Save(m_PakFile, CompressionType.INFLATE);
        }

        private static string iGetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private static string iCheckArgumentsPath(string m_Arg)
        {
            if (m_Arg.EndsWith("\\") == false)
            {
                m_Arg = m_Arg + @"\";
            }
            return m_Arg;
        }

        private static void iSetError(string m_String)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }
    }
}

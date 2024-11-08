using System;
using System.IO;
using System.Reflection;

namespace REE.Unpacker
{
    class Program
    {
        public static String m_Title = "RE Engine PAK Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2023 Ekey (h4x0r) / v{0}\n", iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2 && args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    REE.Unpacker <m_ProjectFile> <m_File> <m_Directory>\n");
                Console.WriteLine("    m_ProjectFile - Project file (Tag) with filenames (file must be in Projects folder)");
                Console.WriteLine("    m_File - Source of PAK archive file");
                Console.WriteLine("    m_Directory - Destination directory (Optional)\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    REE.Unpacker MHR_PC_DEMO E:\\Games\\MHR\\re_chunk_000.pak");
                Console.WriteLine("    REE.Unpacker MHR_PC_DEMO E:\\Games\\MHR\\re_chunk_000.pak D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_ListFile = args[0];
            String m_PakFile = args[1];
            String m_Output = null;

            if (args.Length == 2)
            {
                m_Output = Path.GetDirectoryName(args[1]) + @"\" + Path.GetFileNameWithoutExtension(args[1]) + @"\";
            }
            else
            {
                m_Output = iCheckArgumentsPath(args[2]);
            }

            if (!File.Exists(m_PakFile))
            {
                iSetError("[ERROR]: Input PAK file -> " + m_PakFile + " <- does not exist");
                return;
            }

            var pakList = PakList.FromFile(m_ListFile);
            var pakFile = new PakFile(m_PakFile);
            pakFile.ExtractAll(pakList, m_Output);
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

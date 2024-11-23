using System;

namespace REE.Rom.Cryptor
{
    class Program
    {
        public static String m_Title = "RE Engine MAMEAC (ROM) Cryptor";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    REE.Rom.Cryptor <m_SrcFile> <m_DstFile>\n");
                Console.WriteLine("    m_SrcFile - Source file");
                Console.WriteLine("    m_DstFile - Destination file\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    REE.Rom.Cryptor D:\\roms\\1943u.mameac.2 D:\\roms\\1943u_decrypted.mameac.2");
                Console.ResetColor();
                return;
            }

            String m_SrcFile = args[0];
            String m_DstFile = args[1];

            MameCrypt.iDoIt(m_SrcFile, m_DstFile);
        }
    }
}

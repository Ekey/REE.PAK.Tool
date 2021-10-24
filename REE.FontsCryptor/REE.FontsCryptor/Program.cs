using System;
using System.IO;

namespace REE.FontsCryptor
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("RE Engine Fonts Cryptor");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    REE.FontsCryptor <m_SrcFile> <m_DstFile>\n");
                Console.WriteLine("    m_SrcFile - Source file");
                Console.WriteLine("    m_DstFile - Destination file\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    REE.FontsCryptor fot-skipstd-b.oft.1 fot-skipstd-b.oft");
                Console.ResetColor();
                return;
            }

            String m_SrcFile = args[0];
            String m_DstFile = args[1];

            if (!File.Exists(m_SrcFile))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_SrcFile + " <- does not exist");
                return;
            }

            FileStream TFontsReader = new FileStream(m_SrcFile, FileMode.Open);

            UInt32 dwMagic = TFontsReader.ReadUInt32();

            switch(dwMagic)
            {
                case 0x4F464246: FontDecrypt.iDoIt(TFontsReader, m_DstFile); break;
                case 0x4F54544F: FontEncrypt.iDoIt(TFontsReader, m_DstFile); break;
            }
        }
    }
}

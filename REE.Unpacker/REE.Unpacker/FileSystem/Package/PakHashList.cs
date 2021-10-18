using System;
using System.IO;
using System.Collections.Generic;

namespace REE.Unpacker
{
    class PakHashList
    {
        static String m_Path = Utils.iGetApplicationPath() + @"\Projects\";

        static Dictionary<UInt32, String> m_HashList = new Dictionary<UInt32, String>();

        public static void iLoadProject(String m_ProjectFile)
        {
            String m_Line = null;
            if (!File.Exists(m_Path + m_ProjectFile))
            {
                Utils.iSetWarning("[WARNING]: Unable to load project file " + m_ProjectFile);
            }

            Int32 i = 0;
            m_HashList.Clear();

            StreamReader TProjectFile = new StreamReader(m_Path + m_ProjectFile);
            while ((m_Line = TProjectFile.ReadLine()) != null)
            {
                UInt32 dwHash = PakHash.iGetHash(m_Line);

                if (m_HashList.ContainsKey(dwHash))
                {
                    String m_Collision = null;
                    m_HashList.TryGetValue(dwHash, out m_Collision);
                    Utils.iSetError("[COLLISION]: " + m_Collision + " <-> " + m_Line);
                }

                m_HashList.Add(dwHash, m_Line);
                i++;
            }

            TProjectFile.Close();
            Utils.iSetInfo("[INFO]: Project File Loaded: " + i.ToString());
            Console.WriteLine();
        }

        public static String iGetNameFromHashList(UInt32 dwHash)
        {
            String m_FileName = null;

            if (m_HashList.ContainsKey(dwHash))
            {
                m_HashList.TryGetValue(dwHash, out m_FileName);
            }
            else
            {
                m_FileName = @"__Unknown\" + dwHash.ToString("X8");
            }

            return m_FileName;
        }
    }
}

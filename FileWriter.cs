using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModManager
{
    internal class FileWriter
    {

        public static Dictionary<string, string> bufferedFileWrite = new Dictionary<string, string>();
        public static Dictionary<string, byte[]> bufferedFileWriteByte = new Dictionary<string, byte[]>();
        public static List<string> toDelete = new List<string>();

        public static void writeFile(string path, string content)
        {
            bufferedFileWrite[path] = content;
        }

        public static void writeFile(string path, byte[] content)
        {
            bufferedFileWriteByte[path] = content;
        }

        public static void deleteFile(string path)
        {
            toDelete.Add(path);
        }

        public static void WriteAll()
        {
            if (bufferedFileWrite.Count > 0 || bufferedFileWriteByte.Count > 0 || toDelete.Count > 0)
            {
                foreach (string path in bufferedFileWrite.Keys)
                {
                    File.WriteAllText(path, bufferedFileWrite[path]);
                }

                foreach (string path in bufferedFileWriteByte.Keys)
                {
                    File.WriteAllBytes(path, bufferedFileWriteByte[path]);
                }

                foreach (string path in toDelete)
                {
                    File.Delete(path);
                }

                bufferedFileWrite.Clear();
                bufferedFileWriteByte.Clear();
                toDelete.Clear();
                ModManagerTools.getModLocalInformations();
            }


        }

    }
}

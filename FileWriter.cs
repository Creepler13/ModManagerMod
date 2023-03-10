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

        public static void writeFile(string path,string content)
        {
           bufferedFileWrite[path] = content;
        }

        public static void WriteAll()
        {
            if (bufferedFileWrite.Count > 0)
            {
                foreach (string path in bufferedFileWrite.Keys)
                {

                    File.WriteAllText(path, bufferedFileWrite[path]);
                }
                ModManagerTools.getModLocalInformations();
            }
        }

    }
}

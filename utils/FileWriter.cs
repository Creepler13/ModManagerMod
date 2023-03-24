using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System;
using System.Reflection;

namespace ModManager.utils
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
                try
                {

                    foreach (string path in bufferedFileWrite.Keys)
                    {
                        moveToDelete(path);
                        File.WriteAllText(path, bufferedFileWrite[path]);
                    }

                    foreach (string path in bufferedFileWriteByte.Keys)
                    {
                        moveToDelete(path);
                        File.WriteAllBytes(path, bufferedFileWriteByte[path]);
                    }

                    foreach (string path in toDelete)
                    {
                        moveToDelete(path);
                    }
                }
                catch (Exception e)
                {
                    MelonLoader.MelonLogger.Error(e);
                }
                bufferedFileWrite.Clear();
                bufferedFileWriteByte.Clear();
                toDelete.Clear();
                ModManagerTools.getModLocalInformations();
            }


        }

        public static void deleteFiles()
        {
            Directory.CreateDirectory("Userdata/ModManager/toDelete");
            foreach (string file in Directory.EnumerateFiles("Userdata/ModManager/toDelete"))
            {
                File.Delete(file);
            }
        }

        private static void moveToDelete(string path)
        {
            if (File.Exists(path))
                File.Move(path, "Userdata/ModManager/toDelete/" + path.Split("/".ToCharArray()).Last());
        }



        public static void moveToEnabled(string fileName)
        {
            if (File.Exists(ModManager.disabledModsPath + fileName))
                File.Move(ModManager.disabledModsPath + fileName, ModManager.modsPath + fileName);
        }
        public static void moveToDisabled(string fileName)
        {
            if (File.Exists(ModManager.modsPath + fileName))
                File.Move(ModManager.modsPath + fileName, ModManager.disabledModsPath + fileName);
        }


        public static Stream ReadResourceStream(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"

            resourcePath = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(name));


            Stream s = assembly.GetManifestResourceStream(resourcePath);


            return s;

        }
    }
}

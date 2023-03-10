using System;

using System.Net.Http;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine.Networking;
using System.Threading.Tasks;
using MelonLoader;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Il2CppNewtonsoft.Json.Linq;
using Assets.Scripts.PeroTools.Commons;
using Il2CppNewtonsoft.Json;
using System.Collections;

namespace ModManager
{
    public class ModManagerTools
    {

        public static List<ModInfo> mods = new List<ModInfo>();

        public static string SelectdStyle = "defaultStyle";

        public static void getModLocalInformations()
        {
            mods.Clear();

            string musedashFolder = Directory.GetCurrentDirectory();

            foreach (String file in Directory.EnumerateFiles("mods"))
            {
                if (!file.EndsWith(".dll")) continue;
                Assembly ass = Assembly.LoadFrom(file);
                MelonInfoAttribute info = ass.GetCustomAttribute<MelonInfoAttribute>();

                ModInfo mod = new ModInfo(info, Path.GetFileName(file));
                mod.enabled = true;
                mod.local = true;
                mods.Add(mod);
            }

            if (Directory.Exists("disabledMods"))
                foreach (String file in Directory.EnumerateFiles("disabledMods"))
                {
                    MelonLogger.Msg(file);
                    Assembly ass = Assembly.LoadFrom(musedashFolder + "/" + file);
                    MelonInfoAttribute info = ass.GetCustomAttribute<MelonInfoAttribute>();
                    ModInfo mod = new ModInfo(info, Path.GetFileName(file));
                    mod.enabled = false;
                    mod.local = true;
                    mods.Add(mod);
                }
            else
                Directory.CreateDirectory("disabledMods");

            ModsPnlManager.RefreshBoxes();
            getOnlineModInformation();

        }


        public static void getOnlineModInformation()
        {
            String rawJson = Networking.Get("https://raw.githubusercontent.com/MDModsDev/ModLinks/main/ModLinks.json");
            JArray modList = JsonUtils.Deserialize<JArray>(rawJson, (JsonSerializerSettings)null);


            for(int i = 0;i<modList.Count;i++)
            
            {
                JObject modEntry = modList[i].Cast<JObject>();
                string modName = (string)modEntry["Name"];


                MelonLoader.MelonLogger.Msg("Procession mod:" + modName);
               
                int j = 0;
                for (; j < mods.Count; j++)
                {
                    ModInfo lMod = mods[j];
                    if (lMod.Name == modName)
                    {

                        addOnlineModToLocalMod(modEntry, mods[j]);
                        break;
                    }
                }

                if (j == mods.Count)
                {
                    ModInfo modInfo = new ModInfo();
                    modInfo.Name = modName;
                    addOnlineMod(modEntry, modInfo);
                }
            }


            ModsPnlManager.RefreshBoxes();

        }

        private static void addOnlineModToLocalMod(JObject mod, ModInfo modInfo)
        {
            modInfo.online = true;
            modInfo.onlineVersion = (string)mod["Version"];
            modInfo.description =  (string)mod["Description"];
            modInfo.hasUpdate = isVersionGreater(modInfo.onlineVersion, modInfo.Version);
            modInfo.downloadLink = "https://raw.githubusercontent.com/MDModsDev/ModLinks/main/" + (string)mod["DownloadLink"];
        }
        private static void addOnlineMod(JObject mod, ModInfo modInfo)
        {
            modInfo.hasUpdate = true;
            modInfo.online = true;
            modInfo.Author = (string)mod["Author"];
            modInfo.description = (string)mod["Description"];
            modInfo.onlineVersion = (string)mod["Version"];
            modInfo.downloadLink = "https://raw.githubusercontent.com/MDModsDev/ModLinks/main/" + (string)mod["DownloadLink"];

            mods.Add(modInfo);
        }

        public static T getComponentByName<T>(GameObject gm, string name)
        {
            foreach (T comp in gm.GetComponentsInChildren<T>())
            {

                if (((string)comp.GetType().GetField("name").GetValue(comp)) == name)
                    return comp;
            }
            return default(T);
        }

        public static Component getComponentByName(GameObject gm, string name, Type type)
        {
            foreach (Component comp in gm.GetComponentsInChildren<Component>())
            {
                if (((Component)comp).name == name)
                    if (comp.GetType() == type)
                        return comp;

            }



            return null;
        }






        public static Component getComponentByName(GameObject gm, string name)
        {
            foreach (Component comp in gm.GetComponentsInChildren<Component>())
            {
                if (((Component)comp).name == name)
                    return comp;
            }
            return null;
        }

        public static byte[] ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"

            resourcePath = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(name));


            Stream s = assembly.GetManifestResourceStream(resourcePath);




            byte[] bytes = new byte[s.Length + 10];
            int numBytesToRead = (int)s.Length;
            int numBytesRead = 0;
            do
            {
                // Read may return anything from 0 to 10.
                int n = s.Read(bytes, numBytesRead, 10);
                numBytesRead += n;
                numBytesToRead -= n;
            } while (numBytesToRead > 0);
            s.Close();







            // string s2 = System.Text.UTF32Encoding.Default.GetString(bytes);

            //            MelonLoader.MelonLogger.Msg(s2);

            //            MelonLoader.MelonLogger.Msg(bytes[bytes.Length - 1]);

            //MelonLoader.MelonLogger.Msg(bytes[bytes.Length - 2]);


            return bytes;

        }


        public static bool isVersionGreater(string v1, string v2)
        {

            string[] v1Split = v1.Split(".".ToCharArray());
            string[] v2Split = v2.Split(".".ToCharArray());
            int length = v1Split.Length < v2Split.Length ? v1Split.Length : v2Split.Length;

            for (int i = 0; i < length; i++)
            {
                int v1i = int.Parse(v1Split[i]);
                int v2i = int.Parse(v2Split[i]);


                if (v1i > v2i)
                    return true;
                if (v1i < v2i)
                    return false;

            }

            return v1Split.Length > v2Split.Length;

        }



    }

    public class ModSorter : Comparer<ModInfo>
    {
        public override int Compare(ModInfo x, ModInfo y)
        {
            if (!x.local && x.online && !(!y.local && y.online)) return 1;
            return String.Compare(y.Name, x.Name);
        }
    }

/*
        public static void getOnlineModInformationMDMC()
        {
            //download 
            System.Action<UnityWebRequest> callbacl = delegate (UnityWebRequest webRequest)
            {
                JArray val = JsonUtils.Deserialize<JArray>(webRequest.downloadHandler.GetText(), (JsonSerializerSettings)null);


                for (int i = 0; i < val.Count; i++)
                {
                    JObject Omod = val[i].Cast<JObject>();

                    ModInfo mod = new ModInfo();
                    bool islocal = false;
                    int index = 0;
                    for (int j = 0; j < mods.Count; j++)
                    {
                        ModInfo lMod = mods[j];
                        if (lMod.Name == (string)Omod["name"])
                        {
                            index = j;
                            islocal = true;
                            mod = lMod;
                            break;
                        }
                    }


                    mod.Name = (string)Omod["name"];
                    mod.Author = (string)Omod["author"];

                    mod.onlineVersion = (string)Omod["version"];
                    mod.online = true;
                    mod.downloadLink = "https://mdmc.moe/api/v5/download/mod/" + (int)Omod["id"];

                    if (islocal)
                    {
                        mod.hasUpdate = isVersionGreater(mod.onlineVersion, mod.Version);
                        mods[index] = mod;
                    }
                    else
                    {
                        mod.hasUpdate = true;
                        mods.Add(mod);
                    }
                }

                MelonLogger.Msg("GotOnlineMods");

                ModsPnlManager.RefreshBoxes();
            };
            Networking.Get("https://mdmc.moe/api/v5/mods", callbacl);

        }
*/


}

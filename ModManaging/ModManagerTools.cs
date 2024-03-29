﻿using System;

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
using ModManager.utils;

namespace ModManager
{
    public class ModManagerTools
    {

        public static ModsPnlScript modsPnlScript;
        public static List<ModInfo> mods = new List<ModInfo>();
        public static Dictionary<string, Assembly> loaded = new Dictionary<string, Assembly>();
        public static string SelectdStyle = "defaultStyle";

        public static void getModLocalInformations()
        {
            mods.Clear();

            string musedashFolder = Directory.GetCurrentDirectory();

            foreach (String file in Directory.EnumerateFiles(ModManager.modsPath))
            {
                try
                {
                    if (!file.EndsWith(".dll")) continue;
                    Assembly ass = Assembly.LoadFile(file);
                    MelonInfoAttribute info = ass.GetCustomAttribute<MelonInfoAttribute>();

                    ModInfo mod = new ModInfo(info, Path.GetFileName(file));
                    mod.enabled = true;
                    mod.local = true;
                    mods.Add(mod);
                }
                catch
                {
                    MelonLoader.MelonLogger.Msg("Unable to load " + file);

                }
            }

            if (Directory.Exists(ModManager.disabledModsPath))
                foreach (String file in Directory.EnumerateFiles(ModManager.disabledModsPath))
                {
                    try
                    {
                        if (!file.EndsWith(".dll")) continue;

                        Assembly ass = Assembly.LoadFrom(file);
                        loaded[ass.FullName] = ass;
                        MelonInfoAttribute info = ass.GetCustomAttribute<MelonInfoAttribute>();
                        //MelonLogger.Msg(info.Name + " " + info.Version + " size:" + new FileInfo(file).Length + " Disabled");
                        ModInfo mod = new ModInfo(info, Path.GetFileName(file));
                        mod.enabled = false;
                        mod.local = true;
                        mods.Add(mod);
                    }
                    catch
                    {
                        MelonLoader.MelonLogger.Msg("Unable to load " + file);
                    }
                }
            else
                Directory.CreateDirectory(ModManager.disabledModsPath);

            modsPnlScript.mods = mods;
            modsPnlScript.refresh();

            getOnlineModInformation();

        }


        public static void getOnlineModInformation()
        {
            String rawJson = Networking.Get("https://raw.githubusercontent.com/MDModsDev/ModLinks/main/ModLinks.json");
            JArray modList = JsonUtils.Deserialize<JArray>(rawJson, (JsonSerializerSettings)null);


            for (int i = 0; i < modList.Count; i++)

            {
                JObject modEntry = modList[i].Cast<JObject>();
                string modName = (string)modEntry["Name"];


                // MelonLoader.MelonLogger.Msg("Procession mod:" + modName);

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

            modsPnlScript.mods = mods;
            modsPnlScript.refresh();

        }

        public static List<string> temp_ModUpdatedthisInstance = new List<string>();

        private static void addOnlineModToLocalMod(JObject mod, ModInfo modInfo)
        {
            modInfo.online = true;
            modInfo.onlineVersion = (string)mod["Version"];
            modInfo.description = (string)mod["Description"];
            modInfo.hasUpdate = temp_ModUpdatedthisInstance.Contains(modInfo.Name) ? false : isVersionGreater(modInfo.onlineVersion, modInfo.Version); //TODO fix update
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



        public static byte[] ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"

            resourcePath = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(name));


            Stream s = assembly.GetManifestResourceStream(resourcePath);

            byte[] bytes = new byte[s.Length];
            s.Read(bytes, 0, bytes.Length);

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

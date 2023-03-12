
using UnityEngine;
using MelonLoader;
using System.IO;
using static UnhollowerRuntimeLib.ClassInjector;

namespace ModManager
{
    public class ModManager : MelonMod
    {
        public static GameObject ModPnlAsset;
        public static GameObject ModBoxAsset;

        public static string modsPath = "mods/", disabledModsPath = "UserData/ModManager/disabledMods/";


        public override void OnInitializeMelon()
        {
            RegisterTypeInIl2Cpp<ModsPnlScript>();
            RegisterTypeInIl2Cpp<ModBoxScript>();

            Settings.load();
            FileWriter.deleteFiles();
        }

        public override void OnFixedUpdate()
        {
            FileWriter.WriteAll();
        }

        public const string Name = "ModManager";
        public const string Description = "ModManager";
        public const string Version = "1.0.0";
        public const string Author = "Creepler13";



    }
}

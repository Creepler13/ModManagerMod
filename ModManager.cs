
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

     
        public void installPlugin()
        {

            File.Move("plugins/ModManagerAutUpdatePlugin.dll", "Userdata/ModManager/toDelete/ModManagerAutUpdatePlugin.dll");
            File.WriteAllBytes("plugins/ModManagerAutUpdatePlugin.dll", ModManagerTools.ReadResource("ModManager.ModManagerAutUpdatePlugin.dll"));

        }

        public override void OnDeinitializeMelon()
        {
            base.OnDeinitializeMelon();
        }


        public const string Name = "ModManager";
        public const string Description = "ModManager";
        public const string Version = "1.0.0";
        public const string Author = "Creepler13";



    }
}

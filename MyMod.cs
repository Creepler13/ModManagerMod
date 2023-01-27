
using UnityEngine;
using MelonLoader;
using System.IO;
using static UnhollowerRuntimeLib.ClassInjector;

namespace ModManager
{
    public class MyMod : MelonMod
    {

        public override void OnInitializeMelon()
        {
            MelonLoader.MelonLogger.Msg(typeof(ModsPnlScript));
            RegisterTypeInIl2Cpp<ModsPnlScript>();
            RegisterTypeInIl2Cpp<ModBoxScript>();
        }


        public override void OnApplicationStart()
        {
            Settings.load();
            //installPlugin();

            if (!Directory.Exists("Userdata/ModManager/styles"))
                Directory.CreateDirectory("Userdata/ModManager/styles");

            File.WriteAllBytes("Userdata/ModManager/styles/defaultStyle", ModManagerTools.ReadResource("ModManager.Prefabs.defaultStyle"));


            //    ModManagerTools.getModLocalInformations();



        }

        public void installPlugin()
        {
            File.WriteAllBytes("plugins/ModManagerAutUpdatePlugin.dll", ModManagerTools.ReadResource("ModManager.ModManagerAutUpdatePlugin.dll"));

        }




        public const string Name = "ModManager";
        public const string Description = "ModManager";
        public const string Version = "1.0.0";
        public const string Author = "Creepler13";



    }
}

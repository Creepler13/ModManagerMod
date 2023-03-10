
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
        public override void OnInitializeMelon()
        {
            RegisterTypeInIl2Cpp<ModsPnlScript>();
            RegisterTypeInIl2Cpp<ModBoxScript>();

            Settings.load();

        }

        public override void OnApplicationLateStart()
        {
         
        }

        public override void OnFixedUpdate()
        {
            FileWriter.WriteAll();


        }

        public override void OnApplicationStart()
        {

        

            //installPlugin();

            //  if (!Directory.Exists("Userdata/ModManager/styles"))
            //     Directory.CreateDirectory("Userdata/ModManager/styles");

            //   File.WriteAllBytes("Userdata/ModManager/styles/defaultStyle", );


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

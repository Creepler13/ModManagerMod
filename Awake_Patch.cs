using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Assets.Scripts.UI.Panels;
using UnityEngine.UI;
using UnityEngine;
using ModManager;
using UnhollowerBaseLib;
using System.Reflection;
using System.IO;


namespace ModManager
{

    [HarmonyPatch(typeof(MenuSelect), "OnAwake")]
    internal class Awake_Patch
    {
        private static void Prefix(MenuSelect __instance)
        {
            //MelonLoader.MelonLogger.Msg(__instance.m_BtnElfin);

            //Reposition MenuSelect 
            Vector3 parPos = __instance.m_PcOptions[0].transform.parent.position;
            parPos.x = parPos.x - 2.5f;
            __instance.m_PcOptions[0].transform.parent.position = parPos;

            //Add ModToggle to MeneSelect
            Transform lastChild = __instance.m_PcOptions[0].transform.parent.GetChild(__instance.m_PcOptions[0].transform.parent.childCount - 1);
            Toggle ModsToggle = Object.Instantiate<Toggle>(__instance.m_PcOptions[__instance.m_PcOptions.Length - 1], __instance.m_PcOptions[0].transform.parent.transform);
            lastChild.SetAsLastSibling();// move array with "d" to the end;


            //  ModsToggle.GetComponent<Text>().text = "MODS";//WIP


            // GameObject ModsPnl = Object.Instantiate<GameObject>(__instance.m_ActivePnls[__instance.m_PcOptions.Length - 1], __instance.m_ActivePnls[__instance.m_PcOptions.Length - 1].transform.parent);
            //   Component challengeBox = ModsPnl.transform.FindChild("ChallengesBoxs");

            /*
            System.UriBuilder uri = new System.UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            string path = System.Uri.UnescapeDataString(uri.Path);
            MelonLoader.MelonLogger.Msg(path);
            */

            //var bundleLoadRequest = AssetBundle.LoadFromFileAsync(path+"/Prefabs/Plane.prefab");
            var bundleLoadRequest = AssetBundle.LoadFromFile("Userdata/ModManager/styles/defaultStyle");
            GameObject ModPnlAsset = bundleLoadRequest.LoadAsset("assets/ModsPnl.prefab").Cast<GameObject>();
            GameObject ModBoxAsset = bundleLoadRequest.LoadAsset("assets/ModBox.prefab").Cast<GameObject>();
            //Texture2D MDMCIconSprite = bundleLoadRequest.LoadAsset("assets/MDMCIcon.png").Cast<Texture2D>();
            //ModManagerTools.getComponentByName(ModBoxAsset, "MDMCIcon").GetComponent<SpriteRenderer>().sprite = Sprite.Create(MDMCIconSprite, new Rect(0, 0, MDMCIconSprite.width, MDMCIconSprite.height), Vector2.one);





            GameObject ModsPnl = Object.Instantiate<GameObject>(ModPnlAsset, __instance.m_ActivePnls[__instance.m_PcOptions.Length - 1].transform.parent);
            ModsPnl.SetActive(false);

            ModsPnl.AddComponent<ModsPnlScript>();
            ModsPnlScript modsPnlScript = ModsPnl.GetComponent<ModsPnlScript>();
            modsPnlScript.ModBoxAsset = ModBoxAsset;
            modsPnlScript.ContentPanel = ModManagerTools.getComponentByName(ModsPnl, "Content").gameObject;
            ModsPnlManager.modsPnl = ModsPnl;
            ModsPnlManager.modsPnlScript = modsPnlScript;

            ModManagerTools.getModLocalInformations();



            // Object.Instantiate<GameObject>(Asset, ModsPnl.transform);


            //  MelonLoader.MelonLogger.Msg(Asset.GetComponent<Canvas>().renderMode);



            // foreach (Component comp in __instance.m_ActivePnls[__instance.m_PcOptions.Length - 1].GetComponentsInChildren<Component>())




            GameObject[] temp = __instance.m_ActivePnls.ToArray<GameObject>();
            __instance.m_ActivePnls = temp.AddToArray(ModsPnl);
            Toggle[] temp2 = __instance.m_PcOptions.ToArray<Toggle>();
            __instance.m_PcOptions = temp2.AddToArray(ModsToggle);

            /*
          Toggle[] temp = __instance.m_PcOptions.ToArray();
          for (int i = 1; i < temp.Length; i++)
              MelonLoader.MelonLogger.Msg(__instance.m_PcOptions[i].transform.position.ToString());
          /*
          /*
          Toggle temp = __instance.m_PcOptions[0];
          GameObject temp2 = __instance.m_ActivePnls[0];
          __instance.m_ActivePnls.AddItem<GameObject>(temp2);
          __instance.m_PcOptions.AddItem<Toggle>(temp);
          */
        }



    }
}

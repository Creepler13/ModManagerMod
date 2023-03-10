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
            Vector3 parPos = __instance.m_PcOptions[0].transform.parent.position;
            parPos.x = parPos.x - 2.5f;
            __instance.m_PcOptions[0].transform.parent.position = parPos;

            //Add ModToggle to MeneSelect
            Transform lastChild = __instance.m_PcOptions[0].transform.parent.GetChild(__instance.m_PcOptions[0].transform.parent.childCount - 1);
            Toggle ModsToggle = Object.Instantiate<Toggle>(__instance.m_PcOptions[__instance.m_PcOptions.Length - 1], __instance.m_PcOptions[0].transform.parent.transform);

            foreach(Text text in ModsToggle.GetComponents<Text>()){
                MelonLoader.MelonLogger.Msg(text.text);
            }

            var bundleLoadRequest = AssetBundle.LoadFromMemory(ModManagerTools.ReadResource("ModManager.defaultstyle"));

            GameObject ModPnlAsset = bundleLoadRequest.LoadAsset("assets/ModsPnl.prefab").Cast<GameObject>();
            GameObject ModBoxAsset = bundleLoadRequest.LoadAsset("assets/ModBox.prefab").Cast<GameObject>();



            lastChild.SetAsLastSibling();// move array with "d" to the end;

            MelonLoader.MelonLogger.Msg("Creating ModPnl");
            GameObject ModsPnl = Object.Instantiate<GameObject>(ModPnlAsset, __instance.m_ActivePnls[__instance.m_PcOptions.Length - 1].transform.parent);
            ModsPnl.SetActive(false);

            ModsPnl.AddComponent<ModsPnlScript>();
            ModsPnlScript modsPnlScript = ModsPnl.GetComponent<ModsPnlScript>();
            modsPnlScript.ModBoxAsset = ModBoxAsset;
            modsPnlScript.ContentPanel = ModManagerTools.getComponentByName(ModsPnl, "Content").gameObject;
            ModsPnlManager.modsPnl = ModsPnl;
            ModsPnlManager.modsPnlScript = modsPnlScript;

            ModManagerTools.getModLocalInformations();

            GameObject[] temp = __instance.m_ActivePnls.ToArray<GameObject>();
            __instance.m_ActivePnls = temp.AddToArray(ModsPnl);
            Toggle[] temp2 = __instance.m_PcOptions.ToArray<Toggle>();
            __instance.m_PcOptions = temp2.AddToArray(ModsToggle);

        }



    }
}

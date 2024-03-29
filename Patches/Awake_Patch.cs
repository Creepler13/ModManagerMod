﻿using HarmonyLib;
using ModManager.utils;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;

namespace ModManager
{

    [HarmonyPatch(typeof(MenuSelect), "OnAwake")]
    internal class Awake_Patch
    {
        public static ModsPnlScript Modspnlscript;
        private static void Prefix(MenuSelect __instance)
        {

            GameObject btnHowToPlay = __instance.m_ActivePnls[0].transform.GetChild(0).transform.GetChild(0).gameObject;
            Sprite menuButtonImg = btnHowToPlay.GetComponent<Image>().sprite;

            Vector3 parPos = __instance.m_PcOptions[0].transform.parent.position;
            parPos.x = parPos.x - 2.5f;
            __instance.m_PcOptions[0].transform.parent.position = parPos;

            //Add ModToggle to MeneSelect
            Transform lastChild = __instance.m_PcOptions[0].transform.parent.GetChild(__instance.m_PcOptions[0].transform.parent.childCount - 1);
            Toggle ModsToggle = Object.Instantiate<Toggle>(__instance.m_PcOptions[__instance.m_PcOptions.Length - 1], __instance.m_PcOptions[0].transform.parent.transform);

            Sprite melonSprite = SpriteManager.load("melon.png");

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
            modsPnlScript.melonSprite = melonSprite;
            modsPnlScript.ModsToggle = ModsToggle;
            modsPnlScript.menuButtonImg = menuButtonImg;
            modsPnlScript.ContentPanel = UnityUtils.getComponentByName(ModsPnl, "Content").gameObject;

            ModManagerTools.modsPnlScript = modsPnlScript;

            Modspnlscript = modsPnlScript;

            ModManagerTools.getModLocalInformations();

            GameObject[] temp = __instance.m_ActivePnls.ToArray<GameObject>();
            __instance.m_ActivePnls = temp.AddToArray(ModsPnl);
            Toggle[] temp2 = __instance.m_PcOptions.ToArray<Toggle>();
            __instance.m_PcOptions = temp2.AddToArray(ModsToggle);

        }

        static Exception Finalizer()
        {
            MelonLoader.MelonLogger.Msg("POST fix");
            GameObject iconAchv = Awake_Patch.Modspnlscript.ModsToggle.transform.GetChild(0).FindChild("ImgIconAchv").gameObject;
            iconAchv.GetComponent<Image>().sprite = Awake_Patch.Modspnlscript.melonSprite;
            //  iconAchv.GetComponent<CanvasRenderer>().
            Object.Destroy(Awake_Patch.Modspnlscript.ModsToggle.transform.GetChild(0).FindChild("ImgCheckmark").gameObject);
            return null;
        }


        private static void Postfix()
        {



        }

    }
}

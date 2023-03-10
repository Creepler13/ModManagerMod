
using Assets.Scripts.PeroTools.Commons;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace ModManager
{
    internal class ModsPnlManager
    {

        public static GameObject modsPnl;
        public static ModsPnlScript modsPnlScript;

        public static GameObject ModBoxPrefab;
        public static Component Content;


        public static GameObject addModBox(ModInfo mod)
        {
            GameObject modbox = Object.Instantiate<GameObject>(ModBoxPrefab, Content.transform);

            ModManagerTools.getComponentByName(modbox, "ModName").GetComponent<Text>().text = mod.Name;
            ModManagerTools.getComponentByName(modbox, "ModAuthor").GetComponent<Text>().text = mod.Author;
            ModManagerTools.getComponentByName(modbox, "ModVersion").GetComponent<Text>().text = mod.Version;
            ModManagerTools.getComponentByName(modbox, "StatusText").GetComponent<Text>().text = mod.enabled ? "Enabled" : "Disabled";

            Component UiBtn = ModManagerTools.getComponentByName(modbox, "UninstallButton");
            System.Action UIaction = delegate
            {
                mod.delete();
                ModManagerTools.getModLocalInformations();
            };
            UiBtn.GetComponent<Button>().onClick.AddListener(UIaction);

            if (mod.hasUpdate)
            {
                if (mod.local)
                {
                    Component uptxt = ModManagerTools.getComponentByName(modbox, "UpdateText");
                    uptxt.GetComponent<Text>().text = "-> " + mod.onlineVersion;

                    Component iBtn = ModManagerTools.getComponentByName(modbox, "InstallButton");
                    iBtn.GetComponentInChildren<Text>().text = "Update";
                    System.Action action = delegate
                    {
                        mod.update();
                        ModManagerTools.getModLocalInformations();
                    };
                    iBtn.GetComponent<Button>().onClick.AddListener(action);
                }
            }
            else

            {
                ModManagerTools.getComponentByName(modbox, "UpdateText").gameObject.SetActive(false);
                ModManagerTools.getComponentByName(modbox, "InstallButton").gameObject.SetActive(false);
                ModManagerTools.getComponentByName(modbox, "MDMCIcon").gameObject.SetActive(false);

            }

            return modbox;
        }

        public static GameObject addModBoxEnabled(ModInfo mod)
        {
            GameObject modbox = addModBox(mod);
            ModManagerTools.getComponentByName(modbox, "DisableButton").GetComponentInChildren<Text>().text = "Disable";
            System.Action action = delegate
            {
                mod.disable();
                ModManagerTools.getModLocalInformations();
            };
            ModManagerTools.getComponentByName(modbox, "DisableButton").GetComponent<Button>().onClick.AddListener(action);
            return modbox;
        }

        public static GameObject addModBoxDisabled(ModInfo mod)
        {
            GameObject modbox = addModBox(mod);
            ModManagerTools.getComponentByName(modbox, "DisableButton").GetComponentInChildren<Text>().text = "Enable";
            System.Action action = delegate
            {
                mod.enable();
                ModManagerTools.getModLocalInformations();
            };
            ModManagerTools.getComponentByName(modbox, "DisableButton").GetComponent<Button>().onClick.AddListener(action);
            return modbox;
        }

        public static GameObject addModBoxAvailebleonline(ModInfo mod)
        {
            GameObject modbox = addModBox(mod);


            Component uptxt = ModManagerTools.getComponentByName(modbox, "UpdateText");
            uptxt.GetComponent<Text>().text = "-> " + mod.onlineVersion;

            Component iBtn = ModManagerTools.getComponentByName(modbox, "InstallButton");
            iBtn.GetComponentInChildren<Text>().text = "Install";
            System.Action action = delegate
            {
                mod.download();
                ModManagerTools.getModLocalInformations();
            };
            iBtn.GetComponent<Button>().onClick.AddListener(action);

            ModManagerTools.getComponentByName(modbox, "StatusText").GetComponent<Text>().text = "Available";
            ModManagerTools.getComponentByName(modbox, "ModVersion").gameObject.SetActive(false);
            ModManagerTools.getComponentByName(modbox, "DisableButton").gameObject.SetActive(false);
            ModManagerTools.getComponentByName(modbox, "UninstallButton").gameObject.SetActive(false);



            return modbox;
        }


        
        public static void RefreshBoxes()
        {
            modsPnlScript.mods = ModManagerTools.mods;
            modsPnlScript.refresh();
        }


            public static void RefreshBoxesTemp()
        {

            MelonLogger.Msg("refreshing " + ModManagerTools.mods.Count);

            foreach (Component comp in Content.GetComponentsInChildren<Component>())
                if (comp.name == "ModBox(Clone)")
                    Object.Destroy(comp.gameObject);
            //MelonLogger.Msg(comp.name);



            MelonLogger.Msg(Content.GetComponentsInChildren<Component>().Length + " components left");


            ModManagerTools.mods.Sort(new ModSorter());

            foreach (ModInfo mod in ModManagerTools.mods)
            {
                GameObject modbox;
                if (mod.local)
                {
                    if (mod.enabled)
                        modbox = addModBoxEnabled(mod);
                    else
                        modbox = addModBoxDisabled(mod);
                }
                else
                    modbox = addModBoxAvailebleonline(mod);
            }
        }

    }
}

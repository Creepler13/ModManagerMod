using ModManager;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    internal class ModsPnlScript : MonoBehaviour
    {
        // Start is called before the first frame update

        public List<ModInfo> mods = new List<ModInfo>();
        public GameObject ModBoxAsset;
        public GameObject ContentPanel;
        void Start()
        {
            Button[] buttons = GetComponentsInChildren<Button>();

            buttons[buttons.Length - 3].onClick.AddListener((System.Action)delegate { tab = 0; refreshUI(); });
            buttons[buttons.Length - 2].onClick.AddListener((System.Action)delegate { tab = 1; refreshUI(); });
            buttons[buttons.Length - 1].onClick.AddListener((System.Action)delegate { tab = 2; refreshUI(); });
        }

        public List<GameObject> ModBoxInstalled = new List<GameObject>();
        public List<GameObject> ModBoxDisabled = new List<GameObject>();
        public List<GameObject> ModBoxAvailable = new List<GameObject>();

        public int tab = 0;

        public void refreshUI()
        {

            MelonLoader.MelonLogger.Msg(tab);

            foreach (GameObject modBox in ModBoxInstalled)
            {
                modBox.SetActive(tab == 0);
            }
            foreach (GameObject modBox in ModBoxDisabled)
            {
                modBox.SetActive(tab == 1);
            }

            foreach (GameObject modBox in ModBoxAvailable)
            {
                modBox.SetActive(tab == 2);
            }
        }

        public void refresh()
        {
            foreach (Component comp in ContentPanel.GetComponentsInChildren<Component>())
                if (comp.name == "ModBox(Clone)")
                    Object.Destroy(comp.gameObject);

            ModBoxInstalled.Clear();
            ModBoxDisabled.Clear();
            ModBoxAvailable.Clear();

            foreach (ModInfo mod in mods)
            {
                GameObject modBox = GameObject.Instantiate(ModBoxAsset, ContentPanel.transform);
                modBox.AddComponent<ModBoxScript>();
                ModBoxScript modBoxScript = modBox.GetComponent<ModBoxScript>();
                modBoxScript.modInfo = mod;
                modBoxScript.modsPnl = this;

                if (mod.local)
                {
                    if (mod.enabled)
                    {
                        modBoxScript.tab = 0;
                        ModBoxInstalled.Add(modBox);
                    }
                    else
                    {
                        modBoxScript.tab = 1;
                        ModBoxDisabled.Add(modBox);
                    }

                }
                else if (mod.online)
                {
                    modBoxScript.tab = 2;
                    ModBoxAvailable.Add(modBox);
                }



            }

            refreshUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public ModsPnlScript(System.IntPtr intPtr) : base(intPtr)
        {
        }
    }
}
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
        public Scrollbar verticalScrollbar;
        void Start()
        {
            tabs = new List<GameObject>[] { ModBoxInstalled, ModBoxDisabled, ModBoxAvailable, ModBoxUpdate };

            verticalScrollbar = GetComponentInChildren<ScrollRect>().verticalScrollbar;
            Button[] buttons = GetComponentsInChildren<Button>();
            buttons[buttons.Length - 4].onClick.AddListener((System.Action)delegate { tab = 0; refreshUI(); });
            buttons[buttons.Length - 3].onClick.AddListener((System.Action)delegate { tab = 1; refreshUI(); });
            buttons[buttons.Length - 2].onClick.AddListener((System.Action)delegate { tab = 2; refreshUI(); });
            buttons[buttons.Length - 1].onClick.AddListener((System.Action)delegate { tab = 3; refreshUI(); });
        }

        private List<GameObject> ModBoxInstalled = new List<GameObject>();
        private List<GameObject> ModBoxDisabled = new List<GameObject>();
        private List<GameObject> ModBoxAvailable = new List<GameObject>();
        private List<GameObject> ModBoxUpdate = new List<GameObject>();

        public List<GameObject>[] tabs = new List<GameObject>[0];

        public int tab = 0;

        public void refreshUI()
        {
            if (!gameObject.active) return;

            for(int tabIndex = 0;tabIndex<tabs.Length;tabIndex++)
            {
                foreach (GameObject modBox in tabs[tabIndex])
                {
                   
                    modBox.SetActive(tab == tabIndex);
                  if(modBox.active) modBox.GetComponent<ModBoxScript>().updateUI();
                }
            }

           
            verticalScrollbar.value = 1;
            

        }

        public void refresh()
        {
            foreach (Component comp in ContentPanel.GetComponentsInChildren<Component>())
                if (comp.name == "ModBox(Clone)")
                    Object.Destroy(comp.gameObject);

            ModBoxInstalled.Clear();
            ModBoxDisabled.Clear();
            ModBoxAvailable.Clear();
            ModBoxUpdate.Clear();

            foreach (ModInfo mod in mods)
            {
                GameObject modBox = GameObject.Instantiate(ModBoxAsset, ContentPanel.transform);
                modBox.AddComponent<ModBoxScript>();
                ModBoxScript modBoxScript = modBox.GetComponent<ModBoxScript>();
                modBoxScript.modInfo = mod;
                modBoxScript.modsPnl = this;

                if (mod.local)
                {
                    if (mod.hasUpdate)
                    {
                        modBoxScript.tab = 3;
                        ModBoxUpdate.Add(modBox);
                    }
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
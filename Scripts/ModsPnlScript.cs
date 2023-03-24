using ModManager;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public class ModsPnlScript : MonoBehaviour
    {

        public Sprite menuButtonImg;

        public List<ModInfo> mods = new List<ModInfo>();
        public GameObject ModBoxAsset;
        public Toggle ModsToggle;

        public Sprite melonSprite;

        public GameObject ContentPanel;
        public Scrollbar verticalScrollbar;
        void Start()
        {
            Awake_Patch.Modspnlscript.ModsToggle.transform.GetChild(0).FindChild("TxtAchv").gameObject.GetComponent<Text>().text = "Mods";

            tabs = new List<GameObject>[] { ModBoxInstalled, ModBoxDisabled, ModBoxAvailable, ModBoxUpdate };

            verticalScrollbar = GetComponentInChildren<ScrollRect>().verticalScrollbar;
            buttons = GetComponentsInChildren<Button>();

            for (int i = 0; i < tabs.Length; i++)
            {
                int tempI = i;
                buttons[i].onClick.AddListener((System.Action)delegate { tab = tempI; refreshUI(); });
                buttons[i].image.sprite = menuButtonImg;
            }

            refreshUI();
        }

        Button[] buttons;

        private List<GameObject> ModBoxInstalled = new List<GameObject>();
        private List<GameObject> ModBoxDisabled = new List<GameObject>();
        private List<GameObject> ModBoxAvailable = new List<GameObject>();
        private List<GameObject> ModBoxUpdate = new List<GameObject>();

        public List<GameObject>[] tabs = new List<GameObject>[0];

        public List<GameObject> selects = new List<GameObject>();

        public int tab = 0;

        public void refreshUI()
        {
            if (!gameObject.active) return;

            for (int tabIndex = 0; tabIndex < tabs.Length; tabIndex++)
            {
                foreach (GameObject modBox in tabs[tabIndex])
                {
                    modBox.SetActive(tab == tabIndex);
                }

                buttons[tabIndex].transform.GetChild(0).GetComponent<Text>().fontStyle = tab == tabIndex ? FontStyle.Bold : FontStyle.Normal;
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
                    if (mod.enabled)
                        ModBoxInstalled.Add(modBox);
                    else
                        ModBoxDisabled.Add(modBox);
                    if (mod.hasUpdate)
                        ModBoxUpdate.Add(modBox);
                }
                else if (mod.online)
                    ModBoxAvailable.Add(modBox);
            }

            refreshUI();
        }

        public ModsPnlScript(System.IntPtr intPtr) : base(intPtr)
        {
        }
    }
}
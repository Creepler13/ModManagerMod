using ModManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{

    internal class ModBoxScript : MonoBehaviour
    {
        public ModsPnlScript modsPnl;
        public ModInfo modInfo;
        public int tab = 0;

        // Start is called before the first frame update
        void Start()
        {
            //tab 0 = installed  1 = disabled  2 = available

            Button[] buttons = GetComponentsInChildren<Button>();
            //0 = install/update/uninstall  1= disable/enable  2= uninstall
            Text[] texts = GetComponentsInChildren<Text>();

            texts[0].text = modInfo.Name;
            texts[1].text = " by " + modInfo.Author;
            texts[2].text = modInfo.Version != "" ? modInfo.Version : modInfo.onlineVersion;
            texts[3].text = modInfo.description;

            switch (tab)
            {
                case 0:
                    if (modInfo.hasUpdate)
                    {
                        buttons[0].GetComponentInChildren<Text>().text = "Update to " + modInfo.onlineVersion;
                        buttons[0].onClick.AddListener((System.Action)delegate { modInfo.update(); modsPnl.refresh(); });
                    }
                    else
                    {
                        buttons[0].gameObject.SetActive(false);
                    }

                    buttons[1].GetComponentInChildren<Text>().text = "Disable";

                    buttons[1].onClick.AddListener((System.Action)delegate { modInfo.disable(); modsPnl.refresh(); });
                    buttons[2].onClick.AddListener((System.Action)delegate { modInfo.delete(); modsPnl.refresh(); });
                    break;

                case 1:

                    if (modInfo.hasUpdate)
                    {
                        buttons[0].GetComponentInChildren<Text>().text = "Update to " + modInfo.onlineVersion;
                        buttons[0].onClick.AddListener((System.Action)delegate { modInfo.update(); modsPnl.refresh(); });
                    }
                    else
                    {
                        buttons[0].gameObject.SetActive(false);
                    }


                    buttons[1].GetComponentInChildren<Text>().text = "Enable";

                    buttons[1].onClick.AddListener((System.Action)delegate { modInfo.enable(); modsPnl.refresh(); });
                    buttons[2].onClick.AddListener((System.Action)delegate { modInfo.delete(); modsPnl.refresh(); });
                    break;

                case 2:
                    buttons[0].gameObject.SetActive(false);
                    buttons[1].gameObject.SetActive(false);

                    buttons[2].GetComponentInChildren<Text>().text = "Install";
                    buttons[2].onClick.AddListener((System.Action)delegate { modInfo.download(); modsPnl.refresh(); });
                    break;
            }


        }



        // Update is called once per frame
        void Update()
        {

        }

        public ModBoxScript(IntPtr intPtr) : base(intPtr)
        {
        }
    }
}
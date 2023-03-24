using ModManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace ModManager
{

    internal class ModBoxScript : MonoBehaviour
    {
        public ModsPnlScript modsPnl;
        public ModInfo modInfo;

        void Start()
        {

            Button[] buttons = GetComponentsInChildren<Button>();
            Text[] texts = GetComponentsInChildren<Text>();

            texts[0].text = modInfo.Name;
            texts[1].text = " by " + modInfo.Author;
            texts[2].text = modInfo.Version != "" ? modInfo.Version : modInfo.onlineVersion;
            texts[3].text = modInfo.description;

            if (modInfo.local)
            {
                //update button and text
                if (modInfo.hasUpdate)
                {
                    buttons[0].GetComponentInChildren<Text>().text = "Update to " + modInfo.onlineVersion;
                    buttons[0].onClick.AddListener((System.Action)delegate { modInfo.update(); modsPnl.refresh(); });
                }
                else
                {
                    buttons[0].gameObject.SetActive(false);
                }

                //enable-disable button and text
                if (!modInfo.enabled)
                {
                    buttons[1].GetComponentInChildren<Text>().text = "Enable";
                    buttons[1].onClick.AddListener((System.Action)delegate { modInfo.enable(); modsPnl.refresh(); });
                }
                else
                {
                    buttons[1].GetComponentInChildren<Text>().text = "Disable";
                    buttons[1].onClick.AddListener((System.Action)delegate { modInfo.disable(); modsPnl.refresh(); });
                }

                //deleteButton
                buttons[2].GetComponent<Image>().sprite = SpriteManager.load("delete.png");
                buttons[2].onClick.AddListener((System.Action)delegate { modInfo.delete(); modsPnl.refresh(); });

            }
            //install button and text
            else if (modInfo.online)
            {
                buttons[1].gameObject.SetActive(false);
                buttons[2].gameObject.SetActive(false);

                buttons[0].GetComponentInChildren<Text>().text = "Install";
                buttons[0].onClick.AddListener((System.Action)delegate { modInfo.download(); modsPnl.refresh(); });
            }
        }

        public ModBoxScript(IntPtr intPtr) : base(intPtr)
        {
        }
    }
}
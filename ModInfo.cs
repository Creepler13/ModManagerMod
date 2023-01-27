using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MelonLoader;
using System.Threading.Tasks;
using System.Net;

namespace ModManager
{
    public class ModInfo
    {
        public ModInfo(MelonInfoAttribute mod, string fileName)
        {
            this.Author = mod.Author;
            this.Name = mod.Name;
            this.Version = mod.Version;

            this.fileName = fileName;
        }

        public ModInfo() { }

        public ModInfo(string name, string author, string version, string fileName)
        {
            this.Author = author;
            this.Name = name;
            this.Version = version;
            this.fileName = fileName;
        }

        public string fileName;
        public string Name;
        // public string Description;
        public string Version = "";
        public string Author;
        public string description = "";
        public bool enabled;
        public bool local = false;

        public bool online = false;
        public string downloadLink;
        public string onlineVersion;
        public bool hasUpdate = false;

        //   public string[] dependency = new string[0];

        public void download()
        {

            Networking.DownloadFile(downloadLink, "mods/" + Name + ".dll");
            MelonLogger.Msg("Installed " + Name + " v" + onlineVersion);
        }

        public void delete()
        {
            File.Delete(((enabled) ? "mods" : "disabledMods") + "/" + fileName);
        }

        public void update()
        {
            //    WebClient client = new WebClient();
            //    client.DownloadFile(downloadLink, ((enabled) ? "mods" : "disabledMods") + "/" + Name + ".dll");
            Networking.DownloadFile(downloadLink, ((enabled) ? "mods" : "disabledMods") + "/" + Name + ".dll");
            MelonLogger.Msg("Updated " + Name + " " + Version + " -> " + onlineVersion);
        }


        public void disable()
        {

            if (!enabled) return;

            if (File.Exists("mods/" + fileName))
            {
                File.Move("mods/" + fileName, "disabledMods/" + fileName);
                enabled = false;
            }
        }

        public void enable()
        {
            if (enabled) return;
            if (File.Exists("disabledMods/" + fileName))
            {
                File.Move("disabledMods/" + fileName, "mods/" + fileName);
                enabled = false;
            }
        }

    }
}

using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModManager.utils
{
    internal class Settings
    {
        public static MelonPreferences_Category category;

        private static MelonPreferences_Entry<bool> doAutoUpdate;

        public static void load()
        {
            MelonLogger.Msg(Directory.GetCurrentDirectory());
            foreach (MelonPreferences_Category c in MelonPreferences.Categories)
            {
                if (c.Identifier == "ModUpdaterSettings")
                    category = c;
            }
            if (category == null)
            {
                Settings.category = MelonPreferences.CreateCategory("ModUpdaterSettings");
                Settings.category.IsInlined = false;
                Settings.category.SetFilePath("Userdata/ModUpdater.cfg", true);
                Settings.doAutoUpdate = Settings.category.CreateEntry<bool>("doAutoUpdate", false, "doAutoUpdate");
            }
            else
            {
                Settings.doAutoUpdate = Settings.category.GetEntry<bool>("doAutoUpdate");
            }
            save();
        }

        public static void save()
        {
            category.SaveToFile(false);
        }

        public static bool autoUpdate
        {
            get
            {
                return Settings.doAutoUpdate.Value;
            }
            set
            {
                Settings.doAutoUpdate.Value = value;
            }
        }
    }
}

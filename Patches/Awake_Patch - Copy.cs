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
using System.Drawing;
using Image = UnityEngine.UI.Image;

namespace ModManager
{

    [HarmonyPatch(typeof(MenuSelect), "OnAwake")]
    internal class Awake_PostFixPatch
    {

      

    }
}

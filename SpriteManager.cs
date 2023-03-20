using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ModManager
{
    internal class SpriteManager
    {

        private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public static Sprite load(string name)
        {
            string path = "ModManager.images." + name;

            if (sprites.ContainsKey(name))
                return sprites[name];

            Bitmap bm = new Bitmap(ModManagerTools.ReadResourceStream(path));

            Texture2D text = new Texture2D(bm.Width, bm.Height, TextureFormat.ARGB32, false);

            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    System.Drawing.Color c = bm.GetPixel(x, bm.Height - 1 - y);
                    text.SetPixel(x, y, new UnityEngine.Color32(c.R, c.G, c.B, c.A));
                }
            }


            text.Apply();
            sprites[name] = Sprite.Create(text, new Rect(0, 0, text.width, text.height), Vector2.zero);

            return sprites[name];
        }
    }



}

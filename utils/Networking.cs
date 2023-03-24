using Il2CppSystem;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine.Networking;
using System.Net;
using System.IO;

namespace ModManager.utils
{
    internal class Networking
    {


        public static string Get(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.AutomaticDecompression = DecompressionMethods.GZip;


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static void Get(string URL, Action<UnityWebRequest> callback)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(URL);
            webRequest.SendWebRequest();
            System.Func<bool> func = delegate ()
                            {
                                if (!webRequest.isDone)
                                {
                                    return false;
                                }
                                callback.Invoke(webRequest);
                                webRequest.Dispose();
                                return true;

                            };
            System.Action func2 = delegate () { };
            SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(func2, func
        , 0f, func2);

        }

        public static void DownloadFile(string URL, string path)
        {
            WebClient we = new WebClient();
            byte[] bytes = we.DownloadData(URL);
            FileWriter.writeFile(path, bytes);

        }

    }
}

using System;
using UnityEngine;

namespace ModManager.utils
{
    public class UnityUtils
    {

        public static T getComponentByName<T>(GameObject gm, string name)
        {
            foreach (T comp in gm.GetComponentsInChildren<T>())
            {

                if (((string)comp.GetType().GetField("name").GetValue(comp)) == name)
                    return comp;
            }
            return default(T);
        }

        public static Component getComponentByName(GameObject gm, string name, Type type)
        {
            foreach (Component comp in gm.GetComponentsInChildren<Component>())
            {
                if (((Component)comp).name == name)
                    if (comp.GetType() == type)
                        return comp;

            }



            return null;
        }


        public static Component getComponentByName(GameObject gm, string name)
        {
            foreach (Component comp in gm.GetComponentsInChildren<Component>())
            {
                if (((Component)comp).name == name)
                    return comp;
            }
            return null;
        }


    }
}

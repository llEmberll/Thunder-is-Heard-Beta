using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesUtils
{
    public static Sprite LoadIcon(string section, string name = null)
    {
        if (Config.resources.ContainsKey(section))
        {
            section = Config.resources[section];
        }

        if (name == null)
        {
            return Resources.Load<Sprite>(section);
        }

        Sprite[] iconSection = Resources.LoadAll<Sprite>(section);
        Sprite icon = null;
        if (iconSection.Length == 1)
        {
            icon = iconSection[0];
        }
        else if (name != "")
        {
            foreach (Sprite i in iconSection)
            {
                if (i.name == name)
                {
                    icon = i;
                    break;
                }
            }
        }

        return icon;
    }

}

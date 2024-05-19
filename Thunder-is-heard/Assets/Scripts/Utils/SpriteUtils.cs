using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SpriteUtils
{
    public static Sprite FindSpriteByName(string name,  Sprite[] sprites)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }

        return null;
    } 
}

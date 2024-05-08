using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildShopItem: ExposableShopItem
{
    public static string type = "Build";
    public override string Type { get { return type; } }
}

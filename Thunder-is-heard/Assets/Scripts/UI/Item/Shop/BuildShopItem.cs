using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildShopItem: ExposableShopItem
{
    public static string type = "Build";
    public override string Type { get { return type; } }


    public override void SaveExpose(Bector2Int[] occypation, int rotation)
    {
        ObjectProcessor.OnExposedBuild(coreId, name, occypation, rotation);
        resourcesProcessor.SubstractResources(costData);
        resourcesProcessor.Save();
    }
}

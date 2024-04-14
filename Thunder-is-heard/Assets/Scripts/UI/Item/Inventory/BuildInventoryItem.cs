using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInventoryItem: ExposableInventoryItem
{
    public static string type = "Build";
    public override string Type { get { return type; } }


    public override void SaveExpose(Bector2Int[] occypation, int rotation)
    {
        ObjectProcessor.OnExposedBuild(coreId, name, occypation, rotation);
    }
}

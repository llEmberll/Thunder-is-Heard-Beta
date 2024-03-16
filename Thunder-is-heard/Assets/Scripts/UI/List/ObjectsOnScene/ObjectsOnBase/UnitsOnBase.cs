

using UnityEngine;

public class UnitsOnBase : ObjectsOnBase
{
    public override Entity FindObjectById(string id)
    {
        throw new System.NotImplementedException();
    }

    public override void OnBuildModeEnable()
    {
    }

    public static void AddAndPrepareUnitComponent(GameObject unitObj, Transform model, string id, Vector2Int size, Vector2Int[] occypation)
    {
        Unit component = unitObj.AddComponent<Unit>();
        component.id = id;
        component.currentSize = size;
    }

    public static GameObject CreateUnitObject(Vector2Int position, string name, Transform parent)
    {
        return null;
    }
}

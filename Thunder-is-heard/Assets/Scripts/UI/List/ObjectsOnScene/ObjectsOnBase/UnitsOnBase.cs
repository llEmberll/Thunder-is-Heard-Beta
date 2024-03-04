

using UnityEngine;

public class UnitsOnBase : ItemList, IObjectsOnScene
{
    public Map map;

    public override void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<Map>();
        base.Start();
    }

    public override void OnBuildModeEnable()
    {
    }
}

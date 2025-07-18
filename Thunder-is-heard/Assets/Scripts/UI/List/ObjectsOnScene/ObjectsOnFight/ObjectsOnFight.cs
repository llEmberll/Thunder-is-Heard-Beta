
using UnityEngine;

public abstract class ObjectsOnFight : ItemList, IObjectsOnScene
{
    public Map map;

    public override void Start()
    {
        map = GameObject.FindWithTag(Tags.map).GetComponent<Map>();
        base.Start();
    }

    public abstract Entity FindObjectByCoreId(string id);
    public abstract Entity FindObjectByChildId(string id);
    public abstract bool IsProperType(string type);

    public override void OnClickOutside()
    {
        
    }
}

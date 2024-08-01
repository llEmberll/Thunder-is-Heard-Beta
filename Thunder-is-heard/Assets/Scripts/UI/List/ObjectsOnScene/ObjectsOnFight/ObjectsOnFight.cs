using UnityEngine;

public abstract class ObjectsOnFight : ItemList, IObjectsOnScene
{
    public Map map;

    public override void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<Map>();
        base.Start();
    }

    public abstract Entity FindObjectById(string id);

    public override void OnClickOutside()
    {
        
    }
}

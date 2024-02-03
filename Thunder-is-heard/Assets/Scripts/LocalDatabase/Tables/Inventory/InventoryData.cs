using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventoryData : SomeTableItem
{
    [Tooltip("“ËÔ")]
    [SerializeField] public string type;
    public string Type
    {
        get { return type; }
        set { }
    }

    [Tooltip("id")]
    [SerializeField] public int id;
    public int Id
    {
        get { return id; }
        set { }
    }

    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "type", type },
            { "id", id }
        };
    }

    public override ITableItem Clone()
    {
        InventoryData clone = new InventoryData();
        clone.type = type;
        clone.id = id;
        return clone;
    }
}

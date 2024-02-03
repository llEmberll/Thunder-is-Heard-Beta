using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


[System.Serializable]
public class ShopData : SomeTableItem
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

    [Tooltip("cost")]
    [SerializeField] public ResourcesData cost;
    public ResourcesData Cost
    {
        get { return cost; }
        set { }
    }

    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "type", type },
            { "id", id },
            { "cost", cost}
        };
    }

    public override ITableItem Clone()
    {
        ShopData clone = new ShopData();
        clone.type = type;
        clone.id = id;
        
        ResourcesData cloneCost = cost.Clone();
        clone.cost = cloneCost;

        return clone;
    }
}

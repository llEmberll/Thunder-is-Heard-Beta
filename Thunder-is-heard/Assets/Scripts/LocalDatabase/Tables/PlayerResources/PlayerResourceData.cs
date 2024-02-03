using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PlayerResourceData : SomeTableItem
{
    [Tooltip("Ресурсы")]
    [SerializeField] public ResourcesData resources;
    public ResourcesData Resources
    {
        get { return resources; }
        set { }
    }

    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "resources", resources }
        };
    }

    public override ITableItem Clone()
    {
        PlayerResourceData clone = new PlayerResourceData();
        clone.resources = resources.Clone();
        return clone;
    }
}


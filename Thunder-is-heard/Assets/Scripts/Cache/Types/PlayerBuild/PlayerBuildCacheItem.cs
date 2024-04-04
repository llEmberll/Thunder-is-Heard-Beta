using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerBuildCacheItem : CacheItem
{
    public PlayerBuildCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("rotation"))
        {
            SetRotation(0);
        }
    }

    public Bector2Int[] GetPosition()
    {
        object value = GetField("position");
        if (value == null)
        {
            throw new Exception("Undefined build position");
        }

        return JsonConvert.DeserializeObject<Bector2Int[]>(value.ToString());
    }

    public void SetPosition(Bector2Int[] value)
    {
        SetField("position", value);
    }

    public int GetRotation()
    {
        object value = GetField("rotation");
        return (value != null) ? Convert.ToInt32(value) : 0;

    }

    public void SetRotation(int value)
    {
        SetField("rotation", value);
    }

    public override CacheItem Clone()
    {
        BuildCacheItem clone = new BuildCacheItem(fields);
        return clone;
    }
}

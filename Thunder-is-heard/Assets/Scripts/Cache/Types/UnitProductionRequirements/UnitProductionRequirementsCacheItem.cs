using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class UnitProductionRequirementsCacheItem : CacheItem
{
    public UnitProductionRequirementsCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("data"))
        {
            SetData(new UnitProductionRequirementsData());
        }
    }

    public string GetUnitProductionId()
    {
        return (string)GetField("unitProductionId");
    }

    public void SetUnitProductionId(string value)
    {
        SetField("unitProductionId", value);
    }

    public UnitProductionRequirementsData GetData()
    {
        object value = GetField("data");
        if (value == null)
        {
            return new UnitProductionRequirementsData();
        }

        return JsonConvert.DeserializeObject<UnitProductionRequirementsData>(value.ToString());
    }

    public void SetData(UnitProductionRequirementsData value)
    {
        SetField("data", value);
    }


    public override CacheItem Clone()
    {
        UnitProductionRequirementsCacheItem clone = new UnitProductionRequirementsCacheItem(fields);
        return clone;
    }
}

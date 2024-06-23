using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class MissionCacheItem : CacheItem
{
    public MissionCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("iconPath"))
        {
            SetIconPath("");
        }

        if (!objFields.ContainsKey("scenarioId"))
        {
            SetScenarioId("");
        }

        if (!objFields.ContainsKey("rewards"))
        {
            SetRewards(new RewardData[] { });
        }

        if (!objFields.ContainsKey("passed"))
        {
            SetPassed(false);
        }

        if (!objFields.ContainsKey("gives"))
        {
            SetGives(new ResourcesData());
        }
    }

    public string GetIconPath()
    {
        return (string)GetField("iconPath");
    }

    public void SetIconPath(string value)
    {
        SetField("iconPath", value);
    }

    public string GetScenarioId()
    {
        return (string)GetField("scenarioId");
    }

    public void SetScenarioId(string value)
    {
        SetField("scenarioId", value);
    }

    public RewardData[] GetRewards()
    {
        object value = GetField("rewards");
        if (value == null)
        {
            return new RewardData[] { };
        }

        return JsonConvert.DeserializeObject<RewardData[]>(value.ToString());
    }

    public ResourcesData GetGives()
    {
        object value = GetField("gives");
        if (value == null)
        {
            return new ResourcesData();
        }

        return JsonConvert.DeserializeObject<ResourcesData>(value.ToString());
    }

    public void SetGives(ResourcesData value)
    {
        SetField("gives", value);
    }

    public void SetRewards(RewardData[] value)
    {
        SetField("rewards", value);
    }

    public bool GetPassed()
    {
        return (bool)GetField("passed");
    }

    public void SetPassed(bool value)
    {
        SetField("passed", value);
    }

    public override CacheItem Clone()
    {
        MissionCacheItem clone = new MissionCacheItem(fields);
        clone.SetGives(GetGives().Clone());
        return clone;
    }
}

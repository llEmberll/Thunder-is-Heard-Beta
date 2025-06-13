using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class MissionCacheItem : CacheItem
{
    public MissionCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {

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

        if (!objFields.ContainsKey("poseOnMap"))
        {
            SetPoseOnMap(new Bector2Int(new UnityEngine.Vector2Int(0, 0)));
        }
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

        if (value is RewardData[] typedValue)
        {
            return typedValue;
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

        if (value is ResourcesData typedValue)
        {
            return typedValue;
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

    public Bector2Int GetPoseOnMap()
    {
        object value = GetField("poseOnMap");
        if (value == null)
        {
            return null;
        }

        if (value is Bector2Int typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<Bector2Int>(value.ToString());
    }

    public void SetPoseOnMap(Bector2Int value)
    {
        SetField("poseOnMap", value);
    }

    public override CacheItem Clone()
    {
        MissionCacheItem clone = new MissionCacheItem(fields);
        clone.SetGives(GetGives().Clone());
        return clone;
    }
}

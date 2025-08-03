using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class BattleCacheItem : CacheItem
{
    public BattleCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("missionId"))
        {
            SetMissionId("");
        }

        if (!objFields.ContainsKey("currentStage"))
        {
            SetCurrentStage(null);
        }

        if (!objFields.ContainsKey("units"))
        {
            SetUnits(new UnitOnBattle[] { });
        }

        if (!objFields.ContainsKey("builds"))
        {
            SetBuilds(new BuildOnBattle[] { });
        }

        if (!objFields.ContainsKey("obstacles"))
        {
            SetObstacles(new ObstacleOnBattle[] { });
        }

        if (!objFields.ContainsKey("turn"))
        {
            SetTurn(Sides.federation);
        }

        if (!objFields.ContainsKey("turnIndex"))
        {
            SetTurnIndex(0);
        }

        if (!objFields.ContainsKey("customData"))
        {
            SetCustomData(new Dictionary<string, object>());
        }
    }

    public string? GetMissionId()
    {
        return (string?)GetField("missionId");
    }

    public void SetMissionId(string value)
    {
        SetField("missionId", value);
    }

    public StageData GetCurrentStage()
    {
        object value = GetField("currentStage");

        if (value == null)
        {
            return null;
        }

        if (value is StageData typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<StageData>(value.ToString());
    }

    public void SetCurrentStage(StageData value)
    {
        SetField("currentStage", value);
    }

    public UnitOnBattle[] GetUnits()
    {
        object value = GetField("units");
        if (value == null)
        {
            return new UnitOnBattle[] { };
        }

        if (value is UnitOnBattle[] typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<UnitOnBattle[]>(value.ToString());
    }

    public void SetUnits(UnitOnBattle[] value)
    {
        SetField("units", value);
    }

    public BuildOnBattle[] GetBuilds()
    {
        object value = GetField("builds");
        if (value == null)
        {
            return new BuildOnBattle[] { };
        }

        if (value is BuildOnBattle[] typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<BuildOnBattle[]>(value.ToString());
    }

    public void SetBuilds(BuildOnBattle[] value)
    {
        SetField("builds", value);
    }

    public ObstacleOnBattle[] GetObstacles()
    {
        object value = GetField("obstacles");
        if (value == null)
        {
            return new ObstacleOnBattle[] { };
        }

        if (value is ObstacleOnBattle[] typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<ObstacleOnBattle[]>(value.ToString());
    }

    public void SetObstacles(ObstacleOnBattle[] value)
    {
        SetField("obstacles", value);
    }

    public string GetTurn()
    {
        return (string)GetField("turn");
    }

    public void SetTurn(string value)
    {
        SetField("turn", value);
    }

    public int GetTurnIndex()
    {
        object value = GetField("turnIndex");

        if (value == null)
        {
            return 0;
        }

        return Convert.ToInt32(value);
    }

    public void SetTurnIndex(int value)
    {
        SetField("turnIndex", value);
    }

    public Dictionary<string, object> GetCustomData()
    {
        object value = GetField("customData");
        if (value == null)
        {
            return new Dictionary<string, object>();
        }

        if (value is Dictionary<string, object> typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<Dictionary<string, object>>(value.ToString());
    }

    public void SetCustomData(Dictionary<string, object> value)
    {
        SetField("customData", value);
    }

    public void SetCustomDataValue(string key, object value)
    {
        Dictionary<string, object> customData = GetCustomData();
        if (customData == null)
        {
            customData = new Dictionary<string, object>();
        }
        customData[key] = value;
        SetCustomData(customData);
    }

    public T GetCustomDataValue<T>(string key, T defaultValue = default(T))
    {
        Dictionary<string, object> customData = GetCustomData();
        if (customData.ContainsKey(key))
        {
            object value = customData[key];
            if (value is T typedValue)
            {
                return typedValue;
            }
            
            try
            {
                return JsonConvert.DeserializeObject<T>(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        return defaultValue;
    }

    public override CacheItem Clone()
    {
        BattleCacheItem clone = new BattleCacheItem(fields);
        return clone;
    }
}

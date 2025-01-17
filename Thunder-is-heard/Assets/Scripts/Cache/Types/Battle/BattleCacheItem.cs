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

        if (!objFields.ContainsKey("isLanded"))
        {
            SetIsLanded(false);
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

        return JsonConvert.DeserializeObject<StageData>(value.ToString());
    }

    public void SetCurrentStage(StageData value)
    {
        SetField("currentStage", value);
    }

    public bool GetIsLanded()
    {
        return (bool)GetField("isLanded");
    }

    public void SetIsLanded(bool value)
    {
        SetField("isLanded", value);
    }

    public UnitOnBattle[] GetUnits()
    {
        object value = GetField("units");
        if (value == null)
        {
            return new UnitOnBattle[] { };
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

    public override CacheItem Clone()
    {
        BattleCacheItem clone = new BattleCacheItem(fields);
        return clone;
    }
}

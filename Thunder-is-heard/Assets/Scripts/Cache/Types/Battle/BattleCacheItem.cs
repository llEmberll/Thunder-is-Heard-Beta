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

        if (!objFields.ContainsKey("stageIndex"))
        {
            SetStageIndex(0);
        }

        if (!objFields.ContainsKey("map"))
        {
            SetMap(new CellData[] { });
        }

        if (!objFields.ContainsKey("units"))
        {
            SetUnits(new UnitOnBattle[] { });
        }

        if (!objFields.ContainsKey("builds"))
        {
            SetBuilds(new BuildOnBattle[] { });
        }

        if (!objFields.ContainsKey("turn"))
        {
            SetTurn(Sides.federation);
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

    public int GetStageIndex()
    {
        return (int)GetField("stageIndex");
    }

    public void SetStageIndex(int value)
    {
        SetField("stageIndex", value);
    }

    public CellData[] GetMap()
    {
        object value = GetField("map");
        if (value == null)
        {
            return new CellData[] {};
        }

        return JsonConvert.DeserializeObject<CellData[]>(value.ToString());
    }

    public void SetMap(CellData[] value)
    {
        SetField("map", value);
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

    public string GetTurn()
    {
        return (string)GetField("turn");
    }

    public void SetTurn(string value)
    {
        SetField("turn", value);
    }

    public override CacheItem Clone()
    {
        BattleCacheItem clone = new BattleCacheItem(fields);
        return clone;
    }
}

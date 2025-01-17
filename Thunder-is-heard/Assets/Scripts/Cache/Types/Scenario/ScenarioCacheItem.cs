using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class ScenarioCacheItem : CacheItem
{
    public ScenarioCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("terrainPath"))
        {
            SetTerrainPath("");
        }

        if (!objFields.ContainsKey("mapSize"))
        {
            SetMapSize(new Bector2Int(new UnityEngine.Vector2Int(15, 15)));
        }

        if (!objFields.ContainsKey("units"))
        {
            SetUnits(new UnitOnBattle[] { });
        }

        if (!objFields.ContainsKey("builds"))
        {
            SetBuilds(new BuildOnBattle[] { });
        }

        if (!objFields.ContainsKey("startDialogue"))
        {
            SetStartDialogue(new Replic[] { });
        }

        if (!objFields.ContainsKey("obstacles"))
        {
            SetObstacles(new ObstacleOnBattle[] { });
        }

        if (!objFields.ContainsKey("landing"))
        {
            SetLanding(new LandingData(new Bector2Int[] {}, 0));
        }

        if (!objFields.ContainsKey("startStage"))
        {
            SetStartStage(new StageData());
        }
    }

    public string GetTerrainPath()
    {
        return (string)GetField("terrainPath");
    }

    public void SetTerrainPath(string value)
    {
        SetField("terrainPath", value);
    }

    public Bector2Int GetMapSize()
    {
        object value = GetField("mapSize");
        return JsonConvert.DeserializeObject<Bector2Int>(value.ToString());
    }

    public void SetMapSize(Bector2Int value)
    {
        SetField("mapSize", value);
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

    public Replic[] GetStartDialogue()
    {
        object value = GetField("startDialogue");
        if (value == null)
        {
            return new Replic[] { };
        }

        return JsonConvert.DeserializeObject<Replic[]>(value.ToString());
    }

    public void SetStartDialogue(Replic[] value)
    {
        SetField("startDialogue", value);
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

    public LandingData GetLanding()
    {
        object value = GetField("landing");
        if (value == null)
        {
            return new LandingData(new Bector2Int[] { }, 0);
        }

        return JsonConvert.DeserializeObject<LandingData>(value.ToString());
    }

    public void SetLanding(LandingData value)
    {
        SetField("landing", value);
    }

    public StageData GetStartStage()
    {
        object value = GetField("startStage");
        if (value == null)
        {
            return new StageData();
        }

        return JsonConvert.DeserializeObject<StageData>(value.ToString());
    }

    public void SetStartStage(StageData value)
    {
        SetField("startStage", value);
    }

    public override CacheItem Clone()
    {
        ScenarioCacheItem clone = new ScenarioCacheItem(fields);
        return clone;
    }
}

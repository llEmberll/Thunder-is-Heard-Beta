using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class FightProcessor : MonoBehaviour
{
    public BattleCacheItem battleData;

    public Scenario scenario;
    public Scenario Scenario { get { return scenario; } }

    public void Init(string battleId)
    {
        InitBattleData(battleId);

        InitEvents();

        ConstructScenario();

        InstantiateMap();
        //TODO InstantiateTerrain()
        InstantiateObjects();

        EnableListeners();
    }

    public void InitBattleData(string battleId)
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        battleData = new BattleCacheItem(cacheItem.Fields);
    }

    public void InitEvents()
    {
        EventMaster.current.FightIsStarted += StartFight;
    }

    public void ConstructScenario()
    {
        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem cacheItemMission = missionTable.GetById(battleData.GetMissionId());
        MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

        ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
        CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
        ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

        //TODO решить, как создавать клетки из сценария(генератором, или расширить класс Map)

        //scenario = new Scenario(map, terrain, objects, stages, landableCells) ;
    }

    public Map GetMapFromData(Dictionary<string, object> data)
    {
        string pathToMap = (string)data["map"];
        return Resources.Load<GameObject>(pathToMap).GetComponent<Map>();
    }

    public Sprite GetTerrainFromData(Dictionary<string, object> data)
    {
        string pathToTerrain = (string)data["terrain"];
        return Resources.Load<Sprite>(pathToTerrain);
    }

    public Dictionary<Vector2Int, Entity> GetObjectsFromData(Dictionary<string, object> data)
    {
        Dictionary<Vector2Int, Entity> objects = new Dictionary<Vector2Int, Entity>();

        Dictionary<Vector2Int, string> objectsData = (Dictionary<Vector2Int, string>)data["objects"];
        foreach (var objectData in objectsData)
        {
            GameObject obj = Resources.Load<GameObject>(objectData.Value);
            objects.Add(objectData.Key, obj.GetComponent<Entity>());
        }

        return objects;
    }

    public List<IStage> GetStagesFromData(Dictionary<string, object> data)
    {
        List<IStage> stages = new List<IStage>();
        List<string> stageClasses = (List<string>)data["stages"];
        foreach(var className in stageClasses)
        {
            Type type = Assembly.GetExecutingAssembly().GetType(className);
            IStage stage = (IStage)Activator.CreateInstance(type);
            stages.Add(stage);
        }

        return stages;
    }

    public List<Vector2Int> GetLandableCellsFromData(Dictionary<string, object> data)
    {
        return (List<Vector2Int>)data["landableCells"];
    }

    public void InstantiateMap()
    {
        Instantiate(scenario.Map.gameObject, scenario.Map.transform.position, Quaternion.identity);
    }
    
    public void InstantiateObjects() 
    {
        //TODO создать на сцене юниты и здания из battleData

        //foreach (var objectData in scenario.Objects)
        //{
        //    Instantiate(objectData.Value.gameObject, new Vector3(
        //        objectData.Key.x, 
        //        objectData.Value.transform.position.y, 
        //        objectData.Key.y
        //        ), Quaternion.identity);
        //}
    }

    public void EnableListeners()
    {
        EventMaster.current.FightLost += Defeat;
        EventMaster.current.FightWon += Victory;
    }

    public void DisableListeners()
    {
        EventMaster.current.FightLost -= Defeat;
        EventMaster.current.FightWon -= Victory;
    }

    public void Landing()
    {
        GlowLandableCells();
        //TODO open landing panel
        //TODO switch state for building
    }

    public void StartFight()
    {
        TurnOffLandableCells();
        //TODO hide landing panel
        //TODO return to fight state

        Scenario.Begin();
    }

    public void NextTurn()
    {
        Scenario.OnNextTurn();
    }

    public void Defeat()
    {
        DisableListeners();
    }

    public void Victory()
    {
        DisableListeners();
    }

    public void GlowLandableCells()
    {
        Dictionary<Vector2Int, Cell> landableCells = Scenario.Map.FindCellsByPosition(Scenario.LandableCells);

        foreach(var item in landableCells)
        {
            MeshRenderer renderer = item.Value.gameObject.GetComponent<MeshRenderer>();
            renderer.material = Resources.Load(Config.resources["landableCellMaterial"], typeof(Material)) as Material;
        }
    }

    public void TurnOffLandableCells()
    {
        Dictionary<Vector2Int, Cell> landableCells = Scenario.Map.FindCellsByPosition(Scenario.LandableCells);

        foreach (var item in landableCells)
        {
            MeshRenderer renderer = item.Value.gameObject.GetComponent<MeshRenderer>();
            renderer.material = Resources.Load(Config.resources["defaultCellMaterial"], typeof(Material)) as Material;
        }
    }
}

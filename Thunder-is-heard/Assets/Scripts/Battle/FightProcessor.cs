using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightProcessor : MonoBehaviour
{
    public string _battleId;
    public BattleCacheItem _battleData;

    public Scenario _scenario;
    public Scenario Scenario { get { return _scenario; } }

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        InitBattleData(FightSceneLoader.parameters._battleId);

        ConstructScenario();

        EnableListeners();

        ContinueFight();
    }

    public void InitBattleData(string battleId)
    {
        _battleId = battleId;
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        _battleData = new BattleCacheItem(cacheItem.Fields);
    }

    public void ConstructScenario()
    {
        _scenario = new Scenario();

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem cacheItemMission = missionTable.GetById(_battleData.GetMissionId());
        MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

        ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
        CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
        ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

        bool isLanded = _battleData.GetIsLanded();

        int stageIndex = _battleData.GetStageIndex();
        StageData[] stageDatas = scenarioData.GetStages();
        List<IStage> stages = StageFactory.GetAndInitStagesByStageDatasAndScenario(stageDatas, Scenario);

        UnitOnBattle[] scenarioUnits = scenarioData.GetUnits();
        BuildOnBattle[] scenarioBuilds = scenarioData.GetBuilds();

        LandingData landingData = scenarioData.GetLanding();
        List<Vector2Int> landingCells = Bector2Int.MassiveToVector2Int(landingData.zone).ToList();
        int landingMaxStaff = landingData.maxStaff;

        Vector2Int mapSize = scenarioData.GetMapSize().ToVector2Int();
        string terrainPath = scenarioData.GetTerrainPath();
        Map map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        map.Init(mapSize, terrainPath);

        Scenario.Init(map, scenarioUnits, scenarioBuilds, landingCells, landingMaxStaff, stages, stageIndex, isLanded);
    }

    public void EnableListeners()
    {
        EventMaster.current.FightLost += Defeat;
        EventMaster.current.FightWon += Victory;
        EventMaster.current.FightIsStarted += StartFight;
        EventMaster.current.StartLanding += Landing;
        EventMaster.current.BattleChanged += ReloadBattleData;
    }

    public void DisableListeners()
    {
        EventMaster.current.FightLost -= Defeat;
        EventMaster.current.FightWon -= Victory;
    }

    public void ReloadBattleData(string battleId)
    {
        InitBattleData(battleId);
    }

    public string GetBattleId()
    {
        return _battleId;
    }

    public void Landing(List<Vector2Int> landableCells, int landingMaxStaff)
    {
        Scenario.Map.Display(landableCells);
        //TODO open landing panel
        //TODO switch state for building
    }

    public void ContinueFight()
    {
        Scenario.Begin();
    }

    public void StartFight()
    {
        Scenario.Map.HideAll();
        //TODO hide landing panel
        //TODO return to fight state

        Scenario._isLanded = true;
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
}

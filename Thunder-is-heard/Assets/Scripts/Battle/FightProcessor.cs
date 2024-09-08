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


    public ObjectProcessor _objectProcessor;


    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        InitBattleData(FightSceneLoader.parameters._battleId);

        ConstructScenario();

        EnableListeners();

        InitObjectProcessor();
    }

    public void EnableListeners()
    {
        EventMaster.current.FightLost += Defeat;
        EventMaster.current.FightWon += Victory;
        EventMaster.current.FightIsStarted += StartFight;
        EventMaster.current.BattleObjectsChanged += ReloadBattleData;

        EventMaster.current.TurnExecuted += ExecuteTurn;
    }

    public void DisableListeners()
    {
        EventMaster.current.FightLost -= Defeat;
        EventMaster.current.FightWon -= Victory;
        EventMaster.current.FightIsStarted -= StartFight;
        EventMaster.current.BattleObjectsChanged -= ReloadBattleData;

        EventMaster.current.TurnExecuted -= ExecuteTurn;
    }

    public void Start()
    {
        ContinueFight();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void InitBattleData(string battleId)
    {
        _battleId = battleId;
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        _battleData = new BattleCacheItem(cacheItem.Fields);
    }

    public void ChangeSideTurn()
    {
        Dictionary<string, string> newTurnByCurrentTurn = new Dictionary<string, string>()
        {
            {Sides.federation, Sides.empire },
            {Sides.empire, Sides.neutral },
            {Sides.neutral, Sides.federation },
        };

        string newTurn = newTurnByCurrentTurn[_battleData.GetTurn()];
        _battleData.SetTurn(newTurn);
    }

    public void IncrementTurnIndex()
    {
        _battleData.SetTurnIndex(_battleData.GetTurnIndex() + 1);
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

    public void ReloadBattleData()
    {
        InitBattleData(_battleId);
    }

    public string GetBattleId()
    {
        return _battleId;
    }

    public void ContinueFight()
    {
        Debug.Log("FightProcessor: continue fight");

        Scenario.Begin();
    }

    public void StartFight()
    {
        EventMaster.current.OnBaseMode();

        Scenario._isLanded = true;
        Scenario.Begin();
    }

    public void NextTurn()
    {
        UpdateEffects();

        ChangeSideTurn();
        IncrementTurnIndex();

        EventMaster.current.OnNextTurn(_battleData.GetTurn());
        Scenario.OnNextTurn();
    }

    public void ExecuteTurn(TurnData turnData)
    {
        //Логика реализации выбранного хода с обработкой скил-контроллера
        //Обновить _battleData

        NextTurn();
    }

    public void ExecuteSkill()
    {
        //Логика использования скила
        //Обновить _battleData
    }

    public void UpdateEffects()
    {
        // Обновить все эффекты на юнитах(например от скиллов) согласно кд и условиям прекращения или продолжения эффекта.
        // Наложить заново эффект от пассивных скиллов если условия соблюдаются
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

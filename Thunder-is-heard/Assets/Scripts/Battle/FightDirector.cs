using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightDirector : MonoBehaviour
{
    public string _battleId;
    public BattleCacheItem _battleData;

    public Scenario _scenario;
    public Scenario Scenario { get { return _scenario; } }


    public ObjectProcessor _objectProcessor;

    public BattleEngine _battleEngine;

    public UnitsOnFight _unitsOnFightManager;
    public BuildsOnFight _buildsOnFightManager;

    public TurnController _turnController;


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
        InitBattleEngine();
        InitUnitsOnFightManager();
        InitBuildsOnFightManager();
        InitTurnController();
    }

    public void EnableListeners()
    {
        EventMaster.current.FightLost += Defeat;
        EventMaster.current.FightWon += Victory;
        EventMaster.current.FightIsStarted += StartFight;
        EventMaster.current.BattleObjectsChanged += ReloadBattleData;

        EventMaster.current.TurnExecuted += ExecuteTurn;
        EventMaster.current.StageIndexChanged += ChangeStageIndex;
    }

    public void DisableListeners()
    {
        EventMaster.current.FightLost -= Defeat;
        EventMaster.current.FightWon -= Victory;
        EventMaster.current.FightIsStarted -= StartFight;
        EventMaster.current.BattleObjectsChanged -= ReloadBattleData;

        EventMaster.current.TurnExecuted -= ExecuteTurn;
        EventMaster.current.StageIndexChanged -= ChangeStageIndex;
    }

    public void Start()
    {
        ContinueFight();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void InitUnitsOnFightManager()
    {
        _unitsOnFightManager = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
    }

    public void InitBuildsOnFightManager()
    {
        _buildsOnFightManager = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnFight>();
    }

    public void InitTurnController()
    {
        _turnController = GameObject.FindGameObjectWithTag(Tags.turnController).GetComponent<TurnController>();
    }

    public void InitBattleData(string battleId)
    {
        _battleId = battleId;
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        _battleData = new BattleCacheItem(cacheItem.Fields);
    }

    public void SaveBattleData()
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        battleTable.ChangeById(_battleId, _battleData);
        Cache.Save(battleTable);
        InitBattleData(_battleId);
    }

    public void SyncBattleDataToCurrentBattleSituation()
    {
        Dictionary<string, UnitOnBattle> allUnitsOnBattle = BattleEngine.GetAllUnitsInBattle(_battleEngine.currentBattleSituation);
        Dictionary<string, BuildOnBattle> allBuildsOnBattle = BattleEngine.GetAllBuildsInBattle(_battleEngine.currentBattleSituation);

        UnitOnBattle[] newUnitsForBattleData = allUnitsOnBattle.Values.ToArray();
        BuildOnBattle[] newBuildsForBattleData = allBuildsOnBattle.Values.ToArray();
        _battleData.SetUnits(newUnitsForBattleData);
        _battleData.SetBuilds(newBuildsForBattleData);
        SaveBattleData();
    }

    public void ChangeSideTurn()
    {
        string newTurn = Sides.nextSideTurnByCurrentSide[_battleData.GetTurn()];
        _battleData.SetTurn(newTurn);
        SaveBattleData();
    }

    public void IncrementTurnIndex()
    {
        _battleData.SetTurnIndex(_battleData.GetTurnIndex() + 1);
        SaveBattleData();
    }

    public void ChangeStageIndex(int index)
    {
        _battleData.SetStageIndex(index);
        SaveBattleData();
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
        _scenario.map.HideAll();

        Scenario._isLanded = true;
        Scenario.Begin();

        _turnController.OnNextTurn(_battleData.GetTurn());
    }

    public void NextTurn()
    {
        Debug.Log("Следующий ход");

        UpdateEffects();
        Debug.Log("Эффекты  обновлены");

        ChangeSideTurn();

        IncrementTurnIndex();

        Debug.Log("Данные по очередерности обновлены");

        Scenario.OnNextTurn();

        Debug.Log("Сценарий обновлен");

        SyncBattleDataToCurrentBattleSituation();

        Debug.Log("Битва синхронизирована");

        EventMaster.current.OnNextTurn(_battleData.GetTurn());
        _turnController.OnNextTurn(_battleData.GetTurn());
    }

    public void ExecuteTurn(TurnData turnData)
    {
        if (turnData != null)
        {
            if (IsTurnContainsMovement(turnData)) 
            {
                Debug.Log("Ход с передвижением");

                Unit activeUnit = _unitsOnFightManager.FindObjectByChildId(turnData._activeUnitIdOnBattle) as Unit;

                BattleEngine.OnReplaceUnit(_battleEngine.currentBattleSituation, activeUnit, turnData._route.Last());

                ChangeUnitOccypation(activeUnit, _scenario.map.Cells[turnData._route.Last().ToVector2()]);

                Debug.Log(activeUnit.name + "движется к позиции " + turnData._route.Last());

                activeUnit.Move(_battleEngine.GetCellsByBector2IntPositions(turnData._route));

                // Подождать прибытия юнита
                //Время ожидания в секундах = скорость юнита * длина маршрута * 0.5
            }

            if (IsTurnContainsTarget(turnData))
            {
                Entity target = _unitsOnFightManager.FindObjectByChildId(turnData._targetIdOnBattle);
                if (target == null)
                {
                    target = _buildsOnFightManager.FindObjectByChildId(turnData._targetIdOnBattle);
                }

                Debug.Log("Ход с атакой по " + target.name);

                UnitOnBattle[] attackersData = _battleEngine.currentBattleSituation.attackersByObjectId[target.ChildId].ToArray().OfType<UnitOnBattle>().ToArray();
                if (attackersData != null && attackersData.Length > 0)
                {

                    Debug.Log(attackersData.Count() + "атакующих");

                    List<Unit> attackers = _unitsOnFightManager.GetUnitsByBattleUnitsData(attackersData);
                    int damage = BattleEngine.CalculateDamageToEntity(_battleEngine.currentBattleSituation, attackersData, target); // Нужно учесть модификаторы от скилов + эффекты

                    Debug.Log("Урон " + damage);

                    foreach (Unit attacker in attackers)
                    {
                        attacker.Attack(target);
                    }

                    // Подождать конца атаки и ранения цели = 1 с
                    Debug.Log("Цель сейчас получит");
                    BattleEngine.OnAttackTarget(_battleEngine.currentBattleSituation, target, damage);

                    target.GetDamage(damage);
                }
            }
        }

        NextTurn();
    }

    public void ChangeUnitOccypation(Unit unit, Cell newOccypation)
    {
        Cell unitCurrentCell = _scenario.map.Cells[unit.center];
        unitCurrentCell.Free();

        unit.center = newOccypation.position;
        newOccypation.Occupy();

        UpdateUnitPositionInCache(unit);
    }

    public void UpdateUnitPositionInCache(Unit unit)
    {
        UnitOnBattle[] unitsInCache = _battleData.GetUnits();

        foreach (UnitOnBattle unitInCache in unitsInCache)
        {
            if (unitInCache.idOnBattle == unit.ChildId)
            {
                unitInCache.position = new Bector2Int[] { new Bector2Int(unit.center) };
                unitInCache.rotation = unit.rotation;
                break;
            }
        }

        _battleData.SetUnits(unitsInCache);
        SaveBattleData();
    }

    public bool IsTurnContainsMovement(TurnData turnData)
    {
        return turnData._activeUnitIdOnBattle != null && turnData._route != null && turnData._route.Count() > 0;
    }

    public bool IsTurnContainsTarget(TurnData turnData)
    {
        return turnData._targetIdOnBattle != null;
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

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

    public ResourcesProcessor _resourceProcessor;

    public DialogueController _dialogueController;


    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        InitBattleData(FightSceneLoader.parameters._battleId);

        InitScenario();

        EnableListeners();

        InitObjectProcessor();
        InitBattleEngine();
        InitUnitsOnFightManager();
        InitBuildsOnFightManager();
        InitTurnController();
        InitResourcesProcessor();
        InitDialogueController();
    }

    public void EnableListeners()
    {
        EventMaster.current.FightLost += Defeat;
        EventMaster.current.FightWon += Victory;
        EventMaster.current.FightIsStarted += StartFight;
        EventMaster.current.BattleObjectsChanged += ReloadBattleData;

        EventMaster.current.TurnExecuted += ExecuteTurn;
        EventMaster.current.StageIndexChanged += ChangeStageIndex;
        EventMaster.current.ScenarioUpdated += OnScenarioUpdated;
    }

    public void DisableListeners()
    {
        EventMaster.current.FightLost -= Defeat;
        EventMaster.current.FightWon -= Victory;
        EventMaster.current.FightIsStarted -= StartFight;
        EventMaster.current.BattleObjectsChanged -= ReloadBattleData;

        EventMaster.current.TurnExecuted -= ExecuteTurn;
        EventMaster.current.StageIndexChanged -= ChangeStageIndex;
        EventMaster.current.ScenarioUpdated -= OnScenarioUpdated;
    }


    public void Start()
    {
        if (Scenario._isLanded)
        {
            ContinueFight();
        }
        else
        {
            StartCoroutine(BeforeScenarioBegin());
        }
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

    public void InitResourcesProcessor()
    {
        _resourceProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }

    public void InitDialogueController()
    {
        _dialogueController = GameObject.FindGameObjectWithTag(Tags.dialogueController).GetComponent<DialogueController>();
    }

    public void InitBattleData(string battleId)
    {
        _battleId = battleId;
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        _battleData = new BattleCacheItem(cacheItem.Fields);
    }

    public void SetLandedInBattleData()
    {
        _battleData.SetIsLanded(true);
    }

    public void SaveBattleData()
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        battleTable.ChangeById(_battleId, _battleData);
        Cache.Save(battleTable);
        InitBattleData(_battleId);
    }

    public void ClearBattle()
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        battleTable.DeleteById(_battleId);
        Cache.Save(battleTable);
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
        _battleEngine.currentBattleSituation.NextTurn();
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

    public void InitScenario()
    {
        _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();

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
        ObstacleOnBattle[] scenarioObstacles = scenarioData.GetObstacles();
        Replic[] scenarioStartDialogue = scenarioData.GetStartDialogue();

        LandingData landingData = scenarioData.GetLanding();
        List<Vector2Int> landingCells = Bector2Int.MassiveToVector2Int(landingData.zone).ToList();
        int landingMaxStaff = landingData.maxStaff;

        Vector2Int mapSize = scenarioData.GetMapSize().ToVector2Int();
        string terrainPath = scenarioData.GetTerrainPath();
        Map map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        map.Init(mapSize, terrainPath);

        Scenario.Init(map, landingCells, landingMaxStaff, stages, stageIndex, scenarioStartDialogue, isLanded);
        
    }

    public void ReloadBattleData()
    {
        InitBattleData(_battleId);
    }

    public string GetBattleId()
    {
        return _battleId;
    }

    public IEnumerator BeforeScenarioBegin()
    {
        yield return StartCoroutine(Scenario.StartInitialDialogue());
        StartLanding();
    }

    public void StartLanding()
    {
        Scenario.StartLanding();
    }

    public void ContinueFight()
    {
        EventMaster.current.ContinueFight();
        EventMaster.current.OnBaseMode();
        _scenario.map.HideAll();
        _turnController.OnNextTurn(_battleData.GetTurn());
    }

    public void StartFight()
    {
        EventMaster.current.OnExitBuildMode();
        StartCoroutine(WaitForScenarioBegin());
    }

    public IEnumerator WaitForScenarioBegin()
    {
        EventMaster.current.OnBaseMode();
        _scenario.map.HideAll();

        SetLandedInBattleData();
        SaveBattleData();
        Scenario._isLanded = true;
        yield return StartCoroutine(Scenario.Begin());

        _turnController.OnNextTurn(_battleData.GetTurn());
    }

    public void OnScenarioUpdated()
    {
        Debug.Log("Сценарий обновлен");


        SyncBattleDataToCurrentBattleSituation();

        Debug.Log("Битва синхронизирована");

        EventMaster.current.OnNextTurn(_battleData.GetTurn());
        _turnController.OnNextTurn(_battleData.GetTurn());
    }

    public void NextTurn()
    {
        Debug.Log("Следующий ход");

        UpdateEffects();
        Debug.Log("Эффекты  обновлены");

        UpdateSkills();
        Debug.Log("Умения  обновлены");

        ChangeSideTurn();

        IncrementTurnIndex();

        Debug.Log("Данные по очередерности обновлены");

        StartCoroutine(Scenario.OnNextTurn());
    }

    public void ExecuteTurn(TurnData turnData)
    {
        StartCoroutine(WaitExecuteTurn(turnData));
    }

    public IEnumerator WaitExecuteTurn(TurnData turnData)
    {
        if (turnData != null)
        {
            bool isTurnContainsMovement = IsTurnContainsMovement(turnData);
            if (isTurnContainsMovement)
            {
                Debug.Log("Ход с передвижением");

                Unit activeUnit = _unitsOnFightManager.FindObjectByChildId(turnData._activeUnitIdOnBattle) as Unit;

                BattleEngine.OnReplaceUnit(_battleEngine.currentBattleSituation, activeUnit, turnData._route.Last());

                ChangeUnitOccypation(activeUnit, _scenario.map.Cells[turnData._route.Last().ToVector2()]);

                Debug.Log(activeUnit.name + "движется к позиции " + turnData._route.Last());

                activeUnit.Move(_battleEngine.GetCellsByBector2IntPositions(turnData._route));
                yield return new WaitUntil(() => !activeUnit._onMove);

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

                List<UnitOnBattle> attackersData = _battleEngine.currentBattleSituation.GetAttackersByTargetId(target.ChildId).ToArray().OfType<UnitOnBattle>().ToList();

                if (isTurnContainsMovement)
                {
                    UnitOnBattle activeUnit = _battleEngine.currentBattleSituation.GetUnitById(turnData._activeUnitIdOnBattle);
                    if (!attackersData.Contains(activeUnit))
                    {
                        attackersData.Add(activeUnit);
                    }
                }
                if (attackersData != null && attackersData.Count > 0)
                {

                    Debug.Log(attackersData.Count() + "атакующих");

                    List<Unit> attackers = _unitsOnFightManager.GetUnitsByBattleUnitsData(attackersData.ToArray());
                    int damage = BattleEngine.CalculateDamageToEntity(_battleEngine.currentBattleSituation, attackersData.ToArray(), target); // Нужно учесть модификаторы от скилов + эффекты

                    Debug.Log("Урон " + damage);

                    foreach (Unit attacker in attackers)
                    {
                        attacker.Attack(target);
                    }

                    // Подождать конца атаки и ранения цели = 1 с
                    Debug.Log("Цель сейчас получит");
                    BattleEngine.OnAttackTarget(_battleEngine.currentBattleSituation, target, damage);

                    target.GetDamage(damage);
                    yield return new WaitForSeconds(1);
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

    public void UpdateSkills()
    {
        foreach (Unit unit in _unitsOnFightManager.items.Values)
        {
            if (unit._skills == null) continue;

            SkillOnBattle[] skillDatas = _battleEngine.currentBattleSituation.GetUnitById(unit.ChildId).SkillsData;
            ObjectProcessor.ConfigureSkills(unit, skillDatas);
        }
    }

    public void ReturnToBase()
    {
        Debug.Log("Return to base!");
        SceneLoader.LoadHome();
    }

    public void Defeat()
    {
        DisableListeners();
        ClearBattle();
        Destroy(this.gameObject);
    }

    public void Victory()
    {
        DisableListeners();
        GiveRewardAndPassMission();
        ClearBattle();
        Destroy(this.gameObject);
    }

    public void GiveRewardAndPassMission()
    {
        string missionId = _battleData.GetMissionId();

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem missionCacheItem = missionTable.GetById(missionId);
        MissionCacheItem missionData = new MissionCacheItem(missionCacheItem.Fields);

        if (missionData.GetPassed() == false)
        {
            ResourcesData victoryGivesData = missionData.GetGives();
            _resourceProcessor.AddResources(victoryGivesData);
            _resourceProcessor.Save();

            missionData.SetPassed(true);
            missionTable.ChangeById(missionId, missionData);
            Cache.Save(missionTable);
        }
    }
}

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

    public bool _isNextTurnProcessing = false;
    public bool _isFightStarted = false;


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
        EventMaster.current.BattleObjectsChanged += ReloadBattleData;

        EventMaster.current.TurnExecuted += ExecuteTurn;
        EventMaster.current.CurrentStageChanged += ChangeCurrentStage;
        EventMaster.current.BeginStage += ChangeCurrentStage;
    }

    public void DisableListeners()
    {
        EventMaster.current.FightLost -= Defeat;
        EventMaster.current.FightWon -= Victory;
        EventMaster.current.BattleObjectsChanged -= ReloadBattleData;

        EventMaster.current.TurnExecuted -= ExecuteTurn;
        EventMaster.current.CurrentStageChanged -= ChangeCurrentStage;
        EventMaster.current.BeginStage -= ChangeCurrentStage;
    }

    public void EnableStartFightListener()
    {
        EventMaster.current.ToBattleButtonPressed += OnPressToBattleButton;
    }

    public void DisableStartFightListener()
    {
        EventMaster.current.ToBattleButtonPressed -= OnPressToBattleButton;
    }


    public void Start()
    {
        if (_battleData.GetCurrentStage() == null)
        {
            StartFight();
        }
        else
        {
            ContinueFight();
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

    public void ChangeCurrentStage(IStage stage)
    {
        Debug.Log("FightDirector: ChangeCurrentStage");

        // Сбрасываем фокус при обновлении сценария
        EventMaster.current.OnClearObjectFocus();

        // Восстанавливаем стандартные поведения компонентов
        EventMaster.current.OnResetComponentsBehaviour();

        StageData serializedStage = StageFactory.SerializeStage(stage);
        _battleData.SetCurrentStage(serializedStage);
        SaveBattleData();

        // Применяем новые поведения компонентов
        if (stage.BehaviourIdByComponentName != null)
        {
            foreach (var behaviour in stage.BehaviourIdByComponentName)
            {
                Debug.Log("[FightDirector]: component " + behaviour.Key + " - " +  behaviour.Value);

                EventMaster.current.OnChangeComponentBehaviour(behaviour.Key, behaviour.Value);
            }
        }

        // Устанавливаем фокус если есть
        if (stage.FocusData != null)
        {
            EventMaster.current.OnObjectFocused(stage.FocusData);
        }
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

        IStage currentStage;
        if (_battleData.GetCurrentStage() != null)
        {
            currentStage = StageFactory.GetAndInitStageByStageDataAndScenario(_battleData.GetCurrentStage(), Scenario);
        }
        else
        {
            currentStage = StageFactory.GetAndInitStageByStageDataAndScenario(scenarioData.GetStartStage(), Scenario);
        }

        UnitOnBattle[] scenarioUnits = scenarioData.GetUnits();
        BuildOnBattle[] scenarioBuilds = scenarioData.GetBuilds();
        ObstacleOnBattle[] scenarioObstacles = scenarioData.GetObstacles();
        Replic[] scenarioStartDialogue = scenarioData.GetStartDialogue();

        Vector2Int mapSize = scenarioData.GetMapSize().ToVector2Int();
        string terrainPath = scenarioData.GetTerrainPath();
        Map map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        map.Init(mapSize, terrainPath);

        Scenario.Init(map, currentStage, scenarioStartDialogue, this);
        
    }

    public void ReloadBattleData()
    {
        InitBattleData(_battleId);
    }

    public string GetBattleId()
    {
        return _battleId;
    }

    public IEnumerator BeginScenarioFromStart()
    {
        yield return StartCoroutine(Scenario.StartInitialDialogue());
        if (FightSceneLoader.parameters._autoStartFight)
        {
            OnPressToBattleButton();
        }
        else
        {
            EnableStartFightListener();
        }
    }

    public void StartFight()
    {
        StartCoroutine(BeginScenarioFromStart());
    }

    public void ContinueFight()
    {
        StartCoroutine(WaitForScenarioBegin());
    }

    public void OnPressToBattleButton()
    {
        DisableStartFightListener();
        EventMaster.current.ContinueFight();
        ContinueFight();

        _isFightStarted=true;
    }

    public IEnumerator WaitForScenarioBegin()
    {
        Debug.Log("[FightDirector]: WaitForScenarioBegin");

        EventMaster.current.OnBaseMode();
        _scenario.map.HideAll();

        yield return StartCoroutine(Scenario.Begin());

        Debug.Log("[FightDirector]: scenario begin completed, next turn");

        _isFightStarted = true;

        StartCoroutine(NextTurn());
    }

    public void OnScenarioUpdated()
    {
        Debug.Log("Сценарий обновлен");

        SyncBattleDataToCurrentBattleSituation();

        Debug.Log("Данные синхронизированы");

        EventMaster.current.OnNextTurn(_battleData.GetTurn());
        _turnController.OnNextTurn(_battleData.GetTurn());
    }

    public void Update()
    {
        if (_isNextTurnProcessing || !_isFightStarted || Scenario.IsStageProcessing())
        {
            return;
        }

        if ((Scenario.CurrentStage.IsRealTimeConditionForFail && Scenario.CurrentStage.IsFailed()) || (Scenario.CurrentStage.IsRealTimeConditionForPass && Scenario.CurrentStage.IsPassed()))
        {
            Debug.Log("Next turn by Update!");

            StartCoroutine(NextTurn());
        }
    }

    public IEnumerator NextTurn()
    {
        _isNextTurnProcessing = true;

        Debug.Log("Начинаем новый ход");

        UpdateEffects();

        UpdateSkills();

        ChangeSideTurn();

        IncrementTurnIndex();

        Debug.Log("Черед хода передан");

        yield return StartCoroutine(Scenario.OnNextTurn());
        OnScenarioUpdated();

        _isNextTurnProcessing = false;
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
                Debug.Log("Начинаем движение");

                Unit activeUnit = _unitsOnFightManager.FindObjectByChildId(turnData._activeUnitIdOnBattle) as Unit;

                BattleEngine.OnReplaceUnit(_battleEngine.currentBattleSituation, activeUnit, turnData._route.Last());

                ChangeUnitOccypation(activeUnit, _scenario.map.Cells[turnData._route.Last().ToVector2()]);

                Debug.Log(activeUnit.name + " перемещаемся на " + turnData._route.Last());

                activeUnit.Move(_battleEngine.GetCellsByBector2IntPositions(turnData._route));
                yield return new WaitUntil(() => !activeUnit._onMove);
            }

            if (IsTurnContainsTarget(turnData))
            {
                Entity target = _unitsOnFightManager.FindObjectByChildId(turnData._targetIdOnBattle);
                if (target == null)
                {
                    target = _buildsOnFightManager.FindObjectByChildId(turnData._targetIdOnBattle);
                }

                Debug.Log("Начинаем атаку на " + target.side + " "  + target.name);

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

                    Debug.Log(attackersData.Count() + " атакующих");

                    List<Unit> attackers = _unitsOnFightManager.GetUnitsByBattleUnitsData(attackersData.ToArray());
                    int damage = BattleEngine.CalculateDamageToEntity(_battleEngine.currentBattleSituation, attackersData.ToArray(), target); // ����� ������ ������������ �� ������ + �������

                    Debug.Log("Урон " + damage);

                    foreach (Unit attacker in attackers)
                    {
                        attacker.Attack(target);
                    }

                    BattleEngine.OnAttackTarget(_battleEngine.currentBattleSituation, target, damage);

                    target.GetDamage(damage);
                    yield return new WaitForSeconds(1);
                }
            }
        }

        yield return StartCoroutine(NextTurn());
    }

    public void ChangeUnitOccypation(Unit unit, Cell newOccypation)
    {
        Cell unitCurrentCell = _scenario.map.Cells[unit.center];
        unitCurrentCell.Free();

        unit.center = newOccypation.position;
        unit.SetOccypation(new List<Vector2Int>() { newOccypation.position });
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
        //������ ������������� �����
        //�������� _battleData
    }

    public void UpdateEffects()
    {
        // �������� ��� ������� �� ������(�������� �� �������) �������� �� � �������� ����������� ��� ����������� �������.
        // �������� ������ ������ �� ��������� ������� ���� ������� �����������
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

    public static void ReturnToBase()
    {
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

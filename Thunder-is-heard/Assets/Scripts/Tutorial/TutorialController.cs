using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour
{
    private static TutorialController _instance;
    public static TutorialController Instance => _instance;

    private TutorialCacheItem _activeTutorial;
    private ITutorialStage _currentStage;
    private ICondition _currentConditionForPass;

    private bool _waitingForUpdateStage = false;

    private string _tutorialMissionId = "60fc96d5-e769-4509-a5f1-597b844c7f25";

    public void Start()
    {
        // TODO ВЕРНУТЬ
        //if (!IsFightTutorialPassed())
        //{
        //    LoadTutorialMission();
        //    return;
        //}

        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        ActiveTutorialCacheItem activeTutorialData = GetActiveTutorial();
        if (activeTutorialData != null)
        {
            TutorialCacheTable tutorialsTable = Cache.LoadByType<TutorialCacheTable>();
            CacheItem tutorialCacheItem = tutorialsTable.GetById(activeTutorialData.GetTutorialId());
            TutorialCacheItem tutorialData = new TutorialCacheItem(tutorialCacheItem.Fields);
            LoadTutorial(tutorialData, activeTutorialData.GetStage());
            return;
        }
        TutorialCacheItem nextTutorialData = GetNextTutorial();
        if (nextTutorialData != null)
        {
            LoadTutorial(nextTutorialData);
            return;
        }
    }

    public void EnableListenerForUpdateStage()
    {
        EventMaster.current.StageUpdated += OnUpdateStage;
        _waitingForUpdateStage = true;
    }

    public void DisableListenerForUpdateStage()
    {
        EventMaster.current.StageUpdated -= OnUpdateStage;
        _waitingForUpdateStage = false;
    }

    private ActiveTutorialCacheItem GetActiveTutorial()
    {
        // Проверяем наличие активного обучения
        ActiveTutorialCacheTable activeTutorialsTable = Cache.LoadByType<ActiveTutorialCacheTable>();
        if (activeTutorialsTable.Items.Count > 0)
        {
            CacheItem activetutorialCacheItem = activeTutorialsTable.Items.First().Value;
            ActiveTutorialCacheItem activeTutorialData = new ActiveTutorialCacheItem(activetutorialCacheItem.Fields);
            return activeTutorialData;
        }
        return null;
    }

    private bool IsFightTutorialPassed()
    {
        MissionCacheTable missions = Cache.LoadByType<MissionCacheTable>();
        MissionCacheItem tutorialMission = new MissionCacheItem(missions.GetById(_tutorialMissionId).Fields);
        return tutorialMission.GetPassed();
    }

    private void LoadTutorialMission()
    {
        string startedTutorialMissionbattleId = MissionDetalization.FindBattleIdByMissionId(_tutorialMissionId);
        if (startedTutorialMissionbattleId != null)
        {
            SceneLoader.LoadFight(new FightSceneParameters(battleId: startedTutorialMissionbattleId, autoStartFight: true));
        }
        else
        {
            MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
            CacheItem cacheItemMission = missionTable.GetById(_tutorialMissionId);
            MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

            ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
            CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
            ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

            BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
            BattleCacheItem battleData = new BattleCacheItem(new Dictionary<string, object>());
            battleData.SetMissionId(_tutorialMissionId);
            battleData.SetUnits(scenarioData.GetUnits());
            battleData.SetBuilds(scenarioData.GetBuilds());
            battleData.SetObstacles(scenarioData.GetObstacles());
            battleTable.AddOne(battleData);
            Cache.Save(battleTable);

            SceneLoader.LoadFight(new FightSceneParameters(battleId: battleData.GetExternalId(), autoStartFight: true));
        }
    }

    private TutorialCacheItem GetNextTutorial()
    {
        // Проверяем доступные обучения
        var tutorials = Cache.LoadByType<TutorialCacheTable>().Items.Values;
        foreach (CacheItem tutorial in tutorials)
        {
            TutorialCacheItem tutorialData = new TutorialCacheItem(tutorial.Fields);
            if (IsTutorialCompleted(tutorialData))
                continue;

            if (IsComplyTutorialStartConditions(tutorialData))
            {
                return tutorialData;
            }
        }
        return null;
    }

    private bool IsTutorialCompleted(TutorialCacheItem tutorial)
    {
        return tutorial.GetPassed();
    }

    private bool IsComplyTutorialStartConditions(TutorialCacheItem tutorial)
    {
        if (tutorial.GetConditionForStart() == null)
            return true;
        ICondition condition = ConditionFactory.CreateCondition(tutorial.GetConditionForStart());
        return condition.IsComply();
    }

    private void LoadTutorial(TutorialCacheItem tutorial, TutorialStageData stage=null)
    {
        _activeTutorial = tutorial;
        if (stage == null)
        {
           StartCoroutine(SetStage(TutorialStageFactory.GetAndInitStageByStageData(tutorial.GetFirstStage())));
        }
        else
        {
            StartCoroutine(SetStage(TutorialStageFactory.GetAndInitStageByStageData(stage)));
        }
    }

    private IEnumerator SetStage(ITutorialStage stage)
    {
        _currentStage = stage;
        
        SaveTutorialProgress();

        // Замена поведений компонентов
        if (_currentStage.BehaviourIdByComponentName != null)
        {
            foreach (var behaviour in _currentStage.BehaviourIdByComponentName)
            {
                EventMaster.current.OnChangeComponentBehaviour(behaviour.Key, behaviour.Value);
            }
        }

        Debug.Log("Поведения компонентов этапа установлены");

        EnableListenerForUpdateStage();

        _currentStage.OnStart();
        yield return new WaitUntil(() => !_waitingForUpdateStage);

        Debug.Log("Стартовая логика этапа завершена");

        // Установка фокуса
        Debug.Log("Установка фокуса");


        if (_currentStage.FocusData != null)
        {
            EventMaster.current.OnObjectFocused(_currentStage.FocusData);
        }

        Debug.Log("Новое условие для прохождения - " + stage.ConditionsForPass.GetType().Name);

        _currentConditionForPass = stage.ConditionsForPass;
    }

    public void SaveTutorialProgress()
    {
        ActiveTutorialCacheTable activeTutorialsTable = Cache.LoadByType<ActiveTutorialCacheTable>();
        if (activeTutorialsTable.Items.Count > 0)
        {
            CacheItem activetutorialCacheItem = activeTutorialsTable.Items.First().Value;
            ActiveTutorialCacheItem activeTutorialData = new ActiveTutorialCacheItem(activetutorialCacheItem.Fields);
            activeTutorialData.SetStage(TutorialStageFactory.SerializeStage(_currentStage));
            activeTutorialsTable.ChangeById(activeTutorialData.GetExternalId(), activeTutorialData);
        }
        else
        {
            Dictionary<string, object> activeTutorialFields = new Dictionary<string, object>()
            {
                { "name", _activeTutorial.GetName() },
                { "tutorialId", _activeTutorial.GetExternalId() },
                { "stage", TutorialStageFactory.SerializeStage(_currentStage) }
            };

            ActiveTutorialCacheItem activeTutorialData = new ActiveTutorialCacheItem(activeTutorialFields);
            activeTutorialsTable.AddOne(activeTutorialData);
        }

        Cache.Save(activeTutorialsTable);
    }

    private void Update()
    {
        if (_currentStage == null || _currentConditionForPass == null)
            return;

        // Постоянная проверка условий для перехода к следующему этапу
        if (_currentConditionForPass.IsComply())
        {
            Debug.Log($"[Tutorial] Условие выполнено: {_currentConditionForPass.GetType().Name}");
            _currentConditionForPass = null;

            StartCoroutine(ProcessStageCompletion());
        }
    }

    public void OnUpdateStage()
    {
        DisableListenerForUpdateStage();
    }

    public void ToNextTutorial()
    {
        TutorialCacheItem tutorialData = GetNextTutorial();
        if (tutorialData != null)
        {
            LoadTutorial(tutorialData);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator ProcessStageCompletion()
    {
        EventMaster.current.OnClearObjectFocus();

        // Восстановление стандартных поведений компонентов
        EventMaster.current.OnResetComponentsBehaviour();

        EnableListenerForUpdateStage();
        _currentStage.OnPass();
        yield return new WaitUntil(() => !_waitingForUpdateStage);

        EnableListenerForUpdateStage();
        _currentStage.OnFinish();
        yield return new WaitUntil(() => !_waitingForUpdateStage);

        if (_currentStage.StageOnPass != null)
        {
            var nextStage = _currentStage.StageOnPass;
            yield return StartCoroutine(SetStage(nextStage));
        }
        else
        {
            CompleteTutorial();
            ToNextTutorial();
        }
    }

    private void CompleteTutorial()
    {
        // Сохранение прогресса
        ActiveTutorialCacheTable activeTutorialsTable = Cache.LoadByType<ActiveTutorialCacheTable>();
        activeTutorialsTable.DeleteAll();

        _activeTutorial.SetPassed(true);
        TutorialCacheTable tutorialsTable = Cache.LoadByType<TutorialCacheTable>();
        tutorialsTable.ChangeById(_activeTutorial.GetExternalId(), _activeTutorial);
        Cache.Save(tutorialsTable);
        Cache.Save(activeTutorialsTable);

        _activeTutorial = null;
        _currentStage = null;
        _currentConditionForPass = null;
    }
} 
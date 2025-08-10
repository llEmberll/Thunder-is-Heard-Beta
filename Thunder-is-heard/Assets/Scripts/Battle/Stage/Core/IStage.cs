using System.Collections.Generic;


public interface IStage
{
    public string StageId { get; }

    public Scenario Scenario { get; }
    public UnitOnBattleSpawnData[] UnitsForSpawn { get; }
    public BuildOnBattleSpawnData[] BuildsForSpawn { get; }

    public Replic[] ReplicsOnStart { get; }
    public Replic[] ReplicsOnPass { get; }
    public Replic[] ReplicsOnFail { get; }

    public List<IScenarioEvent> Events { get; }


    public AISettings[] AISettings { get; }


    public ICondition ConditionsForPass { get; }
    public ICondition ConditionsForFail { get; }

    public IStage StageOnPass { get; }
    public IStage StageOnFail { get; }

    public Dictionary<string, string> BehaviourIdByComponentName { get; }
    public FocusData FocusData { get; }

    public MediaEventData MediaEventData { get; }

    public ScenarioEventData[] ScenarioEvents { get; }

    public LandingData LandingData { get; }

    public string HintText { get; }

    public bool IsRealTimeConditionForPass { get; }
    public bool IsRealTimeConditionForFail { get; }

    public bool AutoPassTurn { get; }


    public void Init(
        string stageId,
        Scenario stageScenario,
        AISettings[] AISettings,
        ICondition conditionsForPass,
        ICondition conditionsForFail,
        UnitOnBattleSpawnData[] stageUnitsForSpawn,
        BuildOnBattleSpawnData[] stageBuildsForSpawn,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        Replic[] replicOnFail,
        IStage stageOnPass,
        IStage stageOnFail,
        Dictionary<string, string> behaviourIdByComponentName = null,
        FocusData focusData = null,
        MediaEventData stageMediaEventData = null,
        LandingData stageLandingData = null,
        string stageHintText = null,
        ScenarioEventData[] stageScenarioEvents = null,
        bool stageAutoPassTurn = false
        );
    public void SetScenario(Scenario value);
    public void SetConditionsForPass(ICondition conditions);
    public void SetConditionsForFail(ICondition conditions);
    public void SetObjectsForSpawn(UnitOnBattleSpawnData[] unitsForSpawn, BuildOnBattleSpawnData[] buildsForSpawn);
    public void SetReplics(Replic[] replicOnStart, Replic[] replicOnPass, Replic[] replicOnFail);

    public void SetCustomProperties();


    public void OnStart();
    public void OnProcess();
    public void OnFinish();

    public bool IsPassed();
    public bool IsFailed();

    public void OnPass();
    public void OnFail();
}
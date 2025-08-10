using System.Collections.Generic;


[System.Serializable]
public class StageData
{
    public string id;

    public UnitOnBattleSpawnData[] unitsForSpawn;
    public BuildOnBattleSpawnData[] buildsForSpawn;

    public Replic[] replicsOnStart;
    public Replic[] replicsOnPass;
    public Replic[] replicsOnFail;

    public AISettings[] AISettings;

    public ConditionData conditionsForFail;
    public ConditionData conditionsForPass;

    public StageData stageOnPass = null;
    public StageData stageOnFail = null;


    public Dictionary<string, string> behaviourIdByComponentName = null;

    public FocusData focusData = null;

    public MediaEventData mediaEventData = null;

    public LandingData landingData = null;

    public string hintText = null;

    public ScenarioEventData[] scenarioEvents = null;

    public bool autoPassTurn = false;


    public StageData() { }

    public StageData(
        UnitOnBattleSpawnData[] stageUnitsForSpawn,
        BuildOnBattleSpawnData[] stageBuildsForSpawn,
        Replic[] stageReplicsOnStart,
        Replic[] stageReplicsOnPass,
        Replic[] stageReplicsOnFail,
        AISettings[] stageAISettings,
        ConditionData stageConditionsForFail,
        ConditionData stageConditionsForPass,
        string stageId = "BasicStage",
        StageData stageStageOnPass = null,
        StageData stageStageOnFail = null,
        Dictionary<string, string> stageBehaviourIdByComponentName = null,
        FocusData stageFocusData = null,
        MediaEventData stageMediaEventData = null,
        LandingData stageLandingData = null,
        string stageHintText = null,
        ScenarioEventData[] stageScenarioEvents = null,
        bool stageAutoPassTurn = false
        )
    {
        id = stageId;
        unitsForSpawn = stageUnitsForSpawn;
        buildsForSpawn = stageBuildsForSpawn;
        replicsOnStart = stageReplicsOnStart;
        replicsOnPass = stageReplicsOnPass;
        replicsOnFail = stageReplicsOnFail;
        AISettings = stageAISettings;
        conditionsForFail = stageConditionsForFail;
        conditionsForPass = stageConditionsForPass;
        stageOnPass = stageStageOnPass;
        stageOnFail = stageStageOnFail;
        behaviourIdByComponentName = stageBehaviourIdByComponentName;
        focusData = stageFocusData;
        mediaEventData = stageMediaEventData;
        landingData = stageLandingData;
        hintText = stageHintText;
        scenarioEvents = stageScenarioEvents;
        autoPassTurn = stageAutoPassTurn;
    }
}

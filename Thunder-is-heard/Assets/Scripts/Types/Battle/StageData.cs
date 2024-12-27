

[System.Serializable]
public class StageData
{
    public string id;

    public UnitOnBattle[] units;
    public BuildOnBattle[] builds;
    
    public Replic[] replicsOnStart;
    public Replic[] replicsOnPass;
    public Replic[] replicsOnFail;

    public AISettings[] AISettings;

    public ConditionData conditionsForDefeat;
    public ConditionData conditionsForVictory;

    public StageData stageOnFail = null;

    
    public StageData() { }

    public StageData(
        UnitOnBattle[] stageUnits,
        BuildOnBattle[] stageBuilds,
        Replic[] stageReplicsOnStart,
        Replic[] stageReplicsOnPass,
        Replic[] stageReplicsOnFail,
        AISettings[] stageAISettings,
        ConditionData stageConditionsForDefeat, 
        ConditionData stageConditionsForVictory,
        string stageId = "BasicStage",
        StageData stageStageOnFail = null
        )
    {
        id = stageId;
        units = stageUnits;
        builds = stageBuilds;
        replicsOnStart = stageReplicsOnStart;
        replicsOnPass = stageReplicsOnPass;
        replicsOnFail = stageReplicsOnFail;
        AISettings = stageAISettings;
        conditionsForDefeat = stageConditionsForDefeat;
        conditionsForVictory = stageConditionsForVictory;
        stageOnFail = stageStageOnFail;
    }
}

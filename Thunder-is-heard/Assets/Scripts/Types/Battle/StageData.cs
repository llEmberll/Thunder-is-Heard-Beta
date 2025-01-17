

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

    public ConditionData conditionsForFail;
    public ConditionData conditionsForPass;

    public StageData stageOnPass = null;
    public StageData stageOnFail = null;

    
    public StageData() { }

    public StageData(
        UnitOnBattle[] stageUnits,
        BuildOnBattle[] stageBuilds,
        Replic[] stageReplicsOnStart,
        Replic[] stageReplicsOnPass,
        Replic[] stageReplicsOnFail,
        AISettings[] stageAISettings,
        ConditionData stageConditionsForFail,
        ConditionData stageConditionsForPass,
        string stageId = "BasicStage",
        StageData stageStageOnPass = null,
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
        conditionsForFail = stageConditionsForFail;
        conditionsForPass = stageConditionsForPass;
        stageOnPass = stageStageOnPass;
        stageOnFail = stageStageOnFail;
    }
}

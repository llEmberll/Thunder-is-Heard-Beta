

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

    public string[] conditionsForDefeat;
    public string[] conditionsForVictory;

    

    public StageData(
        UnitOnBattle[] stageUnits,
        BuildOnBattle[] stageBuilds,
        Replic[] stageReplicsOnStart,
        Replic[] stageReplicsOnPass,
        Replic[] stageReplicsOnFail,
        AISettings[] StageAISettings,
        string[] stageConditionsForDefeat, 
        string[] stageConditionsForVictory,
        string stageId = "BasicStage"
        )
    {
        id = stageId;
        units = stageUnits;
        builds = stageBuilds;
        replicsOnStart = stageReplicsOnStart;
        replicsOnPass = stageReplicsOnPass;
        replicsOnFail = stageReplicsOnFail;
        AISettings = StageAISettings;
        conditionsForDefeat = stageConditionsForDefeat;
        conditionsForVictory = stageConditionsForVictory;
    }
}

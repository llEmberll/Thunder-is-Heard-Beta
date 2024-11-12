

[System.Serializable]
public class StageData
{
    public string id;

    public UnitOnBattle[] units;
    public BuildOnBattle[] builds;

    public AISettings[] AISettings;

    public string[] conditionsForDefeat;
    public string[] conditionsForVictory;

    public StageData(
        UnitOnBattle[] stageUnits,
        BuildOnBattle[] stageBuilds,
        AISettings[] StageAISettings,
        string[] stageConditionsForDefeat, 
        string[] stageConditionsForVictory,
        string stageId = "BasicStage"
        )
    {
        id = stageId;
        units = stageUnits;
        builds = stageBuilds;
        AISettings = StageAISettings;
        conditionsForDefeat = stageConditionsForDefeat;
        conditionsForVictory = stageConditionsForVictory;
    }
}

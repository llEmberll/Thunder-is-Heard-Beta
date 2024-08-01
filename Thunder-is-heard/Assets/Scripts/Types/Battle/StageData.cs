

[System.Serializable]
public class StageData
{
    public string id;

    public UnitOnBattle[] units;
    public BuildOnBattle[] builds;
    public string[] conditionsForDefeat;
    public string[] conditionsForVictory;

    public StageData(
        UnitOnBattle[] stageUnits,
        BuildOnBattle[] stageBuilds,
        string[] stageConditionsForDefeat, 
        string[] stageConditionsForVictory,
        string stageId = "BasicStage"
        )
    {
        id = stageId;
        units = stageUnits;
        builds = stageBuilds;
        conditionsForDefeat = stageConditionsForDefeat;
        conditionsForVictory = stageConditionsForVictory;
    }
}

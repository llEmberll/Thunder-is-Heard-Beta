

[System.Serializable]
public class StageData
{
    public UnitOnBattle[] units;
    public BuildOnBattle[] builds;
    public string[] conditionsForDefeat;
    public string[] conditionsForVictory;

    public StageData(
        UnitOnBattle[] stageUnits,
        BuildOnBattle[] stageBuilds,
        string[] stageConditionsForDefeat, 
        string[] stageConditionsForVictory
        )
    {
        units = stageUnits;
        builds = stageBuilds;
        conditionsForDefeat = stageConditionsForDefeat;
        conditionsForVictory = stageConditionsForVictory;
    }
}

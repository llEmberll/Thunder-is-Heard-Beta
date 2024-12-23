using System;
using System.Collections.Generic;


public static class StageFactory
{
    public static Dictionary<string, Type> stages = new Dictionary<string, Type>()
    {
        { "BasicStage", typeof(BasicStage) }
    };

    public static IStage GetAndInitStageByStageDataAndScenario(StageData data, Scenario scenario)
    {
        if (!stages.ContainsKey(data.id)) return null;
        
        Type type = stages[data.id];
        IStage stage = (IStage)Activator.CreateInstance(type);
        List<ICondition> conditionsForPass = ConditionFactory.GetConditionsByIds(data.conditionsForVictory);
        List<ICondition> conditionsForFail = ConditionFactory.GetConditionsByIds(data.conditionsForDefeat);
        UnitOnBattle[] units = data.units;
        BuildOnBattle[] builds = data.builds;
        Replic[] replicsOnStart = data.replicsOnStart;
        Replic[] replicsOnPass = data.replicsOnPass;
        Replic[] replicsOnFail = data.replicsOnFail;

        stage.Init(scenario, data.AISettings, conditionsForPass, conditionsForFail, units, builds, replicsOnStart, replicsOnPass, replicsOnFail);
        return stage;
    }

    public static List<IStage> GetAndInitStagesByStageDatasAndScenario(StageData[] datas, Scenario scenario)
    {
        List<IStage> stages = new List<IStage>();
        foreach (StageData data in datas)
        {
            IStage stage = GetAndInitStageByStageDataAndScenario(data, scenario);
            if (stage != null)
            {
                stages.Add(stage);
            }
        }

        return stages;
    }
}

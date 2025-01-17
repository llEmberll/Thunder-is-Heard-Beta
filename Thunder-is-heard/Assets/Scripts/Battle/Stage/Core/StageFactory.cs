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
        if (data == null) return null;
        if (!stages.ContainsKey(data.id)) return null;
        
        Type type = stages[data.id];
        IStage stage = (IStage)Activator.CreateInstance(type);
        IStage stageOnPass = GetAndInitStageByStageDataAndScenario(data.stageOnPass, scenario);
        IStage stageOnFail = GetAndInitStageByStageDataAndScenario(data.stageOnFail, scenario);
        ICondition conditionsForPass = ConditionFactory.CreateCondition(data.conditionsForPass);
        ICondition conditionsForFail = ConditionFactory.CreateCondition(data.conditionsForFail);
        UnitOnBattle[] units = data.units;
        BuildOnBattle[] builds = data.builds;
        Replic[] replicsOnStart = data.replicsOnStart;
        Replic[] replicsOnPass = data.replicsOnPass;
        Replic[] replicsOnFail = data.replicsOnFail;

        stage.Init(data.id, scenario, data.AISettings, conditionsForPass, conditionsForFail, units, builds, replicsOnStart, replicsOnPass, replicsOnFail, stageOnPass, stageOnFail);
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

    public static StageData SerializeStage(IStage stage)
    {
        if (stage == null) return null;
        string stageId = stage.StageId;
        UnitOnBattle[] units = stage.Units;
        BuildOnBattle[] builds = stage.Builds;
        Replic[] replicsOnStart = stage.ReplicsOnStart;
        Replic[] replicsOnPass = stage.ReplicsOnPass;
        Replic[] replicsOnFail = stage.ReplicsOnFail;
        StageData stageOnPass = SerializeStage(stage.StageOnPass);
        StageData stageOnFail = SerializeStage(stage.StageOnFail);
        AISettings[] aISettings = stage.AISettings;
        ConditionData serializedConditionsForPass = ConditionFactory.SerializeCondition(stage.ConditionsForPass);
        ConditionData serializedConditionsForFail = ConditionFactory.SerializeCondition(stage.ConditionsForFail);

        return new StageData(
            stageId: stageId,
            stageUnits: units,
            stageBuilds: builds,
            stageReplicsOnStart: replicsOnStart,
            stageReplicsOnPass: replicsOnPass,
            stageReplicsOnFail: replicsOnFail,
            stageAISettings: aISettings,
            stageConditionsForPass: serializedConditionsForPass,
            stageConditionsForFail: serializedConditionsForFail,
            stageStageOnPass: stageOnPass,
            stageStageOnFail: stageOnFail
            );
    }
}

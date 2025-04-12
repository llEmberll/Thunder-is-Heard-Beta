using System;
using System.Collections.Generic;


public static class TutorialStageFactory
{
    public static Dictionary<string, Type> stages = new Dictionary<string, Type>()
    {
        { "BasicTutorialStage", typeof(BasicTutorialStage) }
    };

    public static ITutorialStage GetAndInitStageByStageData(TutorialStageData data)
    {
        if (data == null) return null;
        if (!stages.ContainsKey(data.id)) return null;
        
        Type type = stages[data.id];
        ITutorialStage stage = (ITutorialStage)Activator.CreateInstance(type);
        ITutorialStage stageOnPass = GetAndInitStageByStageData(data.stageOnPass);
        ICondition conditionsForPass = ConditionFactory.CreateCondition(data.conditionsForPass);
        Replic[] replicsOnStart = data.replicsOnStart;
        Replic[] replicsOnPass = data.replicsOnPass;

        stage.Init(
            data.id, 
            conditionsForPass, 
            data.behaviourIdByComponentName,
            data.focusData,
            replicsOnStart, 
            replicsOnPass, 
            stageOnPass
            );
        return stage;
    }

    public static List<ITutorialStage> GetAndInitStagesByStageDatas(TutorialStageData[] datas)
    {
        List<ITutorialStage> stages = new List<ITutorialStage>();
        foreach (TutorialStageData data in datas)
        {
            ITutorialStage stage = GetAndInitStageByStageData(data);
            if (stage != null)
            {
                stages.Add(stage);
            }
        }

        return stages;
    }

    public static TutorialStageData SerializeStage(ITutorialStage stage)
    {
        if (stage == null) return null;
        string stageId = stage.StageId;
        Replic[] replicsOnStart = stage.ReplicsOnStart;
        Replic[] replicsOnPass = stage.ReplicsOnPass;
        TutorialStageData stageOnPass = SerializeStage(stage.StageOnPass);

        Dictionary<string, string> behaviourIdByComponentName = stage.BehaviourIdByComponentName;
        FocusData focusData = stage.FocusData;

        ConditionData serializedConditionsForPass = ConditionFactory.SerializeCondition(stage.ConditionsForPass);

        return new TutorialStageData(
            stageId: stageId,
            stageReplicsOnStart: replicsOnStart,
            stageReplicsOnPass: replicsOnPass,
            stageConditionsForPass: serializedConditionsForPass,
            stageStageOnPass: stageOnPass,
            stageBehaviourIdByComponentName: behaviourIdByComponentName,
            stageFocusData: focusData
            );
    }
}

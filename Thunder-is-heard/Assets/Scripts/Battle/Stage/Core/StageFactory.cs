using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

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
        UnitOnBattleSpawnData[] unitsForSpawn = data.unitsForSpawn;
        BuildOnBattleSpawnData[] buildsForSpawn = data.buildsForSpawn;
        Replic[] replicsOnStart = data.replicsOnStart;
        Replic[] replicsOnPass = data.replicsOnPass;
        Replic[] replicsOnFail = data.replicsOnFail;
        Dictionary<string, string> behaviourIdByComponentName = data.behaviourIdByComponentName;
        FocusData focusData = data.focusData;
        MediaEventData mediaEventData = data.mediaEventData;
        LandingData landingData = data.landingData;
        string hintText = data.hintText;
        ScenarioEventData[] scenarioEvents = data.scenarioEvents;

        stage.Init(
            data.id,
            scenario,
            data.AISettings,
            conditionsForPass,
            conditionsForFail,
            unitsForSpawn,
            buildsForSpawn,
            replicsOnStart,
            replicsOnPass, 
            replicsOnFail, 
            stageOnPass, 
            stageOnFail, 
            behaviourIdByComponentName,
            focusData,
            mediaEventData,
            landingData,
            hintText,
            scenarioEvents,
            data.autoPassTurn
            );
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
        UnitOnBattleSpawnData[] unitsForSpawn = stage.UnitsForSpawn;
        BuildOnBattleSpawnData[] buildsForSpawn = stage.BuildsForSpawn;
        Replic[] replicsOnStart = stage.ReplicsOnStart;
        Replic[] replicsOnPass = stage.ReplicsOnPass;
        Replic[] replicsOnFail = stage.ReplicsOnFail;
        StageData stageOnPass = SerializeStage(stage.StageOnPass);
        StageData stageOnFail = SerializeStage(stage.StageOnFail);
        AISettings[] aISettings = stage.AISettings;
        ConditionData serializedConditionsForPass = ConditionFactory.SerializeCondition(stage.ConditionsForPass);
        ConditionData serializedConditionsForFail = ConditionFactory.SerializeCondition(stage.ConditionsForFail);
        Dictionary<string, string> behaviourIdByCompoentName = stage.BehaviourIdByComponentName;
        FocusData focusData = stage.FocusData;
        MediaEventData mediaEventData = stage.MediaEventData;
        LandingData landingData = stage.LandingData;
        string hintText = stage.HintText;
        ScenarioEventData[] scenarioEvents = stage.ScenarioEvents;
        bool autoPassTurn = stage.AutoPassTurn;

        return new StageData(
            stageUnitsForSpawn: unitsForSpawn,
            stageBuildsForSpawn: buildsForSpawn,
            stageReplicsOnStart: replicsOnStart,
            stageReplicsOnPass: replicsOnPass,
            stageReplicsOnFail: replicsOnFail,
            stageAISettings: aISettings,
            stageConditionsForPass: serializedConditionsForPass,
            stageConditionsForFail: serializedConditionsForFail,
            stageId: stageId,
            stageStageOnPass: stageOnPass,
            stageStageOnFail: stageOnFail,
            stageBehaviourIdByComponentName: behaviourIdByCompoentName,
            stageFocusData: focusData,
            stageMediaEventData: mediaEventData,
            stageLandingData: landingData,
            stageHintText: hintText,
            stageScenarioEvents: scenarioEvents,
            stageAutoPassTurn: autoPassTurn
            );
    }
}

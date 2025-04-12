using System.Collections.Generic;


public class TutorialStageData
{
    public string id;


    public Replic[] replicsOnStart;
    public Replic[] replicsOnPass;


    public ConditionData conditionsForPass;

    public TutorialStageData stageOnPass = null;


    public Dictionary<string, string> behaviourIdByComponentName;

    public FocusData focusData;


    public TutorialStageData() { }

    public TutorialStageData(
        Replic[] stageReplicsOnStart,
        Replic[] stageReplicsOnPass,
        ConditionData stageConditionsForPass,
        Dictionary<string, string> stageBehaviourIdByComponentName,
        FocusData stageFocusData,
        string stageId = "BasicTutorialStage",
        TutorialStageData stageStageOnPass = null
        )
    {
        id = stageId;
        behaviourIdByComponentName = stageBehaviourIdByComponentName;
        focusData = stageFocusData;
        replicsOnStart = stageReplicsOnStart;
        replicsOnPass = stageReplicsOnPass;
        conditionsForPass = stageConditionsForPass;
        stageOnPass = stageStageOnPass;
    }
}

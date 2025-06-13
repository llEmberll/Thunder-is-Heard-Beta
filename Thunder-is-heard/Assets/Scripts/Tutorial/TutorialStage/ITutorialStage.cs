using System.Collections.Generic;

public interface ITutorialStage
{
    public string StageId { get; }

    public Replic[] ReplicsOnStart { get; }
    public Replic[] ReplicsOnPass { get; }


    public ICondition ConditionsForPass { get; }

    public ITutorialStage StageOnPass { get; }

    public Dictionary<string, string> BehaviourIdByComponentName { get; }
    public FocusData FocusData { get; }

    public MediaEventData MediaEventData { get; }


    public void Init(
        string stageId,
        ICondition conditionsForPass,
        Dictionary<string, string> behaviourIdByComponentName,
        FocusData focusData,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        ITutorialStage stageOnPass = null,
        MediaEventData stageMediaEventData = null
        );
    public void SetConditionsForPass(ICondition conditions);
    public void SetReplics(Replic[] replicOnStart, Replic[] replicOnPass, Replic[] replicOnFail);

    public void SetCustomProperties();


    public void OnStart();
    public void OnProcess();
    public void OnFinish();

    public bool IsPassed();

    public void OnPass();
}
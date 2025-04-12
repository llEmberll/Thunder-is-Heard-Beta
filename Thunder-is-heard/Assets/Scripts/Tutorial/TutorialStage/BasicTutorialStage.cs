using System.Collections.Generic;
using UnityEngine;


public class BasicTutorialStage: ITutorialStage
{
    public string _stageId;
    public string StageId { get { return _stageId; } }

    public Replic[] _replicsOnStart;
    public Replic[] _replicsOnPass;

    public ICondition _conditionsForPass;

    public Replic[] ReplicsOnStart { get { return _replicsOnStart; } }
    public Replic[] ReplicsOnPass { get { return _replicsOnPass; } }



    public ICondition ConditionsForPass { get { return _conditionsForPass; } }


    public ITutorialStage _stageOnPass = null;
    public ITutorialStage StageOnPass { get { return _stageOnPass; } }


    public Dictionary<string, string> _behaviourIdByComponentName;
    public Dictionary<string, string> BehaviourIdByComponentName { get { return _behaviourIdByComponentName; } }


    public FocusData _focusData;
    public FocusData FocusData { get { return _focusData; } }



    public ObjectProcessor _objectProcessor;
    public DialogueController _dialogueController;

    public bool isDialogue = false;
    public int replicIndex;


    public virtual void Init(
        string stageId,
        ICondition conditionsForPass,
        Dictionary<string, string> behaviourIdByComponentName,
        FocusData focusData,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        ITutorialStage stageOnPass = null
        )
    {
        _stageId = stageId;
        SetConditionsForPass(conditionsForPass);

        _behaviourIdByComponentName = behaviourIdByComponentName;
        _focusData = focusData;

        _stageOnPass = stageOnPass;
        SetCustomProperties();

        InitObjectProcessor();
        InitDialogueController();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void InitDialogueController()
    {
        _dialogueController = GameObject.FindGameObjectWithTag(Tags.dialogueController).GetComponent<DialogueController>();
    }

    public void EnableListeners()
    {

    }

    public void DisableListeners()
    {

    }

    public void EnableEndDialogueListener()
    {
        EventMaster.current.DialogueEnd += OnEndDialogue;
    }

    public void DisableEndDialogueListener()
    {
        EventMaster.current.DialogueEnd -= OnEndDialogue;
    }

    public virtual void SetConditionsForPass(ICondition conditions)
    {
        _conditionsForPass = conditions;
    }

    public virtual void SetReplics(Replic[] replicsOnStart, Replic[] replicsOnEnd, Replic[] replicsOnFail)
    {
        _replicsOnStart = replicsOnStart;
        _replicsOnPass = replicsOnEnd;
    }

    public virtual void SetCustomProperties()
    {

    }

    public void OnStart()
    {
        if (ReplicsOnStart != null && ReplicsOnStart.Length > 0)
        {
            BeginDialogue(ReplicsOnStart);
        }
        else
        {
            EventMaster.current.OnUpdateStage();
        }
    }

    public void BeginDialogue(Replic[] replics)
    {
        isDialogue = true;
        EventMaster.current.BeginDialogue(replics);
        EnableEndDialogueListener();
    }

    public void OnEndDialogue()
    {
        if (isDialogue)
        {
            isDialogue = false;
            DisableEndDialogueListener();
            EventMaster.current.OnUpdateStage();
        }
    }


    public void OnProcess()
    {
        EventMaster.current.OnUpdateStage();
    }

    public void OnFinish()
    {
        EventMaster.current.OnUpdateStage();

    }

    public bool IsPassed()
    {
        return IsAllConditionsForPassComply();
    }


    public void OnPass()
    {
        if (ReplicsOnPass != null && ReplicsOnPass.Length > 0)
        {
            BeginDialogue(ReplicsOnPass);
        }
        else
        {
            EventMaster.current.OnUpdateStage();
        }
    }

    public bool IsAllConditionsForPassComply()
    {
        return ConditionsForPass.IsComply();
    }
}
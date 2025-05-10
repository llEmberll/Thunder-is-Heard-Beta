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


    public bool isMediaEvent = false;

    public MediaEventData _mediaEventData = null;
    public MediaEventData MediaEventData { get { return _mediaEventData; } }

    private bool _isStartSequenceComplete = false;
    private Queue<System.Action> _startSequenceActions;

    public virtual void Init(
        string stageId,
        ICondition conditionsForPass,
        Dictionary<string, string> behaviourIdByComponentName,
        FocusData focusData,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        ITutorialStage stageOnPass = null,
        MediaEventData stageMediaEventData = null
        )
    {
        _stageId = stageId;
        SetConditionsForPass(conditionsForPass);

        _replicsOnStart = replicsOnStart;
        _replicsOnPass = replicsOnPass;

        _behaviourIdByComponentName = behaviourIdByComponentName;
        _focusData = focusData;

        _stageOnPass = stageOnPass;
        SetCustomProperties();

        _mediaEventData = stageMediaEventData;

        InitObjectProcessor();
        InitDialogueController();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void InitDialogueController()
    {
        // Ищем все экземпляры, включая неактивные
        var allDialogueControllers = Resources.FindObjectsOfTypeAll<DialogueController>();
        foreach (var controller in allDialogueControllers)
        {
            // Проверяем, что объект не является prefab-asset, а именно находится на сцене
            if (controller.gameObject.scene.IsValid())
            {
                _dialogueController = controller;
                return;
            }
        }
        throw new System.Exception("DialogueController не найден на сцене (включая неактивные объекты)!");
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

    public void EnableEndMediaEventListener()
    {
        EventMaster.current.MediaEventEnd += OnEndMediaEvent;
    }

    public void DisableEndMediaEventListener()
    {
        EventMaster.current.MediaEventEnd -= OnEndMediaEvent;
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
        _startSequenceActions = new Queue<System.Action>();
    }

    public void OnStart()
    {
        PrepareStartSequence();
        ProcessNextStartAction();
    }

    protected virtual void PrepareStartSequence()
    {
        Debug.Log("Prepare start sequence...");

        _startSequenceActions.Clear();
        _isStartSequenceComplete = false;

        if (_mediaEventData != null)
        {
            _startSequenceActions.Enqueue(() => BeginMediaEvent(_mediaEventData));
        }

        Debug.Log("Media event prepared");

        if (ReplicsOnStart != null && ReplicsOnStart.Length > 0)
        {
            _startSequenceActions.Enqueue(() => BeginDialogue(ReplicsOnStart));
        }

        Debug.Log("Dialogue prepared");
    }

    protected void ProcessNextStartAction()
    {
        Debug.Log("Next action!");
        if (_startSequenceActions.Count > 0)
        {
            Debug.Log("actions exists");

            var nextAction = _startSequenceActions.Dequeue();

            Debug.Log("next action: " + nextAction);


            nextAction.Invoke();

            Debug.Log("invoked");
        }
        else
        {
            CompleteStartSequence();
        }
    }

    protected void CompleteStartSequence()
    {
        Debug.Log("Sequence completed");

        _isStartSequenceComplete = true;
        EventMaster.current.OnUpdateStage();
    }

    public void BeginMediaEvent(MediaEventData eventData)
    {
        isMediaEvent = true;
        EventMaster.current.BeginMediaEvent(eventData);
        EnableEndMediaEventListener();
    }

    public void BeginDialogue(Replic[] replics)
    {
        Debug.Log("Begin dialogue");

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
            ProcessNextStartAction();
        }
    }

    public void OnEndMediaEvent()
    {
        if (isMediaEvent)
        {
            isMediaEvent = false;
            DisableEndMediaEventListener();
            ProcessNextStartAction();
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
using System.Collections.Generic;
using UnityEngine;


public class BasicStage: IStage
{
    public string _stageId;
    public string StageId { get { return _stageId; } }

    public Scenario _scenario;
    public UnitOnBattleSpawnData[] _unitsForSpawn;
    public BuildOnBattleSpawnData[] _buildsForSpawn;

    public Replic[] _replicsOnStart;
    public Replic[] _replicsOnPass;
    public Replic[] _replicsOnFail;

    public List<IScenarioEvent> events;
    public ICondition _conditionsForPass;
    public ICondition _conditionsForFail;

    public UnitOnBattleSpawnData[] UnitsForSpawn {  get { return _unitsForSpawn; } }
    public BuildOnBattleSpawnData[] BuildsForSpawn { get { return _buildsForSpawn; } }

    public Replic[] ReplicsOnStart { get { return _replicsOnStart; } }
    public Replic[] ReplicsOnPass { get { return _replicsOnPass; } }
    public Replic[] ReplicsOnFail { get { return _replicsOnFail; } }

    public List<IScenarioEvent> Events { get { return events; } }


    public AISettings[] _AISettings;
    public AISettings[] AISettings { get { return _AISettings; } }


    public ICondition ConditionsForPass { get { return _conditionsForPass; } }

    public ICondition ConditionsForFail { get { return _conditionsForFail; } }


    public IStage _stageOnPass = null;
    public IStage StageOnPass { get { return _stageOnPass; } }

    public IStage _stageOnFail = null;
    public IStage StageOnFail { get { return _stageOnFail; } }

    public Scenario Scenario { get { return _scenario; } }


    public ObjectProcessor _objectProcessor;
    public DialogueController _dialogueController;

    public bool isDialogue = false;
    public int replicIndex;


    public Dictionary<string, string> _behaviourIdByComponentName;
    public Dictionary<string, string> BehaviourIdByComponentName { get { return _behaviourIdByComponentName; } }


    public FocusData _focusData;
    public FocusData FocusData { get { return _focusData; } }


    public bool isMediaEvent = false;
    public MediaEventData _mediaEventData = null;
    public MediaEventData MediaEventData { get { return _mediaEventData; } }


    public bool isLanding = false;
    public LandingData _landingData = null;
    public LandingData LandingData { get { return  _landingData; } }

    public string _hintText;
    public string HintText { get { return _hintText; } }

    public bool isScenarioEvents = false;
    public ScenarioEventData[] _scenarioEvents;
    public ScenarioEventData[] ScenarioEvents { get { return _scenarioEvents; } }


    private bool _isStartSequenceComplete = false;
    private Queue<System.Action> _startSequenceActions;

    public virtual void Init(
        string stageId,
        Scenario stageScenario,
        AISettings[] AISettings,
        ICondition conditionsForPass,
        ICondition conditionsForFail,
        UnitOnBattleSpawnData[] stageUnitsForSpawn,
        BuildOnBattleSpawnData[] stageBuildsForSpawn,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        Replic[] replicsOnFail,
        IStage stageOnPass = null,
        IStage stageOnFail = null,
        Dictionary<string, string> behaviourIdByComponentName = null,
        FocusData focusData = null,
        MediaEventData stageMediaEventData = null,
        LandingData stageLandingData = null,
        string stageHintText = null,
        ScenarioEventData[] stageScenarioEvents = null
        )
    {
        _stageId = stageId;
        SetScenario(stageScenario);
        SetAISettings(AISettings);
        SetConditionsForPass(conditionsForPass);
        SetConditionsForFail(conditionsForFail);
        SetObjectsForSpawn(stageUnitsForSpawn, stageBuildsForSpawn);
        SetReplics(replicsOnStart, replicsOnPass, replicsOnFail);
        _stageOnPass = stageOnPass;
        _stageOnFail = stageOnFail;
        _behaviourIdByComponentName = behaviourIdByComponentName;
        _focusData = focusData;
        _mediaEventData = stageMediaEventData;

        _landingData = stageLandingData;

        _hintText = stageHintText;
        _scenarioEvents = stageScenarioEvents;

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

    public void EnableEndMediaEventListener()
    {
        EventMaster.current.MediaEventEnd += OnEndMediaEvent;
    }

    public void DisableEndMediaEventListener()
    {
        EventMaster.current.MediaEventEnd -= OnEndMediaEvent;
    }

    public void EnableEndLandingListener()
    {
        EventMaster.current.FightIsContinued += OnEndLanding;
    }

    public void DisableEndLandingListener()
    {
        EventMaster.current.FightIsContinued -= OnEndLanding;
    }

    public void EnableEndScenarioEventsListener()
    {
        EventMaster.current.ScenarioEventsEnd += OnEndScenarioEvents;
    }

    public void DisableEndScenarioEventsListener()
    {
        EventMaster.current.ScenarioEventsEnd -= OnEndScenarioEvents;
    }

    public void OnEndScenarioEvents()
    {
        Debug.Log($"[BasicStage] OnEndScenarioEvents for stage: {_stageId}, isScenarioEvents: {isScenarioEvents}");
        if (isScenarioEvents)
        {
            isScenarioEvents = false;
            DisableEndScenarioEventsListener();
            ProcessNextStartAction();
        }
    }

    public void SetScenario(Scenario value)
    {
        _scenario = value;
    }

    public void SetAISettings(AISettings[] value)
    {
        _AISettings = value;
    }

    public virtual void SetConditionsForPass(ICondition conditions)
    {
        _conditionsForPass = conditions;
    }

    public virtual void SetConditionsForFail(ICondition conditions)
    {
        _conditionsForFail = conditions;
    }

    public virtual void SetObjectsForSpawn(UnitOnBattleSpawnData[] unitsForSpawn, BuildOnBattleSpawnData[] buildsForSpawn)
    {
        _unitsForSpawn = unitsForSpawn;
        _buildsForSpawn = buildsForSpawn;
    }

    public virtual void SetReplics(Replic[] replicsOnStart, Replic[] replicsOnEnd, Replic[] replicsOnFail)
    {
        _replicsOnStart = replicsOnStart;
        _replicsOnPass = replicsOnEnd;
        _replicsOnFail = replicsOnFail;
    }

    public void CreateObjects()
    {
        _objectProcessor.CreateObjectsOnBattleFromSpawnData(UnitsForSpawn, BuildsForSpawn);
    }

    public virtual void SetCustomProperties()
    {
        _startSequenceActions = new Queue<System.Action>();
    }

    public void OnStart()
    {
        Debug.Log($"[BasicStage] OnStart called for stage: {_stageId}");
        CreateObjects();
        Debug.Log($"[BasicStage] Objects created for stage: {_stageId}");
        PrepareStartSequence();
        Debug.Log($"[BasicStage] Start sequence prepared for stage: {_stageId}");
        ProcessNextStartAction();
    }

    protected virtual void PrepareStartSequence()
    {
        Debug.Log($"[BasicStage] PrepareStartSequence for stage: {_stageId}");
        _startSequenceActions.Clear();
        _isStartSequenceComplete = false;

        if (_mediaEventData != null)
        {
            Debug.Log($"[BasicStage] Adding MediaEvent to sequence for stage: {_stageId}");
            _startSequenceActions.Enqueue(() => BeginMediaEvent(_mediaEventData));
        }
        
        if (_scenarioEvents != null && _scenarioEvents.Length > 0)
        {
            Debug.Log($"[BasicStage] Adding ScenarioEvents to sequence for stage: {_stageId} (count: {_scenarioEvents.Length})");
            _startSequenceActions.Enqueue(() => BeginScenarioEvents(_scenarioEvents));
        }

        if (ReplicsOnStart != null && ReplicsOnStart.Length > 0)
        {
            Debug.Log($"[BasicStage] Adding StartDialogue to sequence for stage: {_stageId} (count: {ReplicsOnStart.Length})");
            _startSequenceActions.Enqueue(() => BeginDialogue(ReplicsOnStart));
        }

        if (_hintText != null)
        {
            Debug.Log($"[BasicStage] Adding Hint to sequence for stage: {_stageId}");
            _startSequenceActions.Enqueue(() => SetHint(_hintText));
        }

        if (LandingData != null)
        {
            Debug.Log($"[BasicStage] Adding Landing to sequence for stage: {_stageId}");
            _startSequenceActions.Enqueue(() => BeginLanding(LandingData));
        }
        
        Debug.Log($"[BasicStage] Start sequence prepared with {_startSequenceActions.Count} actions for stage: {_stageId}");
    }

    protected void ProcessNextStartAction()
    {
        Debug.Log($"[BasicStage] ProcessNextStartAction for stage: {_stageId}, remaining actions: {_startSequenceActions.Count}");
        if (_startSequenceActions.Count > 0)
        {
            var nextAction = _startSequenceActions.Dequeue();
            Debug.Log($"[BasicStage] Executing next action for stage: {_stageId}");
            nextAction.Invoke();
        }
        else
        {
            Debug.Log($"[BasicStage] No more actions, completing start sequence for stage: {_stageId}");
            CompleteStartSequence();
        }
    }

    protected void CompleteStartSequence()
    {
        Debug.Log($"[BasicStage] CompleteStartSequence for stage: {_stageId}");
        _isStartSequenceComplete = true;
        EventMaster.current.OnUpdateStage();
    }

    public void BeginMediaEvent(MediaEventData eventData)
    {
        Debug.Log($"[BasicStage] BeginMediaEvent for stage: {_stageId}");
        isMediaEvent = true;
        EventMaster.current.BeginMediaEvent(eventData);
        EnableEndMediaEventListener();
    }

    public void BeginDialogue(Replic[] replics)
    {
        Debug.Log($"[BasicStage] BeginDialogue for stage: {_stageId} (replics count: {replics.Length})");
        isDialogue = true;
        EventMaster.current.BeginDialogue(replics);
        EnableEndDialogueListener();
    }

    public void BeginLanding(LandingData landingData)
    {
        Debug.Log($"[BasicStage] BeginLanding for stage: {_stageId}");
        isLanding = true;
        EventMaster.current.Landing(landingData);
        EnableEndLandingListener();
    }

    public void BeginScenarioEvents(ScenarioEventData[] events)
    {
        Debug.Log($"[BasicStage] BeginScenarioEvents for stage: {_stageId} (events count: {events.Length})");
        isScenarioEvents = true;
        EventMaster.current.BeginScenarioEvents(events);
        EnableEndScenarioEventsListener();
    }

    public void SetHint(string text)
    {
        Debug.Log($"[BasicStage] SetHint for stage: {_stageId}: {text}");
        EventMaster.current.OnSetHint(text);
        ProcessNextStartAction();
    }

    public void HideHint()
    {
        EventMaster.current.OnHideHint();
    }

    public void OnEndMediaEvent()
    {
        Debug.Log($"[BasicStage] OnEndMediaEvent for stage: {_stageId}, isMediaEvent: {isMediaEvent}");
        if (isMediaEvent)
        {
            isMediaEvent = false;
            DisableEndMediaEventListener();
            ProcessNextStartAction();
        }
    }

    public void OnEndDialogue()
    {
        Debug.Log($"[BasicStage] OnEndDialogue for stage: {_stageId}, isDialogue: {isDialogue}");
        if (isDialogue)
        {
            isDialogue = false;
            DisableEndDialogueListener();
            ProcessNextStartAction();
        }
    }

    public void OnEndLanding()
    {
        Debug.Log($"[BasicStage] OnEndLanding for stage: {_stageId}, isLanding: {isLanding}");
        if (isLanding)
        {
            isLanding = false;
            DisableEndLandingListener();
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
        bool passed = IsAllConditionsForPassComply();


        return passed;

    }

    public bool IsFailed()
    {
        bool failed = IsAllConditionsForFailComply();
        return failed;
    }

    public void OnPass()
    {
        Debug.Log($"[BasicStage] OnPass called for stage: {_stageId}");
        HideHint();
        if (ReplicsOnPass != null && ReplicsOnPass.Length > 0)
        {
            Debug.Log($"[BasicStage] Starting pass dialogue for stage: {_stageId}");
            BeginDialogue(ReplicsOnPass);
        }
        else
        {
            Debug.Log($"[BasicStage] No pass dialogue, updating stage: {_stageId}");
            EventMaster.current.OnUpdateStage();
        }
    }

    public void OnFail()
    {
        Debug.Log($"[BasicStage] OnFail called for stage: {_stageId}");
        HideHint();
        if (ReplicsOnFail != null && ReplicsOnFail.Length > 0)
        {
            Debug.Log($"[BasicStage] Starting fail dialogue for stage: {_stageId}");
            BeginDialogue(ReplicsOnFail);
        }
        else
        {
            Debug.Log($"[BasicStage] No fail dialogue, updating stage: {_stageId}");
            EventMaster.current.OnUpdateStage();
        }
    }

    public bool IsAllConditionsForPassComply()
    {
        return ConditionsForPass.IsComply();
    }

    public bool IsAllConditionsForFailComply()
    {
        return ConditionsForFail.IsComply();
    }
}
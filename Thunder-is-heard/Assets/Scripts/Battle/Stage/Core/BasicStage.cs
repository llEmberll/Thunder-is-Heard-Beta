using System.Collections.Generic;
using UnityEngine;


public class BasicStage: IStage
{
    public Scenario _scenario;
    public UnitOnBattle[] _units;
    public BuildOnBattle[] _builds;

    public Replic[] _replicsOnStart;
    public Replic[] _replicsOnPass;
    public Replic[] _replicsOnFail;

    public List<IScenarioEvent> events;
    public ICondition _conditionsForPass;
    public ICondition _conditionsForFail;

    public UnitOnBattle[] Units { get { return _units; } }
    public BuildOnBattle[] Builds { get { return _builds; } }

    public Replic[] ReplicsOnStart { get { return _replicsOnStart; } }
    public Replic[] ReplicsOnPass { get { return _replicsOnPass; } }
    public Replic[] ReplicsOnFail { get { return _replicsOnFail; } }

    public List<IScenarioEvent> Events { get { return events; } }


    public Dictionary<string, AISettings> _AISettingsBySide;
    public Dictionary<string, AISettings> AISettingsBySide { get { return _AISettingsBySide; } }


    public ICondition ConditionsForPass { get { return _conditionsForPass; } }

    public ICondition ConditionsForFail { get { return _conditionsForFail; } }

    public IStage _stageOnFail = null;
    public IStage StageOnFail { get { return _stageOnFail; } }

    public Scenario Scenario { get { return _scenario; } }


    public ObjectProcessor _objectProcessor;
    public DialogueController _dialogueController;

    public bool isDialogue = false;
    public int replicIndex;


    public virtual void Init(
        Scenario stageScenario,
        AISettings[] AISettings,
        ICondition conditionsForPass,
        ICondition conditionsForFail,
        UnitOnBattle[] units,
        BuildOnBattle[] builds,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        Replic[] replicsOnFail,
        IStage stageOnFail = null
        )
    {
        SetScenario(stageScenario);
        SetAISettingsBySide(AISettings);
        SetConditionsForPass(conditionsForPass);
        SetConditionsForFail(conditionsForFail);
        SetUnits(units);
        SetBuilds(builds);
        SetReplics(replicsOnStart, replicsOnPass, replicsOnFail);
        _stageOnFail = stageOnFail;
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

    public void SetScenario(Scenario value)
    {
        _scenario = value;
    }

    public void SetAISettingsBySide(AISettings[] value)
    {
        _AISettingsBySide = new Dictionary<string, AISettings>();
        foreach (var settings in value)
        {
            _AISettingsBySide.Add(settings.side, settings);
        }
    }

    public virtual void SetConditionsForPass(ICondition conditions)
    {
        _conditionsForPass = conditions;
    }

    public virtual void SetConditionsForFail(ICondition conditions)
    {
        _conditionsForFail = conditions;
    }

    public virtual void SetUnits(UnitOnBattle[] units)
    {
        _units = units;
    }

    public virtual void SetBuilds(BuildOnBattle[] builds)
    {
        _builds = builds;
    }

    public virtual void SetReplics(Replic[] replicsOnStart, Replic[] replicsOnEnd, Replic[] replicsOnFail)
    {
        _replicsOnStart = replicsOnStart;
        _replicsOnPass = replicsOnEnd;
        _replicsOnFail = replicsOnFail;
    }

    public void CreateObjects()
    {
        _objectProcessor.CreateObjectsOnBattle(_units, _builds);
    }

    public virtual void SetCustomProperties()
    {

    }

    public void OnStart()
    {
        CreateObjects();

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

    public bool IsFailed()
    {
        return IsAllConditionsForFailComply();
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

    public void OnFail()
    {
        if (ReplicsOnFail != null && ReplicsOnFail.Length > 0)
        {
            BeginDialogue(ReplicsOnFail);
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

    public bool IsAllConditionsForFailComply()
    {
        return ConditionsForFail.IsComply();
    }
}
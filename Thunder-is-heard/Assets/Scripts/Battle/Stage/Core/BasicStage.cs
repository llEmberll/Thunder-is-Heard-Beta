using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BasicStage: IStage
{
    [SerializeField] public Scenario _scenario;
    [SerializeField] public UnitOnBattle[] _units;
    [SerializeField] public BuildOnBattle[] _builds;

    [SerializeField] public Replic[] _replicsOnStart;
    [SerializeField] public Replic[] _replicsOnPass;
    [SerializeField] public Replic[] _replicsOnFail;

    [SerializeField] public List<IScenarioEvent> events;
    [SerializeField] public List<ICondition> _conditionsForPass;
    [SerializeField] public List<ICondition> _conditionsForFail;

    public UnitOnBattle[] Units { get { return _units; } }
    public BuildOnBattle[] Builds { get { return _builds; } }

    public Replic[] ReplicsOnStart { get { return _replicsOnStart; } }
    public Replic[] ReplicsOnPass { get { return _replicsOnPass; } }
    public Replic[] ReplicsOnFail { get { return _replicsOnFail; } }

    public List<IScenarioEvent> Events { get { return events; } }


    public Dictionary<string, AISettings> _AISettingsBySide;
    public Dictionary<string, AISettings> AISettingsBySide { get { return _AISettingsBySide; } }


    public List<ICondition> ConditionsForPass { get { return _conditionsForPass; } }

    public List<ICondition> ConditionsForFail { get { return _conditionsForFail; } }

    public Scenario Scenario { get { return _scenario; } }


    public ObjectProcessor _objectProcessor;
    public DialogueController _dialogueController;

    public bool isDialogue = false;
    public int replicIndex;


    public virtual void Init(
        Scenario stageScenario, 
        AISettings[] AISettings, 
        List<ICondition> conditionsForPass, 
        List<ICondition> conditionsForFail, 
        UnitOnBattle[] units, 
        BuildOnBattle[] builds,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        Replic[] replicsOnFail
        )
    {
        SetScenario(stageScenario);
        SetAISettingsBySide(AISettings);
        SetConditionsForPass(conditionsForPass);
        SetConditionsForFail(conditionsForFail);
        SetUnits(units);
        SetBuilds(builds);
        SetReplics(replicsOnStart, replicsOnPass, replicsOnFail);
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
        foreach (var settings  in value)
        {
            _AISettingsBySide.Add(settings.side, settings);
        }
    }

    public virtual void SetConditionsForPass(List<ICondition> conditions)
    {
        _conditionsForPass = conditions;
        foreach (ICondition condition in _conditionsForPass)
        {
            condition.Init(_scenario);
        }
    }

    public virtual void SetConditionsForFail(List<ICondition> conditions)
    {
        _conditionsForFail = conditions;
        foreach (ICondition condition in _conditionsForFail)
        {
            condition.Init(_scenario);
        }
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
        foreach (var condition in ConditionsForPass) 
        { 
            if (!condition.IsComply())
            {
                return false;
            }
        }

        return true;
    }

    public bool IsAllConditionsForFailComply()
    {
        foreach (var condition in ConditionsForFail)
        {
            if (!condition.IsComply())
            {
                return false;
            }
        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicStage: IStage
{
    [SerializeField] public Scenario _scenario;
    [SerializeField] public UnitOnBattle[] _units;
    [SerializeField] public BuildOnBattle[] _builds;
    [SerializeField] public List<IScenarioEvent> events;
    [SerializeField] public List<ICondition> _conditionsForPass;
    [SerializeField] public List<ICondition> _conditionsForFail;

    public UnitOnBattle[] Units { get { return _units; } }
    public BuildOnBattle[] Builds { get { return _builds; } }

    public List<IScenarioEvent> Events { get { return events; } }

    public List<ICondition> ConditionsForPass { get { return _conditionsForPass; } }

    public List<ICondition> ConditionsForFail { get { return _conditionsForFail; } }

    public Scenario Scenario { get { return _scenario; } }


    public ObjectProcessor _objectProcessor;


    public virtual void Init(Scenario stageScenario, List<ICondition> conditionsForPass, List<ICondition> conditionsForFail, UnitOnBattle[] units, BuildOnBattle[] builds)
    {
        SetScenario(stageScenario);
        SetConditionsForPass(conditionsForPass);
        SetConditionsForFail(conditionsForFail);
        SetUnits(units);
        SetBuilds(builds);
        SetCustomProperties();

        InitObjectProcessor();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void SetScenario(Scenario value)
    {
        _scenario = value;
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
    }

    public void OnProcess()
    {
    }

    public void OnFinish()
    {
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
    }

    public void OnFail()
    {
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

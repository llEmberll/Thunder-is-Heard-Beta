using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicStage: IStage
{
    [SerializeField] public Scenario scenario;
    [SerializeField] public Dictionary<Vector2Int, Entity> objects;
    [SerializeField] public List<IScenarioEvent> events;
    [SerializeField] public List<ICondition> conditionsForPass;
    [SerializeField] public List<ICondition> conditionsForFail;
    public Dictionary<Vector2Int, Entity> Objects { get { return objects; } }
    public List<IScenarioEvent> Events { get { return events; } }

    public List<ICondition> ConditionsForPass { get { return conditionsForPass; } }

    public List<ICondition> ConditionsForFail { get { return conditionsForFail; } }

    public Scenario Scenario { get { return scenario; } }

    public virtual void Init(Scenario stageScenario)
    {
        SetScenario(stageScenario);
        SetConditionsForPass();
        SetConditionsForFail();
        SetCustomProperties();
    }

    public void SetScenario(Scenario value)
    {
        scenario = value;
    }

    public virtual void SetConditionsForPass()
    {

    }

    public virtual void SetConditionsForFail()
    {

    }

    public virtual void SetCustomProperties()
    {

    }

    public void OnStart()
    {
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

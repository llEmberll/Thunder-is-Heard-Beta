using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage
{
    public Scenario Scenario { get; }
    public Dictionary<Vector2Int, Entity> Objects { get; }
    public List<IScenarioEvent> Events { get; }

    public List<ICondition> ConditionsForPass { get; }
    public List<ICondition> ConditionsForFail { get; }

    public void Init(Scenario stageScenario);
    public void SetScenario(Scenario value);
    public void SetConditionsForPass();
    public void SetConditionsForFail();
    public void SetCustomProperties();


    public void OnStart();
    public void OnProcess();
    public void OnFinish();

    public bool IsPassed();
    public bool IsFailed();

    public void OnPass();
    public void OnFail();
}

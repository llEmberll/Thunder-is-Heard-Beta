using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage
{
    public Scenario Scenario { get; }
    public UnitOnBattle[] Units { get; }
    public BuildOnBattle[] Builds { get; }
    public List<IScenarioEvent> Events { get; }


    public Dictionary<string, AISettings> AISettingsBySide { get; }


    public List<ICondition> ConditionsForPass { get; }
    public List<ICondition> ConditionsForFail { get; }

    public void Init(Scenario stageScenario, AISettings[] AISettings, List<ICondition> conditionsForPass, List<ICondition> conditionsForFail, UnitOnBattle[] units, BuildOnBattle[] builds);
    public void SetScenario(Scenario value);
    public void SetConditionsForPass(List<ICondition> conditions);
    public void SetConditionsForFail(List<ICondition> conditions);
    public void SetUnits(UnitOnBattle[] units);
    public void SetBuilds(BuildOnBattle[] builds);
    public void SetCustomProperties();


    public void OnStart();
    public void OnProcess();
    public void OnFinish();

    public bool IsPassed();
    public bool IsFailed();

    public void OnPass();
    public void OnFail();
}

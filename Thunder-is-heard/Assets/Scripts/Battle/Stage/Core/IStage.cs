using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStage
{
    public Scenario Scenario { get; }
    public UnitOnBattle[] Units { get; }
    public BuildOnBattle[] Builds { get; }

    public Replic[] ReplicsOnStart { get; }
    public Replic[] ReplicsOnPass { get; }
    public Replic[] ReplicsOnFail { get; }

    public List<IScenarioEvent> Events { get; }


    public Dictionary<string, AISettings> AISettingsBySide { get; }


    public ICondition ConditionsForPass { get; }
    public ICondition ConditionsForFail { get; }

    public IStage StageOnFail { get; }

    public void Init(
        Scenario stageScenario, 
        AISettings[] AISettings, 
        ICondition conditionsForPass, 
        ICondition conditionsForFail,
        UnitOnBattle[] units, 
        BuildOnBattle[] builds,
        Replic[] replicsOnStart,
        Replic[] replicsOnPass,
        Replic[] replicOnFail,
        IStage stageOnFail
        );
    public void SetScenario(Scenario value);
    public void SetConditionsForPass(ICondition conditions);
    public void SetConditionsForFail(ICondition conditions);
    public void SetUnits(UnitOnBattle[] units);
    public void SetBuilds(BuildOnBattle[] builds);
    public void SetReplics(Replic[] replicOnStart, Replic[] replicOnPass, Replic[] replicOnFail);

    public void SetCustomProperties();


    public void OnStart();
    public void OnProcess();
    public void OnFinish();

    public bool IsPassed();
    public bool IsFailed();

    public void OnPass();
    public void OnFail();
}

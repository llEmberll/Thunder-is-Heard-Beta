using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scenario : MonoBehaviour
{
    public List<Vector2Int> _landableCells;
    public List<Vector2Int> LandableCells { get { return _landableCells; } }

    public int _landingMaxStaff;
    public int LandingMaxStaff { get { return _landingMaxStaff; } }

    public Map map;
    public Map Map { get { return map; } }


    public IStage _currentStage;
    public IStage CurrentStage { get { return _currentStage; } }


    public Replic[] _initialDialogue;
    public Replic[] InitialDialogue { get { return _initialDialogue; } }

    public bool waitingForEndDialogue = false;


    public ObjectProcessor _objectProcessor;
    public FightDirector _fightDirector;
    public UnitsOnFight _unitsOnFight;
    public BuildsOnFight _buildsOnFight;

    public bool waitingForUpdateStage = false;


    public void Init(Map scenarioMap, IStage currentStage, Replic[] startDialogue, FightDirector fightDirector)
    {
        map = scenarioMap;
        _currentStage = currentStage;
        _fightDirector = fightDirector;

        _initialDialogue = startDialogue;

        InitObjectProcessor();
        InitUnitsOnFight();
        InitBuildsOnFight();
        EnableListenerForUpdateStage();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void InitUnitsOnFight()
    {
        _unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
    }

    public void InitBuildsOnFight()
    {
        _buildsOnFight = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnFight>();
    }

    public Unit FindUnitById(string unitId)
    {
        return _unitsOnFight.FindObjectByChildId(unitId) as Unit;
    }

    public Build FindBuildById(string buildId)
    {
        return _buildsOnFight.FindObjectByChildId(buildId) as Build;
    }

    public void EnableListenerForUpdateStage()
    {
        EventMaster.current.StageUpdated += OnUpdateStage;
        waitingForUpdateStage = true;
    }

    public void DisableListenerForUpdateStage()
    {
        EventMaster.current.StageUpdated -= OnUpdateStage;
        waitingForUpdateStage = false;
    }

    public bool IsStageProcessing()
    {
        return (waitingForUpdateStage || waitingForEndDialogue);
    }

    public void OnUpdateStage()
    {
        DisableListenerForUpdateStage();
    }

    public void EnableListenerForEndDialogue()
    {
        waitingForEndDialogue = true;
        EventMaster.current.DialogueEnd += OnEndDialogue;
    }

    public void DisableListenerForEndDialogue()
    {
        waitingForEndDialogue = false;
        EventMaster.current.DialogueEnd -= OnEndDialogue;
    }

    public void OnEndDialogue()
    {
        waitingForEndDialogue = false;
    }

    public IEnumerator ToNextStage(IStage nextStage)
    {
        Debug.Log("To next stage");
        EnableListenerForUpdateStage();
        CurrentStage.OnFinish();
        yield return new WaitUntil(() => !waitingForUpdateStage);

        _currentStage = nextStage;
        EventMaster.current.OnCurrentStageChange(CurrentStage);
        EventMaster.current.OnNextStage(_currentStage);

        Debug.Log("Changed stage");

        EnableListenerForUpdateStage();
        CurrentStage.OnStart();
        yield return new WaitUntil(() => !waitingForUpdateStage);
    }

    public IEnumerator ContinueStage()
    {
        EventMaster.current.OnStageBegin(_currentStage);

        EnableListenerForUpdateStage();
        CurrentStage.OnProcess();
        yield return new WaitUntil(() => !waitingForUpdateStage);
    }

    public IEnumerator StartInitialDialogue()
    {
        if (InitialDialogue != null && InitialDialogue.Length > 0)
        {
            EventMaster.current.BeginDialogue(InitialDialogue);
            EnableListenerForEndDialogue();
            yield return new WaitUntil(() => !waitingForEndDialogue);
        }
    }

    public IEnumerator Begin()
    {
        EventMaster.current.OnStageBegin(CurrentStage);

        EnableListenerForUpdateStage();
        CurrentStage.OnStart();
        yield return new WaitUntil(() => !waitingForUpdateStage);
    }

    public IEnumerator OnNextTurn()
    {
        yield return CheckConditionAndRunNextIfNeed();
    }

    public IEnumerator CheckConditionAndRunNextIfNeed()
    {
        if (CurrentStage.IsFailed())
        {
            EnableListenerForUpdateStage();
            CurrentStage.OnFail();
            yield return new WaitUntil(() => !waitingForUpdateStage);

            IStage nextStage = CurrentStage.StageOnFail;
            if (nextStage == null)
            {
                EventMaster.current.LoseFigth();
                yield break;
            }
            else
            {
                yield return StartCoroutine(ToNextStage(nextStage));
            }
        }

        else if (CurrentStage.IsPassed())
        {
            EnableListenerForUpdateStage();
            CurrentStage.OnPass();
            yield return new WaitUntil(() => !waitingForUpdateStage);
            IStage nextStage = CurrentStage.StageOnPass;
            if (nextStage == null)
            {
                Debug.Log("ÏÎÁÅÄÀ");

                EventMaster.current.WinFight();
                yield break;
            }
            else
            {
                yield return StartCoroutine(ToNextStage(nextStage));
            }
        }

        else
        {
            yield return StartCoroutine(ContinueStage());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scenario: MonoBehaviour
{
    public List<Vector2Int> _landableCells;
    public List<Vector2Int> LandableCells { get { return _landableCells; } }

    public int _landingMaxStaff;
    public int LandingMaxStaff { get { return _landingMaxStaff; } }

    public Map map;
    public Map Map { get { return map; } }


    public List<IStage> _stages;
    public List<IStage> Stages { get { return _stages; } }


    public IStage _currentStage;
    public IStage CurrentStage { get { return _currentStage; } }

    public int _currentStageIndex = 0;
    public int CurrentStageIndex { get { return _currentStageIndex; } }


    public Replic[] _initialDialogue;
    public Replic[] InitialDialogue { get { return _initialDialogue; } }

    public bool waitingForEndDialogue = false;


    public bool _isLanded = false;


    public ObjectProcessor _objectProcessor;

    public bool waitingForUpdateStage = false;


    public void Init(Map scenarioMap, List<Vector2Int> scenarioLandableCells, int landingMaxStaff, List<IStage> scenarioStages, int currentStage, Replic[] startDialogue, bool isLanded)
    {
        map = scenarioMap;
        _landableCells = scenarioLandableCells;
        _landingMaxStaff = landingMaxStaff;
        _stages = scenarioStages;
        _currentStageIndex = currentStage;
        if (_stages.Count == 0)
        {
            _currentStage = null;
        }
        else
        {
            _currentStage = _stages[_currentStageIndex];
        }

        _initialDialogue = startDialogue;
        
        _isLanded = isLanded;

        InitObjectProcessor();
        EnableListenerForUpdateStage();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
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

    public IEnumerator ToNextStage()
    {
        Debug.Log("Çàâåðøåíèå ïðåäûäóùåãî ýòàïà");
        EnableListenerForUpdateStage();
        CurrentStage.OnFinish();
        yield return new WaitUntil(() => !waitingForUpdateStage);


        _currentStageIndex++;
        if (_currentStageIndex + 1 > _stages.Count)
        {
            Debug.Log("ÏÎÁÅÄÀ");

            EventMaster.current.WinFight();
            yield break;
        }

        _currentStage = Stages[_currentStageIndex];

        EventMaster.current.OnStageIndexChange(_currentStageIndex);
        EventMaster.current.OnNextStage(_currentStage);

        Debug.Log("Íà÷àëî íîâîãî ýòàïà");

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

    public void StartLanding()
    {
        EventMaster.current.Landing(LandableCells, LandingMaxStaff);
    }

    public IEnumerator Begin()
    {
        _currentStage = Stages[CurrentStageIndex];
        EventMaster.current.OnStageBegin(_currentStage);

        EnableListenerForUpdateStage();
        CurrentStage.OnStart();
        yield return new WaitUntil(() => !waitingForUpdateStage);
    }

    public IEnumerator OnNextTurn()
    {
        if (CurrentStage.IsFailed())
        {
            Debug.Log("ÏÎÐÀÆÅÍÈÅ");

            EnableListenerForUpdateStage();
            CurrentStage.OnFail();
            yield return new WaitUntil(() => !waitingForUpdateStage);
            EventMaster.current.LoseFigth();
        }

        else if (CurrentStage.IsPassed()) 
        {
            Debug.Log("ÑËÅÄÓÞÙÈÉ ÝÒÀÏ");

            EnableListenerForUpdateStage();
            CurrentStage.OnPass();
            yield return new WaitUntil(() => !waitingForUpdateStage);

            yield return StartCoroutine(ToNextStage());
        }

        else
        {
            yield return StartCoroutine(ContinueStage());
        }
    }
}

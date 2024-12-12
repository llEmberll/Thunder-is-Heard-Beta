using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Scenario
{
    [SerializeField] public List<Vector2Int> _landableCells;
    public List<Vector2Int> LandableCells { get { return _landableCells; } }

    [SerializeField] public int _landingMaxStaff;
    public int LandingMaxStaff { get { return _landingMaxStaff; } }

    [SerializeField] public Map map;
    public Map Map { get { return map; } }


    [SerializeField] public List<IStage> _stages;
    public List<IStage> Stages { get { return _stages; } }


    [SerializeField] public IStage _currentStage;
    public IStage CurrentStage { get { return _currentStage; } }

    [SerializeField] public int _currentStageIndex = 0;
    public int CurrentStageIndex { get { return _currentStageIndex; } }

    public bool _isLanded = false;


    public ObjectProcessor _objectProcessor;


    public void Init(Map scenarioMap, List<Vector2Int> scenarioLandableCells, int landingMaxStaff, List<IStage> scenarioStages, int currentStage, bool isLanded)
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
        
        _isLanded = isLanded;

        InitObjectProcessor();
    }

    public void InitObjectProcessor()
    {
        _objectProcessor = GameObject.FindGameObjectWithTag(Tags.objectProcessor).GetComponent<ObjectProcessor>();
    }

    public void ToNextStage()
    {
        Debug.Log("���������� ����������� �����");
        CurrentStage.OnFinish();

        _currentStageIndex++;
        if (_currentStageIndex + 1 > _stages.Count)
        {
            Debug.Log("������");

            EventMaster.current.WinFight();
            return;
        }

        _currentStage = Stages[_currentStageIndex];

        EventMaster.current.OnStageIndexChange(_currentStageIndex);
        EventMaster.current.OnNextStage(_currentStage);

        Debug.Log("������ ������ �����");
        CurrentStage.OnStart();
    }

    public void ContinueStage()
    {
        //��������� battleData
        EventMaster.current.OnStageBegin(_currentStage);
        CurrentStage.OnProcess();
    }

    public void StartLanding()
    {
        EventMaster.current.Landing(LandableCells, LandingMaxStaff);
    }

    public void Begin()
    {
        _currentStage = Stages[CurrentStageIndex];
        EventMaster.current.OnStageBegin(_currentStage);

        CurrentStage.OnStart();
    }

    public void OnNextTurn()
    {
        if (CurrentStage.IsFailed())
        {
            Debug.Log("���������");

            CurrentStage.OnFail();
            EventMaster.current.LoseFigth();
            return;
        }

        if (CurrentStage.IsPassed()) 
        {
            Debug.Log("��������� ����");

            CurrentStage.OnPass();
            ToNextStage();
            return;
        }

        ContinueStage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Scenario
{
    [SerializeField] public List<Vector2Int> _landableCells;
    public List<Vector2Int> LandableCells { get { return _landableCells; } }

    [SerializeField] public int _landingMaxStaff;
    public int LandingMaxStaff { get { return _landingMaxStaff; } }

    [SerializeField] public Map map;
    public Map Map { get { return map; } }

    [SerializeField] public UnitOnBattle[] _units;
    [SerializeField] public BuildOnBattle[] _builds;

    public UnitOnBattle[] Units { get { return _units; } }
    public BuildOnBattle[] Builds { get { return _builds; } }

    [SerializeField] public List<IStage> _stages;
    public List<IStage> Stages { get { return _stages; } }


    [SerializeField] public IStage _currentStage;
    public IStage CurrentStage { get { return _currentStage; } }

    [SerializeField] public int _currentStageIndex = 0;
    public int CurrentStageIndex { get { return _currentStageIndex; } }

    public bool _isLanded = false;


    public ObjectProcessor _objectProcessor;


    public void Init(Map scenarioMap, UnitOnBattle[] units, BuildOnBattle[] builds, List<Vector2Int> scenarioLandableCells, int landingMaxStaff, List<IStage> scenarioStages, int currentStage, bool isLanded)
    {
        map = scenarioMap;
        _units = units;
        _builds = builds;
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
        CurrentStage.OnFinish();

        _currentStageIndex++;
        if (_currentStageIndex + 1 > _stages.Count)
        {
            EventMaster.current.WinFight();
            return;
        }

        _currentStage = Stages[_currentStageIndex];
        CurrentStage.OnStart();
    }

    public void ContinueStage()
    {
        //Сохранить battleData

        CurrentStage.OnProcess();
    }

    public void Begin()
    {
        Debug.Log("Scenario: in begin");

        if (!_isLanded)
        {
            Debug.Log("Scenario: not isLanded");

            Debug.Log("Scenario: event landing!");

            EventMaster.current.Landing(LandableCells, LandingMaxStaff);
            return;
        }
        else
        {

            Debug.Log("Scenario: isLanded true!");

            _currentStage = Stages[CurrentStageIndex];

            CurrentStage.OnStart();
        }
    }

    public void OnNextTurn()
    {
        if (CurrentStage.IsFailed())
        {
            CurrentStage.OnFail();
            EventMaster.current.LoseFigth();
            return;
        }

        if (CurrentStage.IsPassed()) 
        {
            CurrentStage.OnPass();
            ToNextStage();
            return;
        }

        ContinueStage();
    }


}

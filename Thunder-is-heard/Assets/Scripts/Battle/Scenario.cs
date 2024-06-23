using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Scenario
{
    [SerializeField] public List<Vector2Int> landableCells;
    public List<Vector2Int> LandableCells { get { return landableCells; } }

    [SerializeField] public Map map;
    public Map Map { get { return map; } }


    [SerializeField] public Sprite terrain;
    public Sprite Terrain { get { return terrain; } }


    [SerializeField] public Dictionary<string, Entity> objects;
    public Dictionary<string, Entity> Objects { get { return objects; } }


    [SerializeField] public List<IStage> stages;
    public List<IStage> Stages { get { return stages; } }


    [SerializeField] public IStage currentStage;
    public IStage CurrentStage { get { return currentStage; } }

    [SerializeField] public int currentStageIndex = 0;
    public int CurrentStageIndex { get { return currentStageIndex; } }

    public Scenario(Map scenarioMap, Sprite scenarioTerrain, Dictionary<string, Entity> scenarioObjects, List<IStage> scenarioStages, List<Vector2Int> scenarioLandableCells)
    {
        map = scenarioMap;
        terrain = scenarioTerrain;
        objects = scenarioObjects;
        stages = scenarioStages;
        landableCells = scenarioLandableCells;

        foreach (var stage in stages)
        {
            stage.Init(this);
        }
    }

    public void ToNextStage()
    {
        CurrentStage.OnFinish();

        currentStageIndex++;
        if (currentStageIndex + 1 > stages.Count)
        {
            EventMaster.current.WinFight();
            return;
        }

        currentStage = Stages[currentStageIndex];

        CurrentStage.OnStart();
    }

    public void ContinueStage()
    {
        CurrentStage.OnProcess();
    }

    public void Begin()
    {
        currentStage = Stages[CurrentStageIndex];

        currentStage.OnStart();
    }

    public void OnNextTurn()
    {
        if (CurrentStage.IsFailed())
        {
            CurrentStage.OnFail();
            EventMaster.current.LoseFigth();
            return;
        }

        if (currentStage.IsPassed()) 
        {
            CurrentStage.OnPass();
            ToNextStage();
            return;
        }

        ContinueStage();
    }


}

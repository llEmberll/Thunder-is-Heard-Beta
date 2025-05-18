using UnityEngine;

public class BaseSubsituableBuildingOptionsBehaviour : ISubsituableBuildingOptionsBehaviour
{
    public virtual void Init(BuildingPanel conductor)
    {
        SceneState sceneState = GameObject.FindWithTag(Tags.state).GetComponent<SceneState>();
        State currentState = sceneState.GetCurrentState();
        if (currentState.stateName == "Building")
        {
            OnBuildMode(conductor);
        }
        else
        {
            OnExitBuildMode(conductor);
        }
    }

    public virtual void OnBuildMode(BuildingPanel conductor)
    {
        conductor.TurnOnAllOptionsExceptTurnOnBuilding();
    }

    public virtual void OnExitBuildMode(BuildingPanel conductor)
    {
        conductor.TurnOffAllOptionsExceptTurnOnBuilding();
    }

    public virtual void TurnOffBuilding(BuildingPanel conductor)
    {
        EventMaster.current.OnExitBuildMode();
    }

    public virtual void TurnOnBuilding(BuildingPanel conductor)
    {
        EventMaster.current.OnBuildMode();
    }

    public virtual void Cancel(BuildingPanel conductor)
    {
        TurnOffBuilding(conductor);
    }

    public virtual void Rotate(BuildingPanel conductor)
    {
        EventMaster.current.OnRotatePreview();
    }

    public virtual void ToInventory(BuildingPanel conductor)
    {
        ObjectProcessor.PutSelectedObjectOnBaseToInventory();
    }

    public virtual void Sell(BuildingPanel conductor)
    {
        Debug.Log("Опция в разработке");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSubsituableBuildingOptionsBehaviour : ISubsituableBuildingOptionsBehaviour
{
    ObjectProcessor _objectProcessor;

    public virtual void Init(BuildingPanel conductor)
    {
        _objectProcessor = GameObjectUtils.FindGameObjectByTagIncludingInactive(Tags.objectProcessor).GetComponent<ObjectProcessor>();

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
        State _baseState = StateConfig.statesByScene[SceneManager.GetActiveScene().name];
        if (_baseState.stateName == Scenes.fight)
        {
            _objectProcessor.PutSelectedObjectOnBattleToInventory();
        }

        else if (_baseState.stateName != Scenes.home)
        {
            ObjectProcessor.PutSelectedObjectOnBaseToInventory();
        }
       
        else
        {
            Debug.Log("Put to inventory: undefined game state(Fight | Home)");
        }
    }

    public virtual void Sell(BuildingPanel conductor)
    {
        Debug.Log("Опция в разработке");
    }
}

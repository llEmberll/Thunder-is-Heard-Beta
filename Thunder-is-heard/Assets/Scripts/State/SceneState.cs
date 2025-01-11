using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneState : MonoBehaviour
{
    public State currentState;


    public void Awake()
    {
        SetBaseState();
    }

    public void Start()
    {
        EventMaster.current.ToggledToBuildMode += OnBuildMode;
        EventMaster.current.ToggledOffBuildMode += OnExitBuildMode;
        EventMaster.current.ToggledToBaseMode += OnBaseMode;
    }


    public void OnBaseMode()
    {
        SetBaseState();
    }

    public State GetCurrentState()
    {
        return currentState;
    }
    
    public void OnBuildMode()
    {
        Debug.Log("On build mode!");

        SetNewState(StateConfig.buildingState);
    }

    public void OnExitBuildMode()
    {

        SetBaseState();
    }

    public void SetNewState(State value)
    {
        currentState = value;
        EventMaster.current.OnChangeState(currentState);
    }

    public void SetBaseState()
    {
        currentState = StateConfig.statesByScene[SceneManager.GetActiveScene().name];
        EventMaster.current.OnChangeState(currentState);
    }
    
    
}

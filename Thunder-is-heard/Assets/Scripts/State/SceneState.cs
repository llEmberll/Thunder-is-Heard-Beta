using System.Collections;
using System.Collections.Generic;
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
    }


    public State GetCurrentState()
    {
        return currentState;
    }
    
    public void OnBuildMode()
    {
        currentState = StateConfig.buildingState;
        
        EventMaster.current.OnChangeState(currentState);
    }

    public void OnExitBuildMode()
    {
        SetBaseState();
        
        EventMaster.current.OnChangeState(currentState);
    }

    public void SetBaseState()
    {
        currentState = StateConfig.statesByScene[SceneManager.GetActiveScene().name];
    }
    
    
}

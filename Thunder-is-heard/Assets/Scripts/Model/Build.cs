using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Build : Entity, IDamageable, ITransfer
{
    
    public override string entityType {
        get
        {
            return "Build";
        }
    }	
    
    public StateMachine stateMachine = new StateMachine();
    
    public SceneState sceneState;

    public void Awake()
    {
        
    }
    
    public void Start()
    {
        sceneState = GameObject.FindWithTag("State").GetComponent<SceneState>();
        
        stateMachine.Initialize(sceneState.GetCurrentState());
        
        //OnChangeStateEvent
        EventMaster.current.StateChanged += OnChangeState;
    }

    public override void OnChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
    }
    public void GetDamage()
    {
        Debug.Log("Build damaged!");
    }
    
    public void Rotate()
    {
        Debug.Log("Build rotated!");
    }

    public void Replace()
    {
        Debug.Log("Build replaced!");
    }
    
    protected override void OnMouseEnter()
    {
        stateMachine.currentState.OnBuildMouseEnter(this);
    }

    protected override void OnMouseExit()
    {
        stateMachine.currentState.OnBuildMouseExit(this);
    }

    protected override void OnMouseDown()
    {
        stateMachine.currentState.OnBuildClick(this);
    }
}

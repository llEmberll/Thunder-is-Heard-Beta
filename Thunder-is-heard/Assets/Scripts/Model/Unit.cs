using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : Entity, IMovable, IDamageable, IAttack, ITransfer
{
    public override string entityType {
	get
        {
            return "Unit";
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
    
    public void Move()
    {
        Debug.Log("Unit moves!");
    }
    
    public void GetDamage()
    {
        Debug.Log("Unit damaged!");
    }
    
    public void Attack()
    {
        Debug.Log("Unit attacks!");
    }

    public void Toggle()
    {
        Debug.Log("Unit toggled!");
    }

    public void Rotate()
    {
        Debug.Log("Unit rotated!");
    }

    public void Replace()
    {
        Debug.Log("Unit replaced!");
    }

    protected override void OnMouseEnter()
    {
        stateMachine.currentState.OnUnitMouseEnter(this);
    }

    protected override void OnMouseExit()
    {
        stateMachine.currentState.OnUnitMouseExit(this);
    }

    protected override void OnMouseDown()
    {
        stateMachine.currentState.OnUnitClick(this);
    }
}

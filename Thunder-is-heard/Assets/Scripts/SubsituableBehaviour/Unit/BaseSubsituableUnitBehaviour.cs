using UnityEngine;


public class BaseSubsituableUnitBehaviour : ISubsituableUnitBehaviour
{
    FocusController _focusController;

    public virtual void Init(Unit conductor)
    {
        _focusController = GameObject.FindGameObjectWithTag(Tags.focusController).GetComponent<FocusController>();
    }

    public virtual void OnClick(Unit conductor) 
    {
        if (!IsAllowToInteract(conductor)) return;

        conductor.stateMachine.currentState.OnUnitClick(conductor);
    }

    public virtual void OnFocus(Unit conductor)
    {
        if (!IsAllowToInteract(conductor)) return;

        conductor.stateMachine.currentState.OnUnitMouseEnter(conductor);
    }

    public virtual void OnDefocus(Unit conductor)
    {
        conductor.stateMachine.currentState.OnUnitMouseExit(conductor);
    }

    public virtual bool IsAllowToInteract(Unit conductor)
    {
        return _focusController._targetEntity == null || _focusController._targetEntity.Equals(conductor);
    }
}


public class BaseSubsituableUnitBehaviour : ISubsituableUnitBehaviour
{

    public virtual void Init(Unit conductor)
    {

    }

    public virtual void OnClick(Unit conductor) 
    {
        conductor.stateMachine.currentState.OnUnitClick(conductor);
    }

    public virtual void OnFocus(Unit conductor)
    {
        conductor.stateMachine.currentState.OnUnitMouseEnter(conductor);
    }

    public virtual void OnDefocus(Unit conductor)
    {
        conductor.stateMachine.currentState.OnUnitMouseExit(conductor);
    }
}

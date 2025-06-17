
public class BaseSubsituableCellBehaviour : ISubsituableCellBehaviour
{

    public virtual void Init(Cell conductor)
    {

    }

    public virtual void OnClick(Cell conductor) 
    {
        conductor.stateMachine.currentState.OnCellClick(conductor);
    }

    public virtual void OnFocus(Cell conductor)
    {
        conductor.stateMachine.currentState.OnCellMouseEnter(conductor);
    }

    public virtual void OnDefocus(Cell conductor)
    {
        conductor.stateMachine.currentState.OnCellMouseExit(conductor);
    }
}


public class DisabledObstacleBehaviour : BaseSubsituableObstacleBehaviour
{

    public virtual void OnClick(Obstacle conductor) 
    {
        
    }

    public virtual void OnFocus(Obstacle conductor)
    {
        conductor.stateMachine.currentState.OnObstacleMouseEnter(conductor);
    }

    public virtual void OnDefocus(Obstacle conductor)
    {
        conductor.stateMachine.currentState.OnObstacleMouseExit(conductor);
    }
}

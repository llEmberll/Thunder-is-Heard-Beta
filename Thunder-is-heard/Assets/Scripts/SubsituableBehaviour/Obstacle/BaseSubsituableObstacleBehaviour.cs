
public class BaseSubsituableObstacleBehaviour : ISubsituableObstacleBehaviour
{

    public virtual void Init(Obstacle conductor)
    {

    }

    public virtual void OnClick(Obstacle conductor) 
    {
        conductor.stateMachine.currentState.OnObstacleClick(conductor);
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

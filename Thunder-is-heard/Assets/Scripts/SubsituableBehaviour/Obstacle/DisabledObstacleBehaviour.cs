
public class DisabledObstacleBehaviour : BaseSubsituableObstacleBehaviour
{

    public override void OnClick(Obstacle conductor) 
    {
        
    }

    public override void OnFocus(Obstacle conductor)
    {
        conductor.stateMachine.currentState.OnObstacleMouseEnter(conductor);
    }

    public override void OnDefocus(Obstacle conductor)
    {
        conductor.stateMachine.currentState.OnObstacleMouseExit(conductor);
    }
}

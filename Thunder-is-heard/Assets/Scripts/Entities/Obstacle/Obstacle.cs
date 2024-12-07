using UnityEngine;


public class Obstacle : Entity, ITransfer
{
    public new string side = Sides.neutral;

    public override string Type {
        get
        {
            return "Obstacle";
        }
    }
    
    public void Rotate()
    {
        Debug.Log("Obstacle rotated!");
    }

    public void Replace()
    {
        Debug.Log("Obstacle replaced!");
    }

    public override void OnFocus()
    {
        stateMachine.currentState.OnObstacleMouseEnter(this);
    }

    public override void OnDefocus()
    {
        stateMachine.currentState.OnObstacleMouseExit(this);
    }

    public override void OnClick()
    {
        Debug.Log("Click On Obstacle");

        stateMachine.currentState.OnObstacleClick(this);
    }
}

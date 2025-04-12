using UnityEngine;


public class Obstacle : Entity, ITransfer
{
    public new string side = Sides.neutral;

    public ISubsituableObstacleBehaviour _behaviour;

    public override string Type {
        get
        {
            return "Obstacle";
        }
    }

    public override void Awake()
    {
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset += OnResetBehaviour;

        base.Awake();
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
        _behaviour.OnFocus(this);
    }

    public override void OnDefocus()
    {
        _behaviour.OnDefocus(this);
    }

    public override void OnClick()
    {
        _behaviour.OnClick(this);
    }

    public void OnSomeComponentChangeBehaviour(string componentName, string behaviourName)
    {
        if (componentName != Type) return;
        ChangeBehaviour(behaviourName);
    }

    public void OnResetBehaviour()
    {
        ChangeBehaviour();
    }

    public void ChangeBehaviour(string name = "Base")
    {
        _behaviour = SubsituableObstacleFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }
}

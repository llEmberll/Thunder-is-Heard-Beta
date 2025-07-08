

public class AttackSideCondition : BasicCondition
{
    public string _side;
    public bool attacked = false;

    public AttackSideCondition(string side) 
    { 
        _side = side;
        EnableListeners();
    }


    public void EnableListeners()
    {
        EventMaster.current.DamagedObject += SomeObjectHasBeenAttacked;
    }

    public void DisableListeners()
    {
        EventMaster.current.DamagedObject -= SomeObjectHasBeenAttacked;
    }

    public void SomeObjectHasBeenAttacked(Entity obj)
    {
        if (obj.side == _side)
        {
            attacked = true;
            DisableListeners();
        }
    }


    public override bool IsComply()
    {
        return attacked;
    }
}

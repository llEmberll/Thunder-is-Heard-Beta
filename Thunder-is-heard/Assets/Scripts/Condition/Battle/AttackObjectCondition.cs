

public class AttackObjectCondition : BasicCondition
{
    public string _targetObjectId;
    public bool attacked = false;

    public AttackObjectCondition(string targetObjectId) 
    { 
        _targetObjectId = targetObjectId;
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
        if (obj.ChildId == _targetObjectId)
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

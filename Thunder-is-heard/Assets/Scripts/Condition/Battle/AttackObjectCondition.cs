

public class AttackObjectCondition : BasicCondition
{
    public string _targetObjectId;
    public bool attacked = false;

    public AttackObjectCondition(string targetObjectId) 
    { 
        _targetObjectId = targetObjectId;
        // Убираем EnableListeners() из конструктора - теперь это будет в OnActivate
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

    protected override void OnActivate()
    {
        // Подписываемся на события только при активации
        EnableListeners();
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        attacked = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        return attacked;
    }

    public override bool IsRealTimeUpdate()
    {
        return false;
    }
}

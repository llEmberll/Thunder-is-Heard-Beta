

public class AttackSideCondition : BasicCondition
{
    public string _side;
    public bool attacked = false;

    public AttackSideCondition(string side) 
    { 
        _side = side;
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
        if (obj.side == _side)
        {
            attacked = true;
            DisableListeners();
        }
    }

    protected override void OnActivate()
    {
        // Подписываемся на события при активации
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

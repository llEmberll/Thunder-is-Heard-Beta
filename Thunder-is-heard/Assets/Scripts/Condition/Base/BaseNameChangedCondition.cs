

public class BaseNameChangedCondition: BasicCondition
{
    public bool changed = false;

    public BaseNameChangedCondition() 
    { 
        // Убираем EnableListeners() из конструктора - теперь это будет в OnActivate
    }

    public void EnableListeners()
    {
        EventMaster.current.ChangedBaseName += OnChangeBaseName;
    }

    public void DisableListeners()
    {
        EventMaster.current.ChangedBaseName -= OnChangeBaseName;
    }

    public void OnChangeBaseName(string value)
    {
        changed = true;
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
        changed = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        return changed;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}

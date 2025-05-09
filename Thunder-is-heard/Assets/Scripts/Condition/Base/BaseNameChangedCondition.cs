

public class BaseNameChangedCondition: BasicCondition
{
    public bool changed = false;

    public BaseNameChangedCondition() 
    { 
        EnableListeners();
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


    public override bool IsComply()
    {
        return changed;
    }
}

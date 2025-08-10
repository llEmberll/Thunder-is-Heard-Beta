
public class AlwaysTrueCondition : BasicCondition
{

    public AlwaysTrueCondition() {}

    public override bool IsComply()
    {
        return true;
    }

    public override bool IsRealTimeUpdate()
    {
        return false;
    }
    
    // Простые условия не нуждаются в активации/деактивации
    protected override void OnActivate() { }
    protected override void OnDeactivate() { }
    protected override void OnReset() { }
}

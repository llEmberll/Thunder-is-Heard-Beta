
public class AlwaysFalseCondition : BasicCondition
{

    public AlwaysFalseCondition() {}

    public override bool IsComply()
    {
        return false;
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

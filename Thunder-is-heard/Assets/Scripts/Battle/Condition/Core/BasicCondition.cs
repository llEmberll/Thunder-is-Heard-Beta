
public abstract class BasicCondition : ICondition
{
    public Scenario scenario;
    public Scenario Scenario { get { return scenario; } }


    public abstract bool IsComply();
}

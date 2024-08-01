
public abstract class BasicCondition : ICondition
{
    public Scenario _scenario;
    public Scenario Scenario { get { return _scenario; } }

    public void Init(Scenario scenario)
    {
        _scenario = scenario;
    }

    public abstract bool IsComply();
}

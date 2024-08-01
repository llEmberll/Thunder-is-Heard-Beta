using UnityEngine;

public interface ICondition
{
    public Scenario Scenario { get; }

    public void Init(Scenario scenario);

    public bool IsComply();
}

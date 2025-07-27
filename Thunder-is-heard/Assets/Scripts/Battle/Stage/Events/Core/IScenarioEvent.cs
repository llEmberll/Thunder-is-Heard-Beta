using System.Collections;

public interface IScenarioEvent
{
    Scenario Scenario { get; }
    ScenarioEventData EventData { get; }

    bool IsCompleted { get; }
    IEnumerator Execute();
    void Cancel();
} 
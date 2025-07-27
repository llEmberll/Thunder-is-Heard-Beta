using System.Collections;
using UnityEngine;

public class WaitEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private WaitEventData waitData;

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }



    public WaitEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        waitData = eventData as WaitEventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (waitData == null)
        {
            IsCompleted = true;
            yield break;
        }

        yield return new WaitForSeconds(waitData.waitTime);
        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 
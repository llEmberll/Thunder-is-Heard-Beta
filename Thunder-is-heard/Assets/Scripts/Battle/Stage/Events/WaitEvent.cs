using System.Collections;
using UnityEngine;

public class WaitEvent : IScenarioEvent
{
    public ScenarioEventData EventData { get; private set; }
    public bool IsCompleted { get; private set; }

    private Scenario _scenario = GameObject.FindGameObjectWithTag(Tags.scenario).GetComponent<Scenario>();
    public Scenario Scenario { get { return _scenario; } }

    public WaitEvent(ScenarioEventData eventData)
    {
        EventData = eventData;
        IsCompleted = false;
    }

    public IEnumerator Execute()
    {
        if (EventData == null || EventData.eventType != "Wait")
        {
            IsCompleted = true;
            yield break;
        }

        // Получаем параметры из EventData
        float waitTime = EventData.GetParameter<float>("waitTime", 1f);

        yield return new WaitForSeconds(waitTime);
        IsCompleted = true;
    }

    public void Cancel()
    {
        IsCompleted = true;
    }
} 
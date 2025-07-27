using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ScenarioEventExecutor : MonoBehaviour
{
    private void Awake()
    {
        EventMaster.current.BegunScenarioEvents += OnBeginScenarioEvents;
    }

    private void OnDestroy()
    {
        EventMaster.current.BegunScenarioEvents -= OnBeginScenarioEvents;
    }

    private void OnBeginScenarioEvents(ScenarioEventData[] events)
    {
        StartCoroutine(ExecuteScenarioEvents(events));
    }

    private IEnumerator ExecuteScenarioEvents(ScenarioEventData[] events)
    {
        // Проверяем, есть ли события для параллельного выполнения
        var parallelEvents = new List<IScenarioEvent>();
        var sequentialEvents = new List<IScenarioEvent>();

        foreach (var eventData in events)
        {
            IScenarioEvent scenarioEvent = ScenarioEventFactory.CreateEvent(eventData);
            if (scenarioEvent == null) continue;

            if (eventData.executeInParallel)
            {
                parallelEvents.Add(scenarioEvent);
            }
            else
            {
                sequentialEvents.Add(scenarioEvent);
            }
        }

        // Сначала выполняем параллельные события
        if (parallelEvents.Count > 0)
        {
            yield return StartCoroutine(ExecuteParallelEvents(parallelEvents));
        }

        // Затем выполняем последовательные события
        foreach (var scenarioEvent in sequentialEvents)
        {
            var eventData = scenarioEvent.EventData;
            
            if (eventData.delay > 0)
            {
                yield return new WaitForSeconds(eventData.delay);
            }

            yield return StartCoroutine(scenarioEvent.Execute()); 

            if (eventData.waitForCompletion)
            {
                yield return new WaitUntil(() => scenarioEvent.IsCompleted);
            }
        }
        
        EventMaster.current.OnEndScenarioEvents();
    }

    private IEnumerator ExecuteParallelEvents(List<IScenarioEvent> events)
    {
        // Запускаем все события одновременно
        var coroutines = new List<Coroutine>();
        
        foreach (var scenarioEvent in events)
        {
            var eventData = scenarioEvent.EventData;
            
            if (eventData.delay > 0)
            {
                yield return new WaitForSeconds(eventData.delay);
            }
            
            coroutines.Add(StartCoroutine(scenarioEvent.Execute()));
        }

        // Ждем завершения всех событий
        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
    }
} 
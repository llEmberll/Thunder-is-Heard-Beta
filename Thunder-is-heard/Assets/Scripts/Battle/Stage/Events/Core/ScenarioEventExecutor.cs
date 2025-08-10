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
        Debug.Log($"[ScenarioEventExecutor] OnBeginScenarioEvents called with {events.Length} events");
        StartCoroutine(ExecuteScenarioEvents(events));
    }

    private IEnumerator ExecuteScenarioEvents(ScenarioEventData[] events)
    {
        Debug.Log($"[ScenarioEventExecutor] ExecuteScenarioEvents started with {events.Length} events");
        
        // Проверяем, есть ли события для параллельного выполнения
        var parallelEvents = new List<IScenarioEvent>();
        var sequentialEvents = new List<IScenarioEvent>();

        foreach (var eventData in events)
        {
            Debug.Log($"[ScenarioEventExecutor] Processing event: {eventData.eventType}, executeInParallel: {eventData.executeInParallel}");
            IScenarioEvent scenarioEvent = ScenarioEventFactory.CreateEvent(eventData);
            if (scenarioEvent == null) 
            {
                Debug.LogError($"[ScenarioEventExecutor] Failed to create event for type: {eventData.eventType}");
                continue;
            }

            if (eventData.executeInParallel)
            {
                parallelEvents.Add(scenarioEvent);
                Debug.Log($"[ScenarioEventExecutor] Added to parallel events: {eventData.eventType}");
            }
            else
            {
                sequentialEvents.Add(scenarioEvent);
                Debug.Log($"[ScenarioEventExecutor] Added to sequential events: {eventData.eventType}");
            }
        }

        Debug.Log($"[ScenarioEventExecutor] Parallel events: {parallelEvents.Count}, Sequential events: {sequentialEvents.Count}");

        // Сначала выполняем параллельные события
        if (parallelEvents.Count > 0)
        {
            Debug.Log($"[ScenarioEventExecutor] Executing {parallelEvents.Count} parallel events");
            yield return StartCoroutine(ExecuteParallelEvents(parallelEvents));
            Debug.Log($"[ScenarioEventExecutor] Parallel events completed");
        }

        // Затем выполняем последовательные события
        Debug.Log($"[ScenarioEventExecutor] Executing {sequentialEvents.Count} sequential events");
        foreach (var scenarioEvent in sequentialEvents)
        {
            var eventData = scenarioEvent.EventData;
            Debug.Log($"[ScenarioEventExecutor] Executing sequential event: {eventData.eventType}");
            
            if (eventData.delay > 0)
            {
                Debug.Log($"[ScenarioEventExecutor] Waiting {eventData.delay} seconds before event");
                yield return new WaitForSeconds(eventData.delay);
            }

            Debug.Log($"[ScenarioEventExecutor] Starting execution of event: {eventData.eventType}");
            yield return StartCoroutine(scenarioEvent.Execute()); 
            Debug.Log($"[ScenarioEventExecutor] Event execution completed: {eventData.eventType}");

            if (eventData.waitForCompletion)
            {
                Debug.Log($"[ScenarioEventExecutor] Waiting for completion of event: {eventData.eventType}");
                yield return new WaitUntil(() => scenarioEvent.IsCompleted);
                Debug.Log($"[ScenarioEventExecutor] Event completion confirmed: {eventData.eventType}, IsCompleted: {scenarioEvent.IsCompleted}");
            }
        }
        
        Debug.Log($"[ScenarioEventExecutor] All events completed, calling OnEndScenarioEvents");
        EventMaster.current.OnEndScenarioEvents();
        Debug.Log($"[ScenarioEventExecutor] OnEndScenarioEvents called");
    }

    private IEnumerator ExecuteParallelEvents(List<IScenarioEvent> events)
    {
        Debug.Log($"[ScenarioEventExecutor] ExecuteParallelEvents started with {events.Count} events");
        
        // Запускаем все события одновременно
        var coroutines = new List<Coroutine>();
        
        foreach (var scenarioEvent in events)
        {
            var eventData = scenarioEvent.EventData;
            Debug.Log($"[ScenarioEventExecutor] Starting parallel event: {eventData.eventType}");
            
            if (eventData.delay > 0)
            {
                Debug.Log($"[ScenarioEventExecutor] Waiting {eventData.delay} seconds before parallel event: {eventData.eventType}");
                yield return new WaitForSeconds(eventData.delay);
            }
            
            coroutines.Add(StartCoroutine(scenarioEvent.Execute()));
            Debug.Log($"[ScenarioEventExecutor] Parallel event coroutine started: {eventData.eventType}");
        }

        Debug.Log($"[ScenarioEventExecutor] Waiting for {coroutines.Count} parallel event coroutines to complete");
        // Ждем завершения всех событий
        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
        
        Debug.Log($"[ScenarioEventExecutor] All parallel events completed");
    }
} 
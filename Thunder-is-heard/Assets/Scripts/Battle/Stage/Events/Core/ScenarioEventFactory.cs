using System;
using UnityEngine;
using System.Collections.Generic;

public static class ScenarioEventFactory
{
    private static Dictionary<string, Type> eventTypes = new Dictionary<string, Type>()
    {
        { "UnitAttack", typeof(UnitAttackEvent) },
        { "MultiUnitAttack", typeof(MultiUnitAttackEvent) },
        { "UnitMove", typeof(UnitMoveEvent) },
        { "UnitRotate", typeof(UnitRotateEvent) },
        { "UnitDeath", typeof(UnitDeathEvent) },
        { "MultiUnitDeath", typeof(MultiUnitDeathEvent) },
        { "Wait", typeof(WaitEvent) },
    };

    public static IScenarioEvent CreateEvent(ScenarioEventData eventData)
    {
        if (eventData == null) return null;
        
        if (!eventTypes.ContainsKey(eventData.eventType))
        {
            Debug.LogError($"Unknown event type: {eventData.eventType}");
            return null;
        }

        Type eventType = eventTypes[eventData.eventType];
        IScenarioEvent scenarioEvent = (IScenarioEvent)Activator.CreateInstance(eventType, eventData);
        return scenarioEvent;
    }

    public static void RegisterEventType(string eventType, Type eventClass)
    {
        if (eventTypes.ContainsKey(eventType))
        {
            Debug.LogWarning($"Event type {eventType} already registered, overwriting...");
        }
        eventTypes[eventType] = eventClass;
    }
} 
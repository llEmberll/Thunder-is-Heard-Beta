using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableUnitProductionsBehaviour : ISubsituableUnitProductionsBehaviour
{
    public ResourcesProcessor resourcesProcessor;



    public void Init()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }


    public List<UnitProductionItem> GetItems(UnitProductions conductor)
    {
        return conductor.items;
    }

    public bool IsAvailableToBuy(UnitProductionItem item)
    {
        return resourcesProcessor.IsAvailableToBuy(item.costData);
    }

    public void OnBuy(UnitProductionItem item)
    {
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(item._duration);

        ProcessWorker.CreateProcess(
            item.Type,
            ProcessTypes.component,
            item._sourceObjectId,
            startTime,
            endTime,
            new ProcessSource(item.Type, item._id)
            );
        resourcesProcessor.SubstractResources(item.costData);
        resourcesProcessor.Save();
    }
}

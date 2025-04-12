using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableContractsBehaviour : ISubsituableContractsBehaviour
{
    public ResourcesProcessor resourcesProcessor;



    public virtual void Init(Contracts conductor)
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
        FillContent(conductor);
    }


    public List<ContractItem> GetItems(Contracts conductor)
    {
        return conductor.items;
    }

    public bool IsAvailableToBuy(ContractItem item)
    {
        return resourcesProcessor.IsAvailableToBuy(item.costData);
    }

    public void OnBuy(ContractItem item)
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

    public virtual void Toggle(Contracts conductor)
    {
        if (conductor.gameObject.activeSelf)
        {
            conductor.Hide();
        }
        else
        {
            conductor.Show();
        }
    }

    public virtual void FillContent(Contracts conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ContractItem>();

        ContractCacheTable contractTable = Cache.LoadByType<ContractCacheTable>();
        foreach (var keyValuePair in contractTable.Items)
        {
            ContractCacheItem contractData = new ContractCacheItem(keyValuePair.Value.Fields);
            if (contractData.GetType() != conductor._contractType)
            {
                continue;
            }

            conductor.items.Add(conductor.CreateContractItem(contractData));
        }
    }
}

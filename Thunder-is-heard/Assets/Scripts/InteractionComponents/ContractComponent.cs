using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class ContractComponent : InteractionComponent
{
    public Contracts _conductor;

    public override void Init(string objectOnBaseId, string componentType)
    {
        base.Init(objectOnBaseId, componentType);

        _conductor = Resources.FindObjectsOfTypeAll(typeof(Contracts)).First().GetComponent<Contracts>();
    }


    public override void Finished()
    {
        _conductor.Finished(this);
    }

    public override void HandleFinishedProcess(ProcessOnBaseCacheItem processCacheItem)
    {
        if (processCacheItem.GetSource() == null)
        {
            throw new System.NotImplementedException("Process source for contract component not found");
        }

        string sourceType = processCacheItem.GetSource().type;
        string sourceId = processCacheItem.GetSource().id;

        if (sourceType != "Contract")
        {
            throw new System.NotImplementedException("Process source for contract component must be type of Contract, but is not");
        }

        ContractCacheTable sourceTable = Cache.LoadByType<ContractCacheTable>();
        CacheItem sourceItem = sourceTable.GetById(sourceId);
        ContractCacheItem contractItem = new ContractCacheItem(sourceItem.Fields);
        ResourcesData gives = contractItem.GetGives();

        Dictionary<string, string> resourceData = ResourcesProcessor.GetFirstNotEmptyResourceData(gives);
        string resourceIconSection = Config.resources["resourcesIcons"];
        string resourceIconName = resourceData["name"];
        int resourceCount = int.Parse(resourceData["count"]);

        ObjectProcessor.CreateProductsNotification(
            id, 
            ProductsNotificationTypes.waitingResourceCollection, 
            resourceIconSection, 
            resourceIconName, 
            resourceCount, 
            gives
            );
    }

    public override void Idle()
    {
        _conductor.Idle(this);
    }

    public override void Working()
    {
        _conductor.Working(this);
    }

    public override void ToggleUI()
    {
        _conductor.Toggle();
    }

    public override void HideUI()
    {
        _conductor.Hide();
    }
}

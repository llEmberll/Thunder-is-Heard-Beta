using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class ContractComponent : InteractionComponent
{
    public Contracts _contractsUI;

    public override void Init(string objectOnBaseId, string componentType)
    {
        base.Init(objectOnBaseId, componentType);

        _contractsUI = Resources.FindObjectsOfTypeAll(typeof(Contracts)).First().GetComponent<Contracts>();
    }


    public override void Finished()
    {
        ProductsNotificationCacheTable productsNotificationCacheTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem productsCollectionData = productsNotificationCacheTable.FindBySourceObjectId(id);
        if (productsCollectionData == null )
        {
            throw new System.NotImplementedException("Contract component waiting for collection, but collectionData not found");
        }

        if (resourceProcessor.IsAvailableToAddResources(productsCollectionData.GetGives()))
        {
            resourceProcessor.AddResources(productsCollectionData.GetGives());
            resourceProcessor.Save();
            ObjectProcessor.DeleteProductsNotificationByItemId(productsCollectionData.GetExternalId());
            ObjectProcessor.CreateProductsNotification(id, ProductsNotificationTypes.idle);
            EventMaster.current.OnChangeObjectOnBaseWorkStatus(id, WorkStatuses.idle);
        }

        else
        {
            Debug.Log("Сбор невозможен");

        }
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
        ToggleUI();
        _contractsUI.Init(type, id);
    }

    public override void Working()
    {
        //Показать оставшееся время выполнения
        Debug.Log("working...");
    }

    public override void ToggleUI()
    {
        _contractsUI.Toggle();
    }

    public override void HideUI()
    {
        _contractsUI.Hide();
    }
}

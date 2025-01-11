using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitProductionComponent : InteractionComponent
{
    public UnitProductions _unitProductionsUI;

    public override void Init(string objectOnBaseId, string componentType)
    {
        base.Init(objectOnBaseId, componentType);

        _unitProductionsUI = Resources.FindObjectsOfTypeAll(typeof(UnitProductions)).First().GetComponent<UnitProductions>();
    }

    public override void Finished()
    {
        ProductsNotificationCacheTable productsNotificationCacheTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem productsCollectionData = productsNotificationCacheTable.FindBySourceObjectId(id);
        if (productsCollectionData == null)
        {
            throw new System.NotImplementedException("Unit production component waiting for collection, but collectionData not found");
        }


        ObjectProcessor.AddUnitToInventory(productsCollectionData.GetUnitId());
        ObjectProcessor.DeleteProductsNotificationByItemId(productsCollectionData.GetExternalId());
        ObjectProcessor.CreateProductsNotification(id, ProductsNotificationTypes.idle);
        EventMaster.current.OnChangeObjectOnBaseWorkStatus(id, WorkStatuses.idle);
        
    }

    public override void HandleFinishedProcess(ProcessOnBaseCacheItem processCacheItem)
    {
        if (processCacheItem.GetSource() == null)
        {
            throw new System.NotImplementedException("Process source for unit production component not found");
        }

        string sourceType = processCacheItem.GetSource().type;
        string sourceId = processCacheItem.GetSource().id;

        if (sourceType != "UnitProduction")
        {
            throw new System.NotImplementedException("Process source for unit production component must be type of UnitProduction, but is not");
        }

        UnitProductionCacheTable sourceTable = Cache.LoadByType<UnitProductionCacheTable>();
        CacheItem sourceItem = sourceTable.GetById(sourceId);
        UnitProductionCacheItem unitProductionItem = new UnitProductionCacheItem(sourceItem.Fields);
        string unitId = unitProductionItem.GetUnitId();

        string unitIconSection;
        if (Config.resources.ContainsKey(unitProductionItem.GetIconSection())) {
            unitIconSection = Config.resources[unitProductionItem.GetIconSection()];
        }
        else
        {
             unitIconSection = unitProductionItem.GetIconSection();
        }
        string unitIconName = unitProductionItem.GetIconName();
        int unitCount = 1;

        ObjectProcessor.CreateProductsNotification(
            id,
            ProductsNotificationTypes.waitingUnitCollection,
            unitIconSection,
            unitIconName,
            unitCount,
            unitId: unitId
            );
    }

    public override void Idle()
    {
        ToggleUI();
        _unitProductionsUI.Init(type, id);
    }

    public override void Working()
    {
        //Показать оставшееся время выполнения
        Debug.Log("working...");
    }

    public override void HideUI()
    {
        _unitProductionsUI.Hide();
    }

    public override void ToggleUI()
    {
        _unitProductionsUI.Toggle();
    }
}

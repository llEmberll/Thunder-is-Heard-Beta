using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitProductionComponent : InteractionComponent
{
    public UnitProductions _conductor;

    public override void Init(string objectOnBaseId, string componentType)
    {
        base.Init(objectOnBaseId, componentType);

        _conductor = Resources.FindObjectsOfTypeAll(typeof(UnitProductions)).First().GetComponent<UnitProductions>();
    }

    public override void Finished()
    {
        _conductor.Finished(this);
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
        _conductor.Idle(this);
    }

    public override void Working()
    {
        _conductor.Working(this);
    }

    public override void HideUI()
    {
        _conductor.Hide();
    }

    public override void ToggleUI()
    {
        _conductor.Toggle();
    }
}

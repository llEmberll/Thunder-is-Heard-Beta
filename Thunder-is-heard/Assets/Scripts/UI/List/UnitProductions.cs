using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitProductions : ItemList
{
    public List<UnitProductionItem> items;
    public Transform content;

    public string _unitProductionType;
    public string _sourceObjectId;

    public override void Start()
    {
        InitContent();
    }

    public void Init(string contractType, string sourceObjectId)
    {
        _unitProductionType = contractType; _sourceObjectId = sourceObjectId;
        FillContent();
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.unitProductionItems).transform;
    }

    public override void FillContent()
    {
        ClearItems();
        UnitProductionCacheTable unitProductionTable = Cache.LoadByType<UnitProductionCacheTable>();
        foreach (var keyValuePair in unitProductionTable.Items)
        {
            UnitProductionCacheItem unitProductionData = new UnitProductionCacheItem(keyValuePair.Value.Fields);
            if (unitProductionData.GetType() != _unitProductionType)
            {
                continue;
            }

            items.Add(CreateUnitProductionItem(unitProductionData));
        }
    }

    public UnitProductionItem CreateUnitProductionItem(UnitProductionCacheItem unitProductionData)
    {
        string unitId = unitProductionData.GetUnitId();

        CacheItem coreUnitAsCacheItem = Cache.LoadByType<UnitCacheTable>().GetById(unitId);
        UnitCacheItem coreUnitData = new UnitCacheItem(coreUnitAsCacheItem.Fields);
        int unitHealth = coreUnitData.GetHealth();
        int unitDamage = coreUnitData.GetDamage();
        int unitDistance = coreUnitData.GetDistance();
        int unitMobility = coreUnitData.GetMobility();

        string id = unitProductionData.GetExternalId();
        string name = unitProductionData.GetName();
        ResourcesData cost = unitProductionData.GetCost();
        int duration = unitProductionData.GetDuration();
        string description = unitProductionData.GetDescription();

        Sprite icon = null;
        Sprite[] iconSection = Resources.LoadAll<Sprite>(Config.resources[unitProductionData.GetIconSection()]);
        if (iconSection != null) 
        {
            icon = SpriteUtils.FindSpriteByName(unitProductionData.GetIconName(), iconSection);
        }

        GameObject itemObject = CreateObject(Config.resources["UI" + "UnitProductionItemPrefab"], content);
        itemObject.name = name;
        UnitProductionItem unitProductionComponent = itemObject.GetComponent<UnitProductionItem>();

        unitProductionComponent.Init(
            id, 
            name, 
            _unitProductionType, 
            _sourceObjectId,
            duration, 
            cost, 
            unitId, 
            unitHealth, 
            unitDamage, 
            unitDistance, 
            unitMobility, 
            description, 
            icon
            );
        return unitProductionComponent;
    }

    public void ClearItems()
    {
        GameObject[] children = content.gameObject.GetComponentsInChildren<Transform>(true)
            .Where(obj => obj != content)
            .Select(obj => obj.gameObject)
            .ToArray();

        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        items = new List<UnitProductionItem>();
    }
}

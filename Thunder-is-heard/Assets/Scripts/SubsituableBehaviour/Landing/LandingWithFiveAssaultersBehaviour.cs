using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class SimpleInventoryItem
{
    public string externalId;
    public string coreId;
    public string type;
    public int count;

    public SimpleInventoryItem() { }

    public SimpleInventoryItem(InventoryCacheItem item)
    {
        externalId = item.GetExternalId();
        coreId = item.GetCoreId();
        type = item.GetType();
        count = item.GetCount();
    }

    public InventoryCacheItem ToInventoryCacheItem()
    {
        Dictionary<string, object> fields = new Dictionary<string, object>
        {
            { "externalId", externalId },
            { "coreId", coreId },
            { "type", type },
            { "count", count }
        };
        return new InventoryCacheItem(fields);
    }
}

public class LandingWithFiveAssaultersBehaviour : BaseSubsituableLandingBehaviour
{
    private string keyForStorageLandableUnitsData = "customLandableUnitsData";

    public override void OnPressedToBattleButton(Landing conductor)
    {
        if (conductor._landedStaff >= 10)
        {
            EventMaster.current.ContinueFight();
            conductor.FinishLanding();
        }
        else
        {
            conductor.notLandedStaffForFightWarning.gameObject.SetActive(true);
        }
    }

    public List<InventoryCacheItem> GenerateItems()
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(battleTable.GetById(FightSceneLoader.parameters._battleId).Fields);
        
        Dictionary<string, object> customData = battleData.GetCustomData();
        if (customData.ContainsKey(keyForStorageLandableUnitsData))
        {
            // Просто десериализуем как SimpleInventoryItem[]
            SimpleInventoryItem[] simpleItems = battleData.GetCustomDataValue<SimpleInventoryItem[]>(keyForStorageLandableUnitsData);
            return simpleItems.Select(item => item.ToInventoryCacheItem()).ToList();
        }

        // Если данных нет, создаем начальные
        InventoryCacheItem assaulter = new InventoryCacheItem(new Dictionary<string, object>());
        assaulter.SetExternalId("121ba759-fa9b-4f8b-add7-925569107cc4");
        assaulter.SetCount(5);
        assaulter.SetCoreId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");
        assaulter.SetType("Unit");

        SimpleInventoryItem[] landableUnitsData = new SimpleInventoryItem[] { new SimpleInventoryItem(assaulter) };

        customData = new Dictionary<string, object>() { { keyForStorageLandableUnitsData, landableUnitsData } };
        battleData.SetCustomData(customData);
        battleTable.ChangeById(battleData.GetExternalId(), battleData);
        Cache.Save(battleTable);
        return new List<InventoryCacheItem>() { assaulter };
    }

    public override void FillContent(Landing conductor)
    {
        Debug.Log("Landing With Five Assaulters: FillContent...");

        conductor.ClearItems();
        conductor.items = new List<LandableUnit>();

        List<InventoryCacheItem> items = GenerateItems();
        foreach (InventoryCacheItem inventoryItemData in items)
        {
            string type = inventoryItemData.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(inventoryItemData.GetCoreId());

            switch (type)
            {
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    LandableUnit unit = CreateUnit(conductor, inventoryItemData, unitData);
                    conductor.items.Add(unit);
                    break;
            }
        }
    }

    public override void Substract(Landing conductor, InventoryItem item, int number = 1) 
    {
        item.UpdateCount(item._count - number);

        // Обновляем данные в битве
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(battleTable.GetById(FightSceneLoader.parameters._battleId).Fields);
        
        Dictionary<string, object> customData = battleData.GetCustomData();
        if (customData.ContainsKey(keyForStorageLandableUnitsData))
        {
            SimpleInventoryItem[] simpleItems = battleData.GetCustomDataValue<SimpleInventoryItem[]>(keyForStorageLandableUnitsData);
            
            // Находим и обновляем соответствующий элемент
            foreach (var simpleItem in simpleItems)
            {
                if (simpleItem.externalId == item._id)
                {
                    simpleItem.count -= number;
                    break;
                }
            }
            
            // Сохраняем обновленные данные
            customData[keyForStorageLandableUnitsData] = simpleItems;
            battleData.SetCustomData(customData);
            battleTable.ChangeById(battleData.GetExternalId(), battleData);
            Cache.Save(battleTable);
        }
    }

    public override void Increment(Landing conductor, InventoryItem item, int number = 1)
    {
        base.Increment(conductor, item, number);

        // Обновляем данные в битве
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(battleTable.GetById(FightSceneLoader.parameters._battleId).Fields);
        
        Dictionary<string, object> customData = battleData.GetCustomData();
        if (customData.ContainsKey(keyForStorageLandableUnitsData))
        {
            SimpleInventoryItem[] simpleItems = battleData.GetCustomDataValue<SimpleInventoryItem[]>(keyForStorageLandableUnitsData);
            
            // Находим и обновляем соответствующий элемент
            foreach (var simpleItem in simpleItems)
            {
                if (simpleItem.externalId == item._id)
                {
                    simpleItem.count += number;
                    break;
                }
            }
            
            // Сохраняем обновленные данные
            customData[keyForStorageLandableUnitsData] = simpleItems;
            battleData.SetCustomData(customData);
            battleTable.ChangeById(battleData.GetExternalId(), battleData);
            Cache.Save(battleTable);
        }
    }
}

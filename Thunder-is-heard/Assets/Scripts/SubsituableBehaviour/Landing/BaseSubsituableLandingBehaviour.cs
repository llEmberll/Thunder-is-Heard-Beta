using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseSubsituableLandingBehaviour : ISubsituableLandingBehaviour
{
    public virtual void Init(Landing conductor)
    {
        conductor.FillContent();
    }

    public virtual void OnLandableUnitFocus(Landing conductor, LandableUnit target)
    {
        conductor.TmpHealth.text = target.health.ToString();
        conductor.TmpDamage.text = target.damage.ToString();
        conductor.TmpDistance.text = target.distance.ToString();
        conductor.TmpMobility.text = target.mobility.ToString();
    }

    public virtual void OnLandableUnitDefocus(Landing conductor, LandableUnit target)
    {
        conductor.InitReadings();
    }

    

    public virtual void OnObjectLanded(Landing conductor, Entity entity)
    {
        if (!conductor.IsProperType(entity.Type)) return;
        if (entity.side != Sides.federation) return;
        conductor.ChangeLandedStaff(conductor._landedStaff + Unit.GetStaffByUnit(entity.gameObject.GetComponent<Unit>()));
        conductor.UpdateStaffText();
    }

    public virtual void OnObjectRemoved(Landing conductor, Entity entity)
    {
        if (!conductor.IsProperType(entity.Type)) return;
        conductor.ChangeLandedStaff(conductor._landedStaff - Unit.GetStaffByUnit(entity.gameObject.GetComponent<Unit>()));
        conductor.UpdateStaffText();
    }

    public virtual void OnPressedToBattleButton(Landing conductor)
    {
        if (conductor._landedStaff > 0)
        {
            EventMaster.current.ContinueFight();
            conductor.FinishLanding();
        }
        else
        {
            conductor.notLandedStaffForFightWarning.gameObject.SetActive(true);
        }
    }

    public virtual void StartLanding(Landing conductor, LandingData landingData)
    {
        Debug.Log("StartLanding from DISABLED BEHAVIOUR");

        conductor._maxStaff = landingData.maxStaff;
        conductor.UpdateLandedStaff();
        conductor.UpdateStaffText();
        
        List<Vector2Int> landableZoneAsVectors = Bector2Int.MassiveToVector2Int(landingData.zone).ToList();
        conductor._map.SetInactiveAll();
        conductor._map.SetActive(landableZoneAsVectors);
        conductor._map.Display(landableZoneAsVectors);

        conductor.Show();
    }

    public virtual void FinishLanding(Landing conductor)
    {
        Debug.Log("Landing: finish landing");

        conductor.notLandedStaffForFightWarning.gameObject.SetActive(false);
        conductor._map.SetActiveAll();
        conductor._map.HideAll();

        conductor.ChangeBehaviour("Disabled");
    }

    public virtual List<LandableUnit> GetItems(Landing conductor)
    {
        return conductor.items;
    }

    public virtual void FillContent(Landing conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<LandableUnit>();

        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        foreach (var keyValuePair in inventoryTable.Items)
        {
            InventoryCacheItem inventoryItemData = new InventoryCacheItem(keyValuePair.Value.Fields);
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

    public virtual LandableUnit CreateUnit(Landing conductor, InventoryCacheItem inventoryItemData, UnitCacheItem unitData)
    {
        string id = inventoryItemData.GetExternalId();
        string coreId = inventoryItemData.GetCoreId();
        string name = unitData.GetName();
        int staff = unitData.GetGives().staff;
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = inventoryItemData.GetCount();
        Sprite icon = ResourcesUtils.LoadIcon(unitData.GetLandingIconSection(), unitData.GetLandingIconName());

        GameObject itemObject = Landing.CreateObject(Config.resources["UI" + "Unit" + "LandablePrefab"], conductor.content);
        itemObject.name = name;
        LandableUnit unitComponent = itemObject.GetComponent<LandableUnit>();

        unitComponent.Init(
            id,
            coreId,
            name,
            staff,
            health,
            damage,
            distance,
            mobility,
            count,
            icon
            );
        unitComponent.SetConductor(conductor);
        return unitComponent;
    }

    public void OnUse(Landing conductor, InventoryItem item)
    {
        if (item is LandableUnit landableUnit)
        {
            landableUnit.CreatePreview();

            EventMaster.current.OnBuildMode();
            EventMaster.current.ToggledOffBuildMode += landableUnit.OnCancelExposing;
            EventMaster.current.ObjectExposed += landableUnit.OnObjectExposed;
        }
        else
        {
            Debug.LogError($"Uncorrect landable unit: {item._id}");
        }
    }

    public virtual void CreatePreview(Landing conductor, ExposableInventoryItem item)
    {
        CacheTable itemsTable = Cache.LoadByName(item.Type);

        CacheItem needleItemData = itemsTable.GetById(item.coreId);
        if (needleItemData == null)
        {
            Debug.Log("CreatePreview | Can't find item by id: " + item.coreId);
            item.Finish();
            return;
        }

        string modelPath = (string)needleItemData.GetField("modelPath") + "/" + Tags.federation;
        Vector2Int size = item.GetSize(needleItemData).ToVector2Int();
        Transform model = ObjectProcessor.CreateModel(modelPath, 0).transform;

        ObjectPreview preview = ObjectPreview.Create();
        preview.Init(item._objName, item.Type, item.coreId, size, model);
    }

    public virtual void OnObjectExposed(Landing conductor, ExposableInventoryItem item, Entity obj)
    {
        if (obj.CoreId == item.coreId && obj.Type.Contains(item.Type))
        {
            if (item._count < 2)
            {
                item.Finish();
            }

            item.Substract();
        }

        else
        {
            item.Continue();
        }
    }

    public virtual void Substract(Landing conductor, InventoryItem item, int number = 1) 
    {
        InventoryCacheTable inventoryItemsTable = Cache.LoadByType<InventoryCacheTable>();
        CacheItem cacheItem = inventoryItemsTable.GetById(item._id);
        InventoryCacheItem inventoryItem = new InventoryCacheItem(cacheItem.Fields);
        inventoryItem.SetCount(inventoryItem.GetCount() - 1);
        if (inventoryItem.GetCount() < 1)
        {
            inventoryItemsTable.Delete(new CacheItem[1] { cacheItem });
        }
        else
        {
            cacheItem.fields = inventoryItem.Fields;
        }

        Cache.Save(inventoryItemsTable);

        item.UpdateCount(item._count - number);
    }

    public virtual void OnInventoryItemAdded(Landing conductor, InventoryItem sourceItem, InventoryCacheItem addedItem)
    {
        if (sourceItem.Type.Contains(addedItem.GetType()) && addedItem.GetCoreId() == sourceItem.coreId)
        {
            Increment(conductor, sourceItem);
        }
    }

    public virtual void Increment(Landing conductor, InventoryItem item, int number = 1)
    {
        item.Increment(number);
    }
}

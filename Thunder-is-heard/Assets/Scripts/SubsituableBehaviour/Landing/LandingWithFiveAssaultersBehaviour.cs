using System.Collections.Generic;

public class LandingWithFiveAssaultersBehaviour : BaseSubsituableLandingBehaviour
{

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
        InventoryCacheItem assaulter = new InventoryCacheItem(new Dictionary<string, object>());
        assaulter.SetExternalId("121ba759-fa9b-4f8b-add7-925569107cc4");
        assaulter.SetCount(5);
        assaulter.SetCoreId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");
        assaulter.SetType("Unit");

        return new List<InventoryCacheItem>() { assaulter };
    }

    public override void FillContent(Landing conductor)
    {
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
    }
}

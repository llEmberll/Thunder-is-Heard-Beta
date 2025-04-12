
public class AllUnitsCollectedCondition : BasicCondition
{
    public bool collected = false;


    public AllUnitsCollectedCondition()
    {
        collected = IsAllUnitsCollected();

        if (!collected)
        {
            EnableListeners();
        }
    }

    public bool IsAllUnitsCollected()
    {
        ProductsNotificationCacheTable productsNotificationTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        foreach (CacheItem item in productsNotificationTable.Items.Values)
        {
            ProductsNotificationCacheItem productsNotificationData = new ProductsNotificationCacheItem(item.Fields);
            if (productsNotificationData.GetType() == ProductsNotificationTypes.waitingUnitCollection)
            {
                return false;
            }
        }
        return true;
    }

    public void EnableListeners()
    {
        EventMaster.current.UnitCollected += SomeUnitCollected;
    }

    public void DisableListeners()
    {
        EventMaster.current.UnitCollected -= SomeUnitCollected;
    }

    public void SomeUnitCollected(ProductsNotificationCacheItem productsNotificationData)
    {
        if (!IsAllUnitsCollected()) return;

        collected = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        return collected;
    }
}

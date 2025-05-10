
public class AllResourcesCollectedCondition : BasicCondition
{
    public bool firstCheck = true;

    public bool collected = false;


    public AllResourcesCollectedCondition()
    {
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        collected = IsAllResourcesCollected();

        if (!collected)
        {
            EnableListeners();
        }
    }

    public bool IsAllResourcesCollected()
    {
        ProductsNotificationCacheTable productsNotificationTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        foreach (CacheItem item in productsNotificationTable.Items.Values)
        {
            ProductsNotificationCacheItem productsNotificationData = new ProductsNotificationCacheItem(item.Fields);
            if (productsNotificationData.GetType() == ProductsNotificationTypes.waitingResourceCollection)
            {
                return false;
            }
        }
        return true;
    }

    public void EnableListeners()
    {
        EventMaster.current.ProductsCollected += SomeProductsCollected;
    }

    public void DisableListeners()
    {
        EventMaster.current.ProductsCollected -= SomeProductsCollected;
    }

    public void SomeProductsCollected(ProductsNotificationCacheItem productsNotificationData)
    {
        if (!IsAllResourcesCollected()) return;

        collected = true;
        DisableListeners();
    }


    public override bool IsComply()
    {
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        return collected;
    }
}

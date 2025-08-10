
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

        if (!collected && _isActive)
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

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        else if (!collected)
        {
            // Если уже проверяли и ресурсы не собраны, подписываемся на события
            EnableListeners();
        }
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        firstCheck = true;
        collected = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        if (firstCheck && _isActive)
        {
            FirstComplyCheck();
        }
        return collected;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}

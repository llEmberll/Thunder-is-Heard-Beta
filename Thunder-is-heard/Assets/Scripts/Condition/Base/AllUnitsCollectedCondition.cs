
public class AllUnitsCollectedCondition : BasicCondition
{
    public bool firstCheck = true;

    public bool collected = false;


    public AllUnitsCollectedCondition()
    {
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        collected = IsAllUnitsCollected();

        if (!collected && _isActive)
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

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        else if (!collected)
        {
            // Если уже проверяли и юниты не собраны, подписываемся на события
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

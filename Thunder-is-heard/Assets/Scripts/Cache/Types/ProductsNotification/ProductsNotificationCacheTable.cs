


[System.Serializable]
public class ProductsNotificationCacheTable : CacheTable
{
    public string name = "ProductsNotification";

    public override string Name { get { return name; } }

    public ProductsNotificationCacheItem FindBySourceObjectId(string sourceObjectId)
    {
        foreach (var keyValuePair in this.Items) 
        {
            ProductsNotificationCacheItem currentItem = new ProductsNotificationCacheItem(keyValuePair.Value.Fields);
            if (currentItem.GetSourceObjectId() == sourceObjectId)
            {
                return currentItem;
            }
        }

        return null;
    }

    public ProductsNotificationCacheItem FindByType(string type)
    {
        foreach (var keyValuePair in this.Items)
        {
            ProductsNotificationCacheItem currentItem = new ProductsNotificationCacheItem(keyValuePair.Value.Fields);
            if (currentItem.GetType() == type)
            {
                return currentItem;
            }
        }

        return null;
    }
}

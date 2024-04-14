using System;

public class ShopCacheTable : CacheTable
{
    public string name = "Shop";

    public override string Name { get { return name; } }

    public override void Add(CacheItem[] newItems)
    {
        foreach (var item in newItems)
        {
            string coreId = (string)item.GetField("coreId");
            CacheItem itemWithSameCoreId = GetByCoreId(coreId);
            if (itemWithSameCoreId == null)
            {
                base.AddOne(item);
            }

            else
            {
                IncreaseCount(itemWithSameCoreId, (int)item.GetField("count"));
            }
        }
    }

    public void IncreaseCount(CacheItem item, int addentCount)
    {
        object oldValue = item.GetField("count");
        int oldCount = oldValue != null ? Convert.ToInt32(oldValue) : 1;
        int newCount = addentCount + oldCount;
        item.SetField("count", newCount);
    }
}

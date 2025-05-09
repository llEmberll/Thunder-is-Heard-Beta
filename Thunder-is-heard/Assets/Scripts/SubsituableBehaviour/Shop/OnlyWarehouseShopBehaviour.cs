using System.Collections.Generic;

public class OnlyWarehouseShopBehaviour : BaseSubsituableShopBehaviour
{

    public override void FillContent(Shop conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ShopItem> { };

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem cacheItem = buildsTable.GetById("3d9f0f22-409e-40d7-8511-f4584b583dc0");
        BuildCacheItem buildData = new BuildCacheItem(cacheItem.Fields);

        ShopCacheItem shopItemData = new ShopCacheItem(new Dictionary<string, object>());
        shopItemData.SetExternalId("2e897b7e-9705-4400-9b39-ae163e548df1");
        shopItemData.SetCount(1);
        shopItemData.SetCoreId(buildData.GetExternalId());
        shopItemData.SetType("Build");

        BuildShopItem build = conductor.CreateBuild(shopItemData, buildData);
        conductor.items.Add(build);
    }
}

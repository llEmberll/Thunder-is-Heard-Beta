using System.Collections.Generic;

public class OnlyTentShopBehaviour : BaseSubsituableShopBehaviour
{

    public override void FillContent(Shop conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ShopItem> { };

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem cacheItem = buildsTable.GetById("ba290dde-968d-46ab-868b-b0f7598a7787");
        BuildCacheItem buildData = new BuildCacheItem(cacheItem.Fields);

        ShopCacheItem shopItemData = new ShopCacheItem(new Dictionary<string, object>());
        shopItemData.SetExternalId("f7743220-decc-4c48-8fff-aff0bb37350e");
        shopItemData.SetCount(1);
        shopItemData.SetCoreId(buildData.GetExternalId());
        shopItemData.SetType("Build");

        BuildShopItem build = conductor.CreateBuild(shopItemData, buildData);
        conductor.items.Add(build);
    }
}

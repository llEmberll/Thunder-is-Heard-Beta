using System.Collections.Generic;

public class OnlyTrainingCenterShopBehaviour : BaseSubsituableShopBehaviour
{

    public override void FillContent(Shop conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ShopItem> { };

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem cacheItem = buildsTable.GetById("065a4f61-5b0e-450e-b89b-299651f90b4d");
        BuildCacheItem buildData = new BuildCacheItem(cacheItem.Fields);

        ShopCacheItem shopItemData = new ShopCacheItem(new Dictionary<string, object>());
        shopItemData.SetExternalId("55190fdb-4971-40dc-92aa-9c7cea55b4e7");
        shopItemData.SetCount(1);
        shopItemData.SetCoreId(buildData.GetExternalId());
        shopItemData.SetType("Build");

        BuildShopItem build = conductor.CreateBuild(shopItemData, buildData);
        conductor.items.Add(build);
    }
}

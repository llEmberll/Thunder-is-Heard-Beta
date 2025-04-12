using System.Collections.Generic;

public class OnlyOfficeShopBehaviour : BaseSubsituableShopBehaviour
{

    public override void FillContent(Shop conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ShopItem> { };

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem cacheItem = buildsTable.GetById("8878498b-a4bc-4dc8-8f39-bc9e987a689f");
        BuildCacheItem buildData = new BuildCacheItem(cacheItem.Fields);

        ShopCacheItem shopItemData = new ShopCacheItem(new Dictionary<string, object>());
        shopItemData.SetExternalId("d27c5a8f-a863-4167-bc8d-d3c41695bcea");
        shopItemData.SetCount(1);
        shopItemData.SetCoreId(buildData.GetExternalId());
        shopItemData.SetType("Build");

        BuildShopItem build = conductor.CreateBuild(shopItemData, buildData);
        conductor.items.Add(build);
    }
}

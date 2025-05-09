using System.Collections.Generic;

public class OnlyLaboratoryShopBehaviour : BaseSubsituableShopBehaviour
{

    public override void FillContent(Shop conductor)
    {
        conductor.ClearItems();
        conductor.items = new List<ShopItem> { };

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem cacheItem = buildsTable.GetById("f4465aab-c10e-4d7a-a1f7-78d419c50f24");
        BuildCacheItem buildData = new BuildCacheItem(cacheItem.Fields);

        ShopCacheItem shopItemData = new ShopCacheItem(new Dictionary<string, object>());
        shopItemData.SetExternalId("822aaefa-772f-4a43-aadd-adb5f45a2d7d");
        shopItemData.SetCount(1);
        shopItemData.SetCoreId(buildData.GetExternalId());
        shopItemData.SetType("Build");

        BuildShopItem build = conductor.CreateBuild(shopItemData, buildData);
        conductor.items.Add(build);
    }
}

using System.Collections;
using System.Collections.Generic;

public class BuildInventoryItem: ExposableInventoryItem
{
    public static string type = "Build";
    public override string Type { get { return type; } }


    public override void SaveExpose(Bector2Int[] occypation, int rotation)
    {
        PlayerBuildCacheItem exposedBuildData = new PlayerBuildCacheItem(new Dictionary<string, object>());
        exposedBuildData.SetCoreId(id);
        exposedBuildData.SetName(name);
        exposedBuildData.SetPosition(occypation);
        exposedBuildData.SetRotation(rotation);

        PlayerBuildCacheTable exposedBuilds = Cache.LoadByType<PlayerBuildCacheTable>();
        exposedBuilds.Add(new CacheItem[1] { exposedBuildData });
        Cache.Save(exposedBuilds);
    }
}

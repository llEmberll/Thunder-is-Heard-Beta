using System.Collections.Generic;
using UnityEngine;

public class BuildsOnBase : ItemList, IObjectsOnScene
{
    public Map map;

    public override void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<Map>();
        base.Start();
    }

    public override void OnBuildModeEnable()
    {
    }

    public override void FillContent()
    {
        base.FillContent();

        PlayerBuildCacheTable playerBuildsTable = Cache.LoadByType<PlayerBuildCacheTable>();
        foreach (var pair in playerBuildsTable.Items)
        {
            PlayerBuildCacheItem currentPlayerBuild = new PlayerBuildCacheItem(pair.Value.Fields);
            MappingBuild(currentPlayerBuild);
        }
    }

    private void MappingBuild(PlayerBuildCacheItem playerBuildData)
    {
        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem buildAsCacheItem = buildsTable.GetById(playerBuildData.GetCoreId());
        BuildCacheItem coreBuildData = new BuildCacheItem(buildAsCacheItem.Fields);
        string modelPath = coreBuildData.GetModelPath();
        Bector2Int size = coreBuildData.GetSize();

        string name = playerBuildData.GetName();
        Bector2Int[] position = playerBuildData.GetPosition();
        int rotation = playerBuildData.GetRotation();


        GameObject buildObj = CreateBuildObject(position, name, this.transform);
        GameObject buildModel = CreateBuildModel(modelPath, rotation, buildObj.transform);

        OccypyCells(position);
        SetModelOffsetByRotation(buildModel.transform, size, rotation);
    }

    private GameObject CreateBuildModel(string modelPath, int rotation, Transform parent)
    {
        GameObject buildModelPrefab = Resources.Load<GameObject>(modelPath);
        return Instantiate(
            buildModelPrefab, buildModelPrefab.transform.position + parent.transform.position,
            Quaternion.Euler(new Vector3(0, rotation, 0)),
            parent
            );
    }

    private GameObject CreateBuildObject(Bector2Int[] position, string name, Transform parent)
    {
        var buildPrefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        GameObject obj = Instantiate(
            buildPrefab,
            new Vector3(position[0].x, 0, position[0].y),
            Quaternion.identity,
            parent
            );
        obj.name = name;
        return obj;
    }

    private void OccypyCells(Bector2Int[] position)
    {
        foreach (Bector2Int pos in position)
        {
            var cell = map.cells[new Vector2Int(pos.x, pos.y)];
            cell.Occupy();
        }
    }

    public void SetModelOffsetByRotation(Transform model, Bector2Int size, int rotation)
    {
        if (rotation == 0 || rotation == 360 || rotation == 180) return;

        Bector2Int swappedSizeB = new Bector2Int(GetSwappedSize(size.x, size.y));
        float sizeDiff = ((float)swappedSizeB.x - (float)swappedSizeB.y) / 2;

        Vector3 offset = new Vector3(sizeDiff, 0, -1 * sizeDiff);
        model.position += offset;
    }

    public Vector2Int GetSwappedSize(int x, int y)
    {
        x = y + x;
        y = x - y;
        x -= y;

        return new Vector2Int(x, y);
    }
}

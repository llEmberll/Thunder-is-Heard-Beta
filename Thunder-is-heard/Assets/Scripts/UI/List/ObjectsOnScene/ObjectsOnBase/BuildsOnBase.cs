using System.Collections.Generic;
using UnityEngine;

public class BuildsOnBase : ObjectsOnBase
{
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
        string id = playerBuildData.GetCoreId();

        GameObject buildObj = CreateBuildObject(position[0].ToVector2Int(), name, this.transform);
        GameObject buildModel = CreateBuildModel(modelPath, rotation, buildObj.transform);

        OccypyCells(position);
        SetModelOffsetByRotation(buildModel.transform, size, rotation);

        AddAndPrepareBuildComponent(buildObj, buildModel.transform, id, size.ToVector2Int(), Bector2Int.MassiveToVector2Int(position));
    }

    public static GameObject CreateBuildModel(string modelPath, int rotation, Transform parent)
    {
        GameObject buildModelPrefab = Resources.Load<GameObject>(modelPath);

        Debug.Log("create build, rotation: " + rotation);

        GameObject buildModel = Instantiate(
            buildModelPrefab, buildModelPrefab.transform.position + parent.transform.position,
            Quaternion.Euler(new Vector3(0, rotation, 0)),
            parent
            );
        buildModel.name = "Model";
        return buildModel;
    }

    public static GameObject CreateBuildObject(Vector2Int position, string name, Transform parent)
    {
        var buildPrefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        GameObject obj = Instantiate(
            buildPrefab,
            new Vector3(position.x, 0, position.y),
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

    public override Entity FindObjectById(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Build childEntity = transform.GetChild(i).GetComponent<Build>();
            if (childEntity != null && childEntity.Id == id)
            {
                return childEntity;
            }
        }
        return null;
    }

    public static void AddAndPrepareBuildComponent(GameObject buildObj, Transform model, string id, Vector2Int size, Vector2Int[] occypation)
    {
        Build component = buildObj.AddComponent<Build>();
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.id = id;
        
    }
}

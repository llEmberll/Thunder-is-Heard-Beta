using System.Collections.Generic;
using System.Linq;
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
        int health = coreBuildData.GetHealth();
        int damage = coreBuildData.GetDamage();
        int distance = coreBuildData.GetDistance();
        string interactionComponentName = coreBuildData.GetInteractionComponentName();
        string interactionComponentType = coreBuildData.GetInteractionComponentType();

        string name = playerBuildData.GetName();
        Bector2Int[] position = playerBuildData.GetPosition();
        int rotation = playerBuildData.GetRotation();
        string coreId = playerBuildData.GetCoreId();
        string childId = playerBuildData.GetExternalId();
        string workStatus = playerBuildData.GetWorkStatus();

        GameObject buildObj = ObjectProcessor.CreateBuildObject(position[0].ToVector2Int(), name, this.transform);
        GameObject buildModel = CreateBuildModel(modelPath, rotation, buildObj.transform);

        map.Occypy(Bector2Int.MassiveToVector2Int(position).ToList());
        SetModelOffsetByRotation(buildModel.transform, size, rotation);

        ObjectProcessor.AddAndPrepareBuildComponent(
            buildObj, 
            buildModel.transform, 
            coreId, 
            childId, 
            name, 
            size.ToVector2Int(), 
            Bector2Int.MassiveToVector2Int(position), 
            health, 
            damage, 
            distance, 
            Config.sides["ally"], 
            interactionComponentName, 
            interactionComponentType,
            workStatus
            );
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
            if (childEntity != null && childEntity.CoreId == id)
            {
                return childEntity;
            }
        }
        return null;
    }
}

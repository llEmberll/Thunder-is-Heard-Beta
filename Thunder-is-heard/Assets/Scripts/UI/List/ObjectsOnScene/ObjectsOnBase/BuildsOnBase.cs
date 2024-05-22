using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildsOnBase : ObjectsOnBase
{
    public Transform productsNotificationsBucket;

    public override void Start()
    {
        productsNotificationsBucket = GameObject.FindGameObjectWithTag(Tags.productsNotifications).transform;
        base.Start();

        EventMaster.current.ProductsNotificationCreated += PutOnProductsNotification;
    }

    public override void OnBuildModeEnable()
    {
    }

    public override void FillContent()
    {
        base.FillContent();

        FillBuilds();

        FillProductsNotifcations();
    }

    public void FillBuilds()
    {
        PlayerBuildCacheTable playerBuildsTable = Cache.LoadByType<PlayerBuildCacheTable>();
        foreach (var pair in playerBuildsTable.Items)
        {
            PlayerBuildCacheItem currentPlayerBuild = new PlayerBuildCacheItem(pair.Value.Fields);
            MappingBuild(currentPlayerBuild);
        }
    }

    public void FillProductsNotifcations()
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        foreach (var pair in productsNotificationsTable.Items)
        {
            ProductsNotificationCacheItem currentNotificationItem = new ProductsNotificationCacheItem(pair.Value.Fields);
            PutOnProductsNotification(currentNotificationItem);
        }
    }

    private void MappingProductsNotification(ProductsNotificationCacheItem productsNotificationData, PlayerBuildCacheItem sourceBuildData)
    {
        List<Vector2Int> buildPosition = Bector2Int.MassiveToVector2Int(sourceBuildData.GetPosition()).ToList();
        Vector2Int buildCenter = Entity.CalculateCenter(buildPosition);

        Sprite[] iconSection = Resources.LoadAll<Sprite>(productsNotificationData.GetIconSection());
        Sprite icon = null;
        if (iconSection.Length == 1) 
        { 
            icon = iconSection[0];
        }
        else if (productsNotificationData.GetIconName() != "")
        {
            foreach (Sprite i in iconSection)
            {
                if (i.name == productsNotificationData.GetIconName())
                {
                    icon = i;
                    break;
                }
            }
        }

        GameObject productsNotificationObj = ObjectProcessor.CreateProductsNotificationObject(buildCenter, sourceBuildData.GetName() + "ProductsNotification", productsNotificationsBucket);
        ObjectProcessor.AddAndPrepareProductsNotificationComponent(
            productsNotificationObj,
            productsNotificationData.GetSourceObjectId(),
            productsNotificationData.GetType(),
            icon,
            productsNotificationData.GetCount(),
            productsNotificationData.GetGives()
            );
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

    public void PutOnProductsNotification(ProductsNotificationCacheItem productsNotification)
    {
        PlayerBuildCacheTable buildsOnBaseTable = Cache.LoadByType<PlayerBuildCacheTable>();
        CacheItem currentBuildItem = buildsOnBaseTable.GetById(productsNotification.GetSourceObjectId());
        if (currentBuildItem == null)
        {
            Debug.Log("Build for finded productsNotification not found: " + productsNotification.GetSourceObjectId());
            return;
        }

        PlayerBuildCacheItem currentBuildData = new PlayerBuildCacheItem(currentBuildItem.Fields);

        MappingProductsNotification(productsNotification, currentBuildData);
    }
}

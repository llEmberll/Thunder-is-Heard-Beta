using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildsOnBase : ObjectsOnBase
{
    [SerializeField] public Dictionary<string, Build> items = new Dictionary<string, Build>();

    public Transform productsNotificationsBucket;

    public override void Awake()
    {
        
    }

    public override void Start()
    {
        this.content = this.transform;

        productsNotificationsBucket = GameObject.FindGameObjectWithTag(Tags.productsNotifications).transform;
        base.Start();
    }

    public override void EnableListeners()
    {
        EventMaster.current.ProductsNotificationCreated += PutOnProductsNotification;
        EventMaster.current.BaseObjectRemoved += OnBaseObjectRemoved;
        EventMaster.current.ObjectExposed += OnBaseObjectExposed;
    }

    public override void DisableListeners()
    {
        EventMaster.current.ProductsNotificationCreated -= PutOnProductsNotification;
        EventMaster.current.BaseObjectRemoved -= OnBaseObjectRemoved;
        EventMaster.current.ObjectExposed -= OnBaseObjectExposed;
    }

    public void OnBaseObjectRemoved(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (!items.ContainsKey(obj.ChildId)) return;
        items.Remove(obj.ChildId);
        Destroy(obj.gameObject);
        
    }

    public void OnBaseObjectExposed(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (items.ContainsKey(obj.ChildId)) return;

        if (obj == null) return;
        items.Add(obj.ChildId, obj.gameObject.GetComponent<Build>());
    }

    public override bool IsProperType(string type)
    {
        return type.Contains("Build");
    }

    public void UpdateObjects()
    {
        Debug.Log("Update build objects");

        FillContent();
    }

    public override void FillContent()
    {
        FillBuilds();

        FillProductsNotifcations();
    }

    public void FillBuilds()
    {
        ClearItems();
        items = new Dictionary<string, Build>();

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

        Sprite[] iconSection = new Sprite[] {};
        try
        {
            iconSection = Resources.LoadAll<Sprite>(productsNotificationData.GetIconSection());
        }
        catch (System.Exception e)
        {

            Debug.Log("Load icon error! section = " + productsNotificationData.GetIconSection() + "| icon name = " + productsNotificationData.GetIconName());
        }
            
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
        string modelPath = coreBuildData.GetModelPath() + "/" + Tags.federation;
        Bector2Int size = coreBuildData.GetSize();
        int health = coreBuildData.GetHealth();
        int damage = coreBuildData.GetDamage();
        int distance = coreBuildData.GetDistance();
        string doctrine = coreBuildData.GetDoctrine();
        string interactionComponentName = coreBuildData.GetInteractionComponentName();
        string interactionComponentType = coreBuildData.GetInteractionComponentType();

        string name = playerBuildData.GetName();
        Bector2Int[] position = playerBuildData.GetPosition();
        int rotation = playerBuildData.GetRotation();
        string coreId = playerBuildData.GetCoreId();
        string childId = playerBuildData.GetExternalId();
        string workStatus = playerBuildData.GetWorkStatus();

        GameObject buildObj = ObjectProcessor.CreateEntityObject(position[0].ToVector2Int(), name, this.transform);
        GameObject buildModel = ObjectProcessor.CreateModel(modelPath, rotation, buildObj.transform);

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
            health,
            damage, 
            distance, 
            doctrine,
            Sides.federation, 
            interactionComponentName, 
            interactionComponentType,
            workStatus
            );

        items.Add(childId, buildObj.GetComponent<Build>());
    }

    public void SetModelOffsetByRotation(Transform model, Bector2Int size, int rotation)
    {
        if (rotation == 0 || rotation == 360 || rotation == 180) return;

        Bector2Int swappedSizeB = new Bector2Int(GetSwappedSize(size._x, size._y));
        float sizeDiff = ((float)swappedSizeB._x - (float)swappedSizeB._y) / 2;

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

    public override Entity FindObjectByCoreId(string id)
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

    public override Entity FindObjectByChildId(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Build childEntity = transform.GetChild(i).GetComponent<Build>();
            if (childEntity != null && childEntity.ChildId == id)
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ObjectProcessor : MonoBehaviour
{
    public Map map;

    public void Start()
    {
        map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();

        EventMaster.current.ObjectOnBaseWorkStatusChanged += ChangeObjectOnBaseWorkStatus;
    }

    public static void PutSelectedObjectOnBaseToInventory()
    {
        ObjectPreview preview = FindObjectOfType<ObjectPreview>();
        if (IsSomeObjectSelected(preview))
        {
            Debug.Log("selected object putted to inv");

            AddGameObjectToInventory(preview.buildedObjectOnScene);
            RemoveObjectFromBase(preview.buildedObjectOnScene);
            preview.buildedObjectOnScene = null;
            preview.Cancel();
        }
    }

    public static bool IsSomeObjectSelected(ObjectPreview preview)
    {
        if (preview == null) 
        {
            Debug.Log("No preview => INVALID INPUT");
            return false;
        }

        if (preview.buildedObjectOnScene == null)
        {
            Debug.Log("No buildedObjectOnScene => INVALID INPUT");
            return false;
        }

        return true;
    }

    public static void AddToInventory(InventoryCacheItem newItem)
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();

        CacheItem[] itemsForAdd = new CacheItem[1] { newItem };
        inventory.Add(itemsForAdd);
        Cache.Save(inventory);

        EventMaster.current.OnChangeInventory();
    }

    public static void AddGameObjectToInventory(GameObject obj)
    {
        CacheItem coreObjectData = Cache.GetBaseObjectCoreData(obj);
        if (coreObjectData == null)
        {
            Debug.Log("No coreObjectData => INVALID INPUT");
            return;
        }

        Entity entity = obj.GetComponent<Entity>();

        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", coreObjectData.GetExternalId() },
            { "type", entity.Type },
            { "count", 1 }
        }
        );

        AddToInventory(newItem);
    }

    public static void AddUnitToInventory(string unitId, int count = 1)
    {
        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", unitId },
            { "type", "Unit" },
            { "count", count }
        }
        );

        AddToInventory(newItem);
    }

    public static  void RemoveObjectFromBase(GameObject obj)
    {
        CacheItem objectData = Cache.GetBaseObjectData(obj);
        if (objectData == null)
        {
            Debug.Log("No BaseObjectData => INVALID INPUT");
            return;
        }

        Entity entity = obj.GetComponent<Entity>();

        CacheTable baseObjectsTable = Cache.LoadByName("Player" + entity.Type);
        baseObjectsTable.DeleteById(objectData.GetExternalId());
        Cache.Save(baseObjectsTable);
        Destroy(obj);

        ProductsNotificationCacheTable notificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem notificationByObject = notificationsTable.FindBySourceObjectId(entity.ChildId);
        if (notificationByObject != null)
        {
            DeleteProductsNotificationByItemId(notificationByObject.GetExternalId());
        }

        EventMaster.current.OnRemoveBaseObject(entity.coreId, entity.Type);
        EventMaster.current.OnChangeBaseObjects();
    }

    public void CreateObjectOnBase(string coreId, string type, Transform model, string objName, Vector2Int size, List<Vector2Int> occypation)
    {
        Vector2Int rootPoint = occypation.First();
        ObjectsOnBase objectsPool = GameObject.FindWithTag(Config.exposableObjectsTypeToObjectsOnSceneTag[type]).GetComponent<ObjectsOnBase>();

        Transform entity = null;
        if (type.Contains("Build"))
        {
            BuildCacheTable coreBuildDatas = Cache.LoadByType<BuildCacheTable>();
            CacheItem coreItemData = coreBuildDatas.GetById(coreId);
            BuildCacheItem coreBuildData = new BuildCacheItem(coreItemData.Fields);
            int health = coreBuildData.GetHealth();
            int damage = coreBuildData.GetDamage();
            int distance = coreBuildData.GetDistance();
            string interactionComponentName = coreBuildData.GetInteractionComponentName();
            string interactionComponentType = coreBuildData.GetInteractionComponentType();

            entity = CreateBuildObject(rootPoint, objName, objectsPool.transform).transform;

            PlayerBuildCacheItem playerBuildCacheItem = AddNewBuildOnBaseToCache(
                coreId, 
                objName, 
                Bector2Int.GetVector2IntListAsBector(occypation), 
                Entity.GetDeterminedRotationByModel(model),
                WorkStatuses.idle
                );

            AddAndPrepareBuildComponent(
                entity.gameObject, 
                model, 
                coreId, 
                playerBuildCacheItem.GetExternalId(), 
                objName, 
                size, 
                occypation.ToArray(), 
                health, 
                damage, 
                distance, 
                Sides.federation, 
                interactionComponentName,
                interactionComponentType,
                WorkStatuses.idle
                );
        }

        else if (type.Contains("Unit"))
        {
            UnitCacheTable coreUnitDatas = Cache.LoadByType<UnitCacheTable>();
            CacheItem coreItemData = coreUnitDatas.GetById(coreId);
            UnitCacheItem coreUnitData = new UnitCacheItem(coreItemData.Fields);
            int health = coreUnitData.GetHealth();
            int damage = coreUnitData.GetDamage();
            int distance = coreUnitData.GetDistance();
            int mobility = coreUnitData.GetMobility();

            entity = CreateUnitObject(rootPoint, objName, objectsPool.transform).transform;

            PlayerUnitCacheItem playerUnitCacheItem = AddNewUnitOnBaseToCache(
                coreId, 
                objName, 
                Bector2Int.GetVector2IntListAsBector(occypation), 
                Entity.GetDeterminedRotationByModel(model)
                );

            AddAndPrepareUnitComponent(
                entity.gameObject, 
                model, 
                coreId, 
                playerUnitCacheItem.GetExternalId(), 
                objName, 
                size,
                occypation.ToArray(), 
                health, 
                damage, 
                distance, 
                mobility, 
                Sides.federation
                );
        }

        else
        {
            throw new System.Exception("Неожиданный тип объекта: " + type);
        }

        model.SetParent(entity);

        map.Occypy(occypation);
        EventMaster.current.ExposeObject(
            coreId, 
            type,
            Bector2Int.GetVector2IntListAsBector(occypation), 
            entity.GetComponent<Entity>().rotation
            );

        EventMaster.current.OnChangeBaseObjects();
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

    public static GameObject CreateUnitObject(Vector2Int position, string name, Transform parent)
    {
        var unitPrefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        GameObject obj = Instantiate(
            unitPrefab,
            new Vector3(position.x, 0, position.y),
            Quaternion.identity,
            parent
            );
        obj.name = name;
        return obj;
    }

    public static GameObject CreateProductsNotificationObject(Vector2Int position, string name, Transform parent)
    {
        var productsNotificationPrefab = Resources.Load<GameObject>(Config.resources["UIProductsNotificationPrefab"]);
        GameObject obj = Instantiate(
            productsNotificationPrefab,
            new Vector3(position.x, productsNotificationPrefab.transform.position.y, position.y),
            productsNotificationPrefab.transform.rotation,
            parent
            );
        obj.name = name;
        return obj;
    }

    public void ReplaceObjectOnBase(GameObject obj, List<Vector2Int> newPosition, int newRotation)
    {
        Entity entity = obj.GetComponent<Entity>();

        obj.transform.position = new Vector3(newPosition.First().x, 0, newPosition.First().y);
        entity.model.SetParent(entity.transform);

        map.Free(entity.occypiedPoses);
        entity.SetOccypation(newPosition);
        map.Occypy(newPosition);

        CacheItem baseObjectData = Cache.GetBaseObjectData(obj);

        CacheTable baseObjectsTable = Cache.LoadByName("Player" + entity.Type);
        baseObjectData.SetField("position", Bector2Int.GetVector2IntListAsBector(newPosition));
        baseObjectData.SetField("rotation", newRotation);
        baseObjectsTable.Items[baseObjectData.GetExternalId()] = baseObjectData;
        Cache.Save(baseObjectsTable);

        EventMaster.current.OnReplaceBaseObject(entity);
    }

    public static void AddAndPrepareBuildComponent(
        GameObject buildObj, 
        Transform model, 
        string coreId, 
        string childId,
        string objName, 
        Vector2Int size, 
        Vector2Int[] occypation,
        int health, 
        int damage, 
        int distance, 
        string side, 
        string interactionComponentName,
        string interactionComponentType,
        string workStatus
        )
    {
        Build component = buildObj.AddComponent<Build>();
        component.SetName(objName);
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.SetAttributes(health, damage, distance, 0);
        component.SetSide(side);
        component.coreId = coreId;
        component.childId = childId;

        component.interactionComponent = InteractionComponentFactory.GetComponentById(interactionComponentName);
        component.interactionComponent.Init(childId, interactionComponentType);
        component.ChangeWorkStatus(workStatus);

        if (interactionComponentType != "" && IsObjectIdleAndNotHaveProductsNotification(component))
        {
            CreateProductsNotification(component.ChildId, ProductsNotificationTypes.idle);
        }
    }

    public static void AddAndPrepareUnitComponent(
        GameObject unitObj, 
        Transform model, 
        string coreId, 
        string childId,
        string objName, 
        Vector2Int size, 
        Vector2Int[] occypation,
        int health, 
        int damage,
        int distance,
        int mobility, 
        string side
        )
    {
        Unit component = unitObj.AddComponent<Unit>();
        component.SetName(objName);
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.SetAttributes(health, damage, distance, mobility);
        component.SetSide(side);

        component.coreId = coreId;
        component.childId = childId;
    }

    public static void AddAndPrepareProductsNotificationComponent(
        GameObject productsNotificationObj,
        string sourceObjectId,
        string type,
        Sprite icon, 
        int productionCount,
        ResourcesData gives
        )
    {
        ProductsNotifcation component = productsNotificationObj.GetComponent<ProductsNotifcation>();
        component.Init(sourceObjectId, type, icon, productionCount, gives);
    }

    public static PlayerBuildCacheItem AddNewBuildOnBaseToCache(string buildId, string name, Bector2Int[] occypation, int rotation, string workStatus)
    {
        PlayerBuildCacheItem exposedBuildData = new PlayerBuildCacheItem(new Dictionary<string, object>());
        exposedBuildData.SetCoreId(buildId);
        exposedBuildData.SetName(name);
        exposedBuildData.SetPosition(occypation);
        exposedBuildData.SetRotation(rotation);
        exposedBuildData.SetWorkStatus(workStatus);

        PlayerBuildCacheTable exposedBuilds = Cache.LoadByType<PlayerBuildCacheTable>();
        exposedBuilds.Add(new CacheItem[1] { exposedBuildData });
        Cache.Save(exposedBuilds);

        return exposedBuildData;
    }

    public static PlayerUnitCacheItem AddNewUnitOnBaseToCache(string unitId, string name, Bector2Int[] occypation, int rotation)
    {
        PlayerUnitCacheItem exposedUnitData = new PlayerUnitCacheItem(new Dictionary<string, object>());
        exposedUnitData.SetCoreId(unitId);
        exposedUnitData.SetName(name);
        exposedUnitData.SetPosition(occypation);
        exposedUnitData.SetRotation(rotation);

        PlayerUnitCacheTable exposedUnits = Cache.LoadByType<PlayerUnitCacheTable>();
        exposedUnits.Add(new CacheItem[1] { exposedUnitData });
        Cache.Save(exposedUnits);

        return exposedUnitData;
    }

    public static void OnBuyMaterial(string materialId)
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", materialId },
            { "type", "Material" },
            { "count", 1 }
        }
        );

        CacheItem[] itemsForAdd = new CacheItem[1] { newItem };
        inventory.Add(itemsForAdd);
        Cache.Save(inventory);

        EventMaster.current.OnChangeInventory();
    }

    public void ChangeObjectOnBaseWorkStatus(string objectOnBaseId, string newStatus)
    {
        PlayerBuildCacheTable buildsOnBaseTable = Cache.LoadByType<PlayerBuildCacheTable>();
        CacheItem buildOnBaseCacheItem = buildsOnBaseTable.GetById(objectOnBaseId);
        PlayerBuildCacheItem buildOnBase = new PlayerBuildCacheItem(buildOnBaseCacheItem.Fields);

        buildOnBase.SetWorkStatus(newStatus);
        buildsOnBaseTable.AddOne(buildOnBase);

        Cache.Save(buildsOnBaseTable);
    }

    public static bool IsObjectIdleAndNotHaveProductsNotification(Build build)
    {
        if (build.workStatus != WorkStatuses.idle)
        {
            return false;
        }

        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        return productsNotificationsTable.FindBySourceObjectId(build.ChildId) == null;
    }

    public static void CreateProductsNotification(
        string sourceObjectId,
        string notificationType, 
        string iconSection = "",
        string iconName = "",
        int count = 1, 
        ResourcesData gives = null, 
        string unitId = null
        )
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();

        ProductsNotificationCacheItem newProductsNotification = new ProductsNotificationCacheItem(new Dictionary<string, object>());
        newProductsNotification.SetSourceObjectId(sourceObjectId);
        newProductsNotification.SetType(notificationType);
        newProductsNotification.SetIconSection(iconSection);
        newProductsNotification.SetIconName(iconName);
        newProductsNotification.SetCount(count);
        if (gives != null) newProductsNotification.SetGives(gives);
        newProductsNotification.SetUnitId(unitId);

        productsNotificationsTable.AddOne(newProductsNotification);
        Cache.Save(productsNotificationsTable);

        EventMaster.current.OnCreateProductsNotification(newProductsNotification);
    }

    public static void DeleteProductsNotificationBySourceObjectId(string sourceObjectId)
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem itemForDeletion = productsNotificationsTable.FindBySourceObjectId(sourceObjectId);
        if ( itemForDeletion == null ) 
        {
            return;
        }

        productsNotificationsTable.DeleteById(itemForDeletion.GetExternalId());
        Cache.Save(productsNotificationsTable);

        EventMaster.current.OnDeleteProductsNotification(itemForDeletion);
    }

    public static void DeleteProductsNotificationByItemId(string itemId)
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        CacheItem item = productsNotificationsTable.GetById(itemId);
        if ( item == null ) { return; }

        ProductsNotificationCacheItem productsNotificationForDeletion = new ProductsNotificationCacheItem(item.Fields);
        productsNotificationsTable.DeleteById(itemId);
        Cache.Save(productsNotificationsTable);
        EventMaster.current.OnDeleteProductsNotification(productsNotificationForDeletion);
    }
}

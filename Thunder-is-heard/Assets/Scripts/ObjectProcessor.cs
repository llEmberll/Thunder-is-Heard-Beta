using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class ObjectProcessor : MonoBehaviour
{
    public Map map;

    public void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
    }

    public static void PutSelectedObjectOnBaseToInventory()
    {
        ObjectPreview preview = FindObjectOfType<ObjectPreview>();
        if (IsSomeObjectSelected(preview))
        {
            Debug.Log("selected object putted to inv");

            AddObjectToInventory(preview.buildedObjectOnScene);
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

    public static void AddObjectToInventory(GameObject obj)
    {
        CacheItem coreObjectData = Cache.GetBaseObjectCoreData(obj);
        if (coreObjectData == null)
        {
            Debug.Log("No coreObjectData => INVALID INPUT");
            return;
        }

        Entity entity = obj.GetComponent<Entity>();

        int count = 1;
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", coreObjectData.GetExternalId() },
            { "type", entity.Type },
            { "count", count }
        }
        );

        CacheItem[] itemsForAdd = new CacheItem[1] { newItem };
        inventory.Add(itemsForAdd);
        Cache.Save(inventory);

        EventMaster.current.OnChangeInventory();
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

        EventMaster.current.OnRemoveBaseObject(entity.id, entity.Type);
        EventMaster.current.OnChangeBaseObjects();
    }

    public void CreateObjectOnBase(string id, string type, Transform model, string objName, Vector2Int size, List<Vector2Int> occypation)
    {
        Vector2Int rootPoint = occypation.First();
        ObjectsOnBase objectsPool = GameObject.FindWithTag(Config.exposableObjectsTypeToObjectsOnSceneTag[type]).GetComponent<ObjectsOnBase>();

        Transform entity = null;
        if (type.Contains("Build"))
        {
            entity = CreateBuildObject(rootPoint, objName, objectsPool.transform).transform;
            AddAndPrepareBuildComponent(entity.gameObject, model, id, size, occypation.ToArray());
        }

        else if (type.Contains("Unit"))
        {
            entity = CreateUnitObject(rootPoint, objName, objectsPool.transform).transform;
            AddAndPrepareUnitComponent(entity.gameObject, model, id, size, occypation.ToArray());
        }

        else
        {
            throw new System.Exception("Неожиданный тип объекта: " + type);
        }

        model.SetParent(entity);

        map.Occypy(occypation);
        EventMaster.current.ExposeObject(id, type, Bector2Int.GetVector2IntListAsBector(occypation), entity.GetComponent<Entity>().rotation);
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
    }

    public static void AddAndPrepareBuildComponent(GameObject buildObj, Transform model, string id, Vector2Int size, Vector2Int[] occypation)
    {
        Build component = buildObj.AddComponent<Build>();
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.id = id;
    }

    public static void AddAndPrepareUnitComponent(GameObject unitObj, Transform model, string id, Vector2Int size, Vector2Int[] occypation)
    {
        Unit component = unitObj.AddComponent<Unit>();
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));

        component.id = id;
    }

    public static void OnExposedBuild(string buildId, string name, Bector2Int[] occypation, int rotation)
    {
        PlayerBuildCacheItem exposedBuildData = new PlayerBuildCacheItem(new Dictionary<string, object>());
        exposedBuildData.SetCoreId(buildId);
        exposedBuildData.SetName(name);
        exposedBuildData.SetPosition(occypation);
        exposedBuildData.SetRotation(rotation);

        PlayerBuildCacheTable exposedBuilds = Cache.LoadByType<PlayerBuildCacheTable>();
        exposedBuilds.Add(new CacheItem[1] { exposedBuildData });
        Cache.Save(exposedBuilds);
    }

    public static void OnExposedUnit(string unitId, string name, Bector2Int[] occypation, int rotation)
    {
        PlayerUnitCacheItem exposedUnitData = new PlayerUnitCacheItem(new Dictionary<string, object>());
        exposedUnitData.SetCoreId(unitId);
        exposedUnitData.SetName(name);
        exposedUnitData.SetPosition(occypation);
        exposedUnitData.SetRotation(rotation);

        PlayerUnitCacheTable exposedUnits = Cache.LoadByType<PlayerUnitCacheTable>();
        exposedUnits.Add(new CacheItem[1] { exposedUnitData });
        Cache.Save(exposedUnits);
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
}

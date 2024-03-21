using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class ObjectProcessor
    : MonoBehaviour
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

        EventMaster.current.OnEncreaseInventory(entity.id, entity.Type, count);
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
    }

    public void ReplaceObjectOnBase(GameObject obj, List<Vector2Int> newPosition, int newRotation)
    {
        Entity entity = obj.GetComponent<Entity>();
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
}

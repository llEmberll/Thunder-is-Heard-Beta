using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class Cache
{
    public static string fileFormat = ".json";

    public static void Save(CacheTable table)
    {
        string serializedTable = JsonConvert.SerializeObject(table, Formatting.Indented, new JsonSerializerSettings());
        string filePath = Application.streamingAssetsPath + "/" + Config.streamingAssets["cachePath"] + table.Name + fileFormat;

        File.WriteAllText(filePath, serializedTable, Encoding.UTF8);
    }

    public static T LoadByType<T>()
    {
        Type type = typeof(T);
        string tableName = GetCacheFileName(type.ToString());

        string filePath = Application.streamingAssetsPath + "/" + Config.streamingAssets["cachePath"] + tableName + fileFormat;

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
    }

    public static string GetCacheFileName(string cacheName)
    {
        string substringToRemove = "CacheTable";
        return cacheName.Replace(substringToRemove, "");
    }

    public static CacheTable LoadByName(string tableName)
    {
        string filePath = Application.streamingAssetsPath + "/" + Config.streamingAssets["cachePath"] + tableName + fileFormat;

        CacheTable table = JsonConvert.DeserializeObject<CacheTable>(File.ReadAllText(filePath));
        table.name = tableName;

        return JsonConvert.DeserializeObject<CacheTable>(File.ReadAllText(filePath));
    }

    public static CacheItem GetBaseObjectData(GameObject obj)
    {
        Entity entity = obj.GetComponent<Entity>();
        if (entity == null)
        {
            Debug.Log("No entity => INVALID INPUT");
            return null;
        }

        CacheTable table = Cache.LoadByName("Player" + entity.Type);
        return table.GetByCoreId(entity.Id);
    }

    public static CacheItem GetBaseObjectCoreData(GameObject obj)
    {
        Entity entity = obj.GetComponent<Entity>();
        if ( entity == null )
        {
            Debug.Log("GetBaseObjectCoreData: Entity = null");
            return null;
        }

        CacheTable coreObjectsTable = Cache.LoadByName(obj.GetComponent<Entity>().Type);
        return coreObjectsTable.GetById(entity.id);

    }
}

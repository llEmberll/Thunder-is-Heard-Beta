using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Cache
{
    public static string fileFormat = ".json";

    public static void Save(CacheTable table)
    {
        string serializedTable = JsonConvert.SerializeObject(table, Formatting.Indented, new JsonSerializerSettings());
        string filePath = Application.streamingAssetsPath + "/" + Config.streamingAssets["cachePath"] + table.Name + fileFormat;

        Debug.Log("file path =" + filePath);

        File.WriteAllText(filePath, serializedTable, Encoding.UTF8);
    }

    public static T LoadByType<T>()
    {
        Type type = typeof(T);
        string tableName = GetCacheFileName(type.ToString());

        string filePath = Application.streamingAssetsPath + "/" + Config.streamingAssets["cachePath"] + tableName + fileFormat;

        Debug.Log("file path= " + filePath);

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

        Debug.Log("file path= " + filePath);

        return JsonConvert.DeserializeObject<CacheTable>(File.ReadAllText(filePath));
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class CacheTable : ICacheTable
{
    public Dictionary<string, CacheItem> items;
    public string name;

    public CacheTable(List<CacheItem> cacheItems = null) 
    {
        if (cacheItems == null)
        {
            items = new Dictionary<string, CacheItem>();
        }
    }

    [JsonIgnore]
    public virtual string Name { get { return name; } set { name = value; } }
    [JsonIgnore]
    public Dictionary<string, CacheItem> Items { get { return items; } }

    public virtual void Add(CacheItem[] newItems)
    {
        foreach (var item in newItems)
        {
            AddOne(item);
        }
    }

    public virtual void AddOne(CacheItem item)
    {
        string? itemId = item.GetExternalId();
        if (itemId == null)
        {
            itemId = Guid.NewGuid().ToString();
        }

        if (!items.ContainsKey(itemId))
        {
            items.Add(itemId, item);
        }
        else
        {
            items[itemId] = item;
        }
    }

    public virtual void Delete(CacheItem[] itemsForDelete)
    {
        foreach(CacheItem item in itemsForDelete)
        {
            if (Items.ContainsKey(item.GetExternalId()))
            {
                Items.Remove(item.GetExternalId());
            }
        }
    }

    public virtual void DeleteAll()
    {
        items.Clear();
    }

    public virtual CacheItem GetById(string id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }

        return null;
    }

    public virtual void ChangeById(string id, CacheItem item)
    {
        if (items.ContainsKey(id))
        {
            items[id] = item;
        }
    }

    public virtual CacheItem GetByCoreId(string id)
    {
        foreach (var item in items)
        {
            string? coreId = (string?)item.Value.GetField("coreId");
            if (coreId != null && coreId == id) 
            {
                return item.Value;
            }
        }

        return null;
    }

    public virtual void DeleteById(string id)
    {
        if (items.ContainsKey(id)) items.Remove(id);
    }
}

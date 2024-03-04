using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICacheTable
{
    public string Name { get; }
    public Dictionary<string, CacheItem> Items { get; }
    public void DeleteAll();
    public CacheItem GetById(string id);
    public void DeleteById(string id);
    public void Add(CacheItem[] newItems);
}

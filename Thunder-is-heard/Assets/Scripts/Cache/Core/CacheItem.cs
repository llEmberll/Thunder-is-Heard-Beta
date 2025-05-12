using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheItem: ICacheItem
{
    public Dictionary<string, object> fields = new Dictionary<string, object>();


    [JsonIgnore]
    public Dictionary<string, object> Fields { get { return fields; } }


    public CacheItem(Dictionary<string, object> objFields = null)
    {
        if (objFields == null)
        {
            return;
        }

        if (!objFields.ContainsKey("externalId") || objFields["externalId"]  == null) 
        {
            objFields["externalId"] = Guid.NewGuid().ToString();
        }
        fields = objFields;

        if (!objFields.ContainsKey("description"))
        {
            SetDescription("");
        }
    }

    public string? GetName()
    {
        return (string?)GetField("name");
    }

    public void SetName(string value)
    {
        SetField("name", value);
    }

    public string? GetExternalId()
    {
        return (string?)GetField("externalId");
    }

    public void SetExternalId(string value)
    {
        SetField("externalId", value);
    }

    public string? GetDescription()
    {
        return (string?)GetField("description");
    }

    public void SetDescription(string value)
    {
        SetField("description", value);
    }

    public virtual string? GetCoreId()
    {
        return (string?)GetField("coreId");
    }

    public void SetCoreId(string value)
    {
        SetField("coreId", value);
    }

    public virtual CacheItem Clone()
    {
        return new CacheItem(fields);
    }

    public object GetField(string fieldName)
    {
        if (fields.ContainsKey(fieldName))
        {
            return fields[fieldName];
        }

        return null;
    }

    public void SetField(string fieldName, object value)
    {
        if (value == null)
        {
            fields[fieldName] = null;
            return;
        }

        // Автоматическая сериализация для сложных объектов
        if (value.GetType().IsClass && value.GetType() != typeof(string))
        {
            fields[fieldName] = JsonConvert.SerializeObject(value);
        }
        else
        {
            fields[fieldName] = value;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class SkillCacheItem : CacheItem
{
    public SkillCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("iconSection"))
        {
            SetIconSection("");
        }

        if (!objFields.ContainsKey("iconName"))
        {
            SetIconName("");
        }

        if (!objFields.ContainsKey("conditionsForUse"))
        {
            SetConditionsForUse(new string[] { });
        }

        if (!objFields.ContainsKey("cooldown"))
        {
            SetCooldown(0);
        }

        if (!objFields.ContainsKey("targetType"))
        {
            SetTargetType("");
        }

        if (!objFields.ContainsKey("targetType"))
        {
            SetTargetType("");
        }

        if (!objFields.ContainsKey("targetUnitType"))
        {
            SetTargetUnitType("");
        }

        if(!objFields.ContainsKey("targetUnitDoctrine"))
        {
            SetTargetUnitDoctrine("");
        }

        if (!objFields.ContainsKey("rating"))
        {
            SetRating(0f);
        }
    }


    public string? GetIconSection()
    {
        return (string?)GetField("iconSection");
    }

    public void SetIconSection(string value)
    {
        SetField("iconSection", value);
    }

    public string? GetIconName()
    {
        return (string?)GetField("iconName");
    }

    public void SetIconName(string value)
    {
        SetField("iconName", value);
    }

    public string[] GetConditionsForUse()
    {
        object value = GetField("conditionsForUse");
        if (value == null)
        {
            return new string[] { };
        }

        if (value is string[] typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<string[]>(value.ToString());
    }

    public void SetConditionsForUse(string[] value)
    {
        SetField("conditionsForUse", value);
    }


    public int GetCooldown()
    {
        object value = GetField("cooldown");
        return (value != null) ? Convert.ToInt32(value) : 0;
    }

    public void SetCooldown(int value)
    {
        SetField("cooldown", value);
    }

    public string GetTargetType()
    {
        return (string)GetField("targetType");
    }

    public void SetTargetType(string value)
    {
        SetField("targetType", value);
    }

    public string GetTargetUnitType()
    {
        return (string)GetField("targetUnitType");
    }

    public void SetTargetUnitType(string value)
    {
        SetField("targetUnitType", value);
    }

    public string GetTargetUnitDoctrine()
    {
        return (string)GetField("targetUnitDoctrine");
    }

    public void SetTargetUnitDoctrine(string value)
    {
        SetField("targetUnitDoctrine", value);
    }

    public float GetRating()
    {
        object value = GetField("rating");
        return (value != null) ? Convert.ToSingle(value) : 0f;
    }

    public void SetRating(float value)
    {
        SetField("rating", value);
    }


    public override CacheItem Clone()
    {
        UnitCacheItem clone = new UnitCacheItem(fields);
        return clone;
    }
}

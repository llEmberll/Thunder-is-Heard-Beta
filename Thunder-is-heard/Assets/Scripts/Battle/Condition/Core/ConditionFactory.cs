using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionFactory
{
    public static Dictionary<string, Type> conditions = new Dictionary<string, Type>()
    {
        { "DestroyAllAllies", typeof(DestroyAllAllies) },
        { "DestroyAllEnemy", typeof(DestroyAllEnemy) }
    };

    public static ICondition GetConditionById(string id)
    {
        if (conditions.ContainsKey(id))
        {
            Type type = conditions[id];
            return (ICondition)Activator.CreateInstance(type);
        }

        return null;
    }

    public static List<ICondition> GetConditionsByIds(string[] ids)
    {
        List<ICondition> conditions  = new List<ICondition>();
        foreach (string id in ids)
        {
            ICondition condition = GetConditionById(id);
            if (condition != null)
            {
                conditions.Add(condition);
            }
        }

        return conditions;
    }
}

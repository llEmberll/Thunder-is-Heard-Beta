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

    public static BasicCondition GetConditionById(string id)
    {
        if (conditions.ContainsKey(id))
        {
            Type type = conditions[id];
            return (BasicCondition)Activator.CreateInstance(type);
        }

        return null;
    }
}

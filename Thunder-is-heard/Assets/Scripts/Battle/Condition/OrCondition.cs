using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrCondition : BasicCondition
{
    public List<ICondition> _conditions;


    public OrCondition(List<ICondition> conditions)
    {
        _conditions = conditions;
    }

    public override bool IsComply()
    {
        foreach (var condition in _conditions)
        {
            if (condition.IsComply())
            {
                return true;
            }
        }
        return false;
    }
}

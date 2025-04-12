using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AndCondition : BasicCondition
{
    public List<ICondition> _conditions;


    public AndCondition(List<ICondition> conditions)
    {
        _conditions = conditions;
    }

    public override bool IsComply()
    {
        foreach (var condition in _conditions)
        {
            if (!condition.IsComply())
            {
                return false;
            }
        }
        return true;
    }
}

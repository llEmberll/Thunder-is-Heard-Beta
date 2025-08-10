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

    protected override void OnActivate()
    {
        foreach (var condition in _conditions)
        {
            condition.Activate();
        }
    }
    
    protected override void OnDeactivate()
    {
        foreach (var condition in _conditions)
        {
            condition.Deactivate();
        }
    }
    
    protected override void OnReset()
    {
        foreach (var condition in _conditions)
        {
            condition.Reset();
        }
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

    public override bool IsRealTimeUpdate()
    {
        return false;
    }
}

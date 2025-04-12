using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyAllAlliesCondition : BasicCondition
{
    public UnitsOnFight unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();

    public override bool IsComply()
    {
        foreach (var item in unitsOnFight.items)
        {
            if (item.Value.side == Tags.federation)
            {
                return false;
            }
        }

        return true;
    }
}

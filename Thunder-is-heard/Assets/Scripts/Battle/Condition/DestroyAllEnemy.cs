using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllEnemy : BasicCondition
{
    public override bool IsComply()
    {
        foreach (var item in Scenario.Objects)
        {
            if (item.Value.tag == Tags.empire)
            {
                return false;
            }
        }

        return true;
    }
}

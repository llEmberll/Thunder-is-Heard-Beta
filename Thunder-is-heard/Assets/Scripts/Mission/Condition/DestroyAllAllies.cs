using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllAllies : BasicCondition
{
    public override bool IsComply()
    {
        foreach (var item in Scenario.Objects)
        {
            if (item.Value.tag == Config.tags["federation"])
            {
                return false;
            }
        }

        return true;
    }
}

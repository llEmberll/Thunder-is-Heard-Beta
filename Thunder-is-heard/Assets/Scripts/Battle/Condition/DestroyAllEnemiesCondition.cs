using UnityEngine;


public class DestroyAllEnemiesCondition : BasicCondition
{
    public UnitsOnFight unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();


    public override bool IsComply()
    {
        foreach (var item in unitsOnFight.items)
        {
            if (item.Value.side == Tags.empire)
            {
                return false;
            }
        }

        return true;
    }
}

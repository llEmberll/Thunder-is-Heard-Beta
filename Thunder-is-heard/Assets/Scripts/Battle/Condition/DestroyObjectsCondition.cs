using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyObjectsCondition : BasicCondition
{
    public string[] _targetObjectIds;
    public UnitsOnFight _unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
    public BuildsOnFight _buildsOnFight = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnFight>();


    public DestroyObjectsCondition(string[] targetObjectIds)
    {
        _targetObjectIds = targetObjectIds;
    }

    public override bool IsComply()
    {
        foreach (var id in _targetObjectIds)
        {
            if (_unitsOnFight.FindObjectByChildId(id) != null || _buildsOnFight.FindObjectByChildId(id) != null)
            {
                return false;
            }
        }

        return true;
    }
}

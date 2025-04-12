using UnityEngine;

public class ExistObjectCondition: BasicCondition
{
    public string _targetObjectId;
    public bool exist = false;

    public UnitsOnBase unitsOnBase = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnBase>();
    public BuildsOnBase buildsOnBase = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnBase>();

    public ExistObjectCondition(string targetObjectId) 
    { 
        _targetObjectId = targetObjectId;

        exist = IsTargetObjectExist();

        if (!exist)
        {
            EnableListeners();
        }
    }

    public bool IsTargetObjectExist()
    {
        Entity foundedUnit = unitsOnBase.FindObjectByCoreId(_targetObjectId);
        if (foundedUnit != null)
        {
            return true;
        }

        Entity foundedBuild = buildsOnBase.FindObjectByCoreId(_targetObjectId);
        if (foundedBuild != null)
        {
            return true;
        }

        return false;
    }

    public void EnableListeners()
    {
        EventMaster.current.ObjectExposed += SomeObjectExposed;
    }

    public void DisableListeners()
    {
        EventMaster.current.ObjectExposed -= SomeObjectExposed;
    }

    public void SomeObjectExposed(Entity obj)
    {
        if (obj.CoreId== _targetObjectId)
        {
            exist= true;
            DisableListeners();
        }
    }


    public override bool IsComply()
    {
        return exist;
    }
}

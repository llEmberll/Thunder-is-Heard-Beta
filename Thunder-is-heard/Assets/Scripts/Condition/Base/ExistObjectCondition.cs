using UnityEngine;

public class ExistObjectCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetObjectId;
    public bool exist = false;

    public UnitsOnBase unitsOnBase = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnBase>();
    public BuildsOnBase buildsOnBase = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnBase>();

    public ExistObjectCondition(string targetObjectId) 
    { 
        Debug.Log("condition target id = " + targetObjectId);

        _targetObjectId = targetObjectId;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        exist = IsTargetObjectExist();

        Debug.Log("exist = " + exist);

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

        Debug.Log("founded build = " + foundedBuild);

        if (foundedBuild != null)
        {
            return true;
        }

        Debug.Log("target not found");

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
        Debug.Log("is comply = " + exist);

        if (firstCheck)
        {
            FirstComplyCheck();
        }

        return exist;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ObjectCountCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _targetObjectId;
    public int _needCount;
    public bool completed = false;

    public UnitsOnBase unitsOnBase = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnBase>();
    public BuildsOnBase buildsOnBase = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnBase>();

    public ObjectCountCondition(string targetObjectId, int needCount)
    {
        _targetObjectId = targetObjectId;
        _needCount = needCount;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

        completed=IsObjectCountComply();


        if (!completed && _isActive)
        {
            EnableListeners();
        }
    }

    public bool IsObjectCountComply()
    {
        List<Entity> units = unitsOnBase.FindAllObjectsByCoreId(_targetObjectId);
        List<Entity> builds = buildsOnBase.FindAllObjectsByCoreId(_targetObjectId);
        return units.Count + builds.Count >= _needCount;
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
            completed = IsObjectCountComply();
            if (completed)
            {
                DisableListeners();
            }
        }
    }

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (firstCheck)
        {
            FirstComplyCheck();
        }
        if (!completed)
        {
            // Если уже проверяли и объект не найден, подписываемся на события
            EnableListeners();
        }
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        firstCheck = true;
        completed = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        if (firstCheck && _isActive)
        {
            FirstComplyCheck();
        }

        return completed;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}

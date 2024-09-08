using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : Entity, IMovable, IDamageable, IAttack, ITransfer
{
    public Skill[] _skills = null;

    public override string Type {
	get
        {
            return "Unit";
        }
	}


    public void SetSkills(Skill[] skills)
    {
        _skills = skills;
    }
    
    public void Move()
    {
        Debug.Log("Unit moves!");
    }
    
    public void GetDamage()
    {
        Debug.Log("Unit damaged!");
    }
    
    public void Attack()
    {
        Debug.Log("Unit attacks!");
    }

    public void Toggle()
    {
        Debug.Log("Unit toggled!");
    }

    public void Rotate()
    {
        Debug.Log("Unit rotated!");
    }

    public void Replace()
    {
        Debug.Log("Unit replaced!");
    }

    public override void OnFocus()
    {
        base.OnFocus();
        stateMachine.currentState.OnUnitMouseEnter(this);
    }

    public override void OnDefocus()
    {
        base.OnDefocus();
        stateMachine.currentState.OnUnitMouseExit(this);
    }

    public override void OnClick()
    {
        base.OnClick();
        stateMachine.currentState.OnUnitClick(this);
    }

    public static int GetStaffByUnit(Unit unit)
    {
        UnitCacheTable coreUnitTable = Cache.LoadByType<UnitCacheTable>();
        CacheItem cacheItem = coreUnitTable.GetById(unit.CoreId);
        UnitCacheItem unitCacheItem = new UnitCacheItem(cacheItem.Fields);
        ResourcesData gives = unitCacheItem.GetGives();
        return gives.staff;
    }
}

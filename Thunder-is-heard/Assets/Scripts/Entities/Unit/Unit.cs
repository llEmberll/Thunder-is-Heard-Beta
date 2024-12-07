using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Unit : Entity, IMovable, IAttack, ITransfer
{
    public Skill[] _skills = null;

    public string _unitType;

    public bool _onMove = false;
    public Cell _point;
    public List<Cell> _route;

    public float _movementSpeed;

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

    public void SetMovementSpeed(float value)
    {
        _movementSpeed = value;
    }

    private void FixedUpdate()
    {
        if (_onMove == true)
        {
            Vector2Int pointPosition = _point.position;
            if (Vector2.Distance(pointPosition, new Vector2(transform.position.x, transform.position.z)) > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointPosition.x, transform.position.y, pointPosition.y), (_movementSpeed * 2) * Time.fixedDeltaTime);
            }
            else
            {
                PointCompleted();
            }
        }
    }

    private void PointCompleted()
    {
        Cell nextPoint = GetNextPoint();
        if (nextPoint == null)
        {
            FinishMoving();
            return;
        }

        MoveToPoint(nextPoint);
    }

    private Cell GetNextPoint()
    {
        if (_route.Last().position == _point.position)
        {
            return null; 
        }

        int nextPointIndex = _route.IndexOf(_point) + 1;
        return _route[nextPointIndex];
    }

    private void MoveToPoint(Cell cell)
    {
        _point = cell;
        RotateToTarget(_point.position);

        EventMaster.current.OnStartUnitMove(this);
        _onMove = true;
    }

    public void Move(List<Cell> route)
    {
        animator.StartMove();
        _route = route;
        MoveToPoint(route[0]);
    }

    public void FinishMoving()
    {
        animator.FinishMove();
        _onMove = false;
        EventMaster.current.OnFinishUnitMove(this);
    }

    public void ForceFinishMoving()
    {
        FinishMoving();
        Vector2Int finishRoutePosition = _route.Last().position;
        transform.position = new Vector3(finishRoutePosition.x, transform.position.y, finishRoutePosition.y);
    }

    public override void OnDestroy()
    {
        if (_onMove)
        {
            ForceFinishMoving();
        }
        
        base.OnDestroy();
    }

    public void Attack(Entity target)
    {
        if (_onMove)
        {
            ForceFinishMoving();
        }

        RotateToTarget(target.center);
        animator.Attack();
    }

    public void Toggle()
    {
        Debug.Log("Unit toggled!");
    }

    public void Rotate()
    {
        Debug.Log("Unit rotated!");
    }

    public void RotateToTarget(Vector2Int target)
    {
        model.LookAt(new Vector3(target.x, model.position.y, target.y));
        SetRotation(GetDeterminedRotationByModel(model));
    }

    public void Replace()
    {
        Debug.Log("Unit replaced!");
    }

    public void SetUnitType(string value)
    {
        _unitType = value;
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

    public static int GetStaffByUnit(Unit unit) // Перенести куда-то
    {
        UnitCacheTable coreUnitTable = Cache.LoadByType<UnitCacheTable>();
        CacheItem cacheItem = coreUnitTable.GetById(unit.CoreId);
        UnitCacheItem unitCacheItem = new UnitCacheItem(cacheItem.Fields);
        ResourcesData gives = unitCacheItem.GetGives();
        return gives.staff;
    }
}

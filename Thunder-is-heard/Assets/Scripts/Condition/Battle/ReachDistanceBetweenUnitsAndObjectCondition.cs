using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ReachDistanceBetweenUnitsAndObjectCondition : BasicCondition
{
    public int _minDistance;
    public string _targetObjectId;
    public string _unitsSide;
    public BattleEngine _battleEngine;

    public bool _isReached = false;


    public ReachDistanceBetweenUnitsAndObjectCondition(int minDistance, string targetObjectId, string unitsSide) 
    {
        _minDistance = minDistance;
        _targetObjectId = targetObjectId;
        _unitsSide = unitsSide;
        
        InitBattleEngine();
        if (InitialCheck())
        {
            _isReached = true;
            return;
        }
        else
        {
            EnableListeners();
        }
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void EnableListeners()
    {
        EventMaster.current.UnitMoveStarted += OnSomeUnitMove;
    }

    public void DisableListeners()
    {
        EventMaster.current.UnitMoveStarted -= OnSomeUnitMove;
    }

    public bool InitialCheck()
    {
        ObjectOnBattle target = _battleEngine.currentBattleSituation.GetObjectById(_targetObjectId);
        RectangleBector2Int rectangleTargetPosition = new RectangleBector2Int(target.Position);

        ObjectOnBattle nearestUnitBySide = _battleEngine.currentBattleSituation.FindNearestUnitByRectangleAndSide(rectangleTargetPosition, _unitsSide);
        if (nearestUnitBySide == null) return false;

        int nearestDistance = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(nearestUnitBySide.Position.First(), rectangleTargetPosition);
        if (nearestDistance <= _minDistance) return true;
        else return false;

    }

    public void OnSomeUnitMove(Unit unit)
    {
        if (unit.side != _unitsSide)
        {
            return;
        }

        Bector2Int currentUnitPosition = new Bector2Int(unit.occypiedPoses.First());
        ObjectOnBattle target = _battleEngine.currentBattleSituation.GetObjectById(_targetObjectId);
        RectangleBector2Int rectangleTargetPosition = new RectangleBector2Int(target.Position);
        int currentDistanceToTarget = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(currentUnitPosition, rectangleTargetPosition);
        if (currentDistanceToTarget <= _minDistance)
        {
            _isReached = true;
            DisableListeners();
        }
    }

    public override bool IsComply()
    {
        return _isReached;
    }
}

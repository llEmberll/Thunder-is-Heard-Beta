using UnityEngine;


public class NewTargetForAttackCondition : BasicCondition
{
    public string _targetSide;

    public BattleEngine _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();

    public bool _isComplete = false;

    public NewTargetForAttackCondition(string targetSide)
    {
        _targetSide = targetSide;
        EnableListeners();
    }

    public void EnableListeners()
    {
        EventMaster.current.UnitMoveStarted += OnSomeUnitMove;
    }

    public void DisableListeners()
    {
        EventMaster.current.UnitMoveStarted -= OnSomeUnitMove;
    }

    public void OnSomeUnitMove(Unit unit)
    {
        if (unit.side == _targetSide && _battleEngine.currentBattleSituation.GetAttackersByTargetId(unit.childId).Count > 0)
        {
            _isComplete = true;
            DisableListeners();
        }
    }

    public override bool IsComply()
    {
        return _isComplete;
    }
}

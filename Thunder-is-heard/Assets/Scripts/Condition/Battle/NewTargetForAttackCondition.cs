using UnityEngine;


public class NewTargetForAttackCondition : BasicCondition
{
    public string _targetSide;

    public BattleEngine _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();

    public bool _isComplete = false;

    public NewTargetForAttackCondition(string targetSide)
    {
        _targetSide = targetSide;
        // Убираем EnableListeners() из конструктора - теперь это будет в OnActivate
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

    protected override void OnActivate()
    {
        // Подписываемся на события при активации
        EnableListeners();
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        _isComplete = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        return _isComplete;
    }

    public override bool IsRealTimeUpdate()
    {
        return false;
    }
}

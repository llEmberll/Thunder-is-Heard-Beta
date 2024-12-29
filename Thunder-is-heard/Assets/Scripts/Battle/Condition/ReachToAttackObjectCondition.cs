using System.Collections.Generic;
using UnityEngine;


public class ReachToAttackObjectCondition : BasicCondition
{
    public string _targetObjectId;
    public string _attackerSide;
    public BattleEngine _battleEngine;


    public ReachToAttackObjectCondition(string targetObjectId, string attackerSide) 
    { 
        _targetObjectId = targetObjectId;
        _attackerSide = attackerSide;
        InitBattleEngine();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public override bool IsComply()
    {
        List<ObjectOnBattle> attackers = _battleEngine.currentBattleSituation.GetAttackersByTargetId(_targetObjectId);
        if (attackers.Count < 1 ) return false;
        foreach (ObjectOnBattle attacker in attackers)
        {
            if (attacker.side == _attackerSide) return true;
        }

        return false;
    }
}

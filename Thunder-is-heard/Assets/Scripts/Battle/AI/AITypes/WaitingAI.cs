using System.Collections.Generic;


public class WaitingAI : AbstractAI
{
    /// ������������� ���������. ������� ��� �����������. ���� ��� ����, �� ��������

    public override TurnData GetTurn()
    {
        if (_battleEngine.currentBattleSituation.GetUnitsCollectionBySide(_battleEngine.currentBattleSituation._sideTurn).Count < 1) {  return new TurnData(); }

        Dictionary<TurnData, BattleSituation> attackMoves = _battleEngine.currentBattleSituation.GetAllAttackingSequels();
        if (attackMoves.Count > 0)
        {
            return GetBestAttack(_battleEngine.currentBattleSituation, attackMoves);
        }

        Dictionary<string, ObjectOnBattle> objectsUnderAttack = _battleEngine.currentBattleSituation.GetObjectsUnderAttackBySide(_battleEngine.currentBattleSituation._sideTurn);
        Dictionary<TurnData, BattleSituation> movementMoves = _battleEngine.currentBattleSituation.GetAllMovementSequels();
        if (objectsUnderAttack.Count < 1 || movementMoves.Count < 1)
        {
            //���
            return new TurnData();
        }

        return GetBestOffensiveMovementUnderAttack(_battleEngine.currentBattleSituation, movementMoves);
    }
}

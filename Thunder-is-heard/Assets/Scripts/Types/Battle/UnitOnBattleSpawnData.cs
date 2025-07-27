using Newtonsoft.Json;


[System.Serializable]
public class UnitOnBattleSpawnData
{
    public UnitOnBattle unitData;
    public RectangleBector2Int possibleSpawnPositions;

    [JsonIgnore]
    public UnitOnBattle UnitOnBattle { get { return this.unitData; } }

    [JsonIgnore]
    public RectangleBector2Int PossibleSpawnPositions { get { return this.possibleSpawnPositions; } }

    public UnitOnBattleSpawnData() { }

    public UnitOnBattleSpawnData(UnitOnBattle unitData, RectangleBector2Int possibleSpawnPositions = null)
    {
        this.unitData = unitData;
        this.possibleSpawnPositions = possibleSpawnPositions;
    }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        UnitOnBattleSpawnData other = (UnitOnBattleSpawnData)obj;
        return PossibleSpawnPositions.Equals(other.PossibleSpawnPositions) && UnitOnBattle.Equals(other.UnitOnBattle);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + PossibleSpawnPositions.GetHashCode();
            hash = hash * 23 + UnitOnBattle.GetHashCode();
            return hash;
        }
    }
}

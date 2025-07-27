using Newtonsoft.Json;


[System.Serializable]
public class BuildOnBattleSpawnData
{
    public BuildOnBattle buildData;
    public RectangleBector2Int possibleSpawnPositions;

    [JsonIgnore]
    public BuildOnBattle BuildOnBattle { get { return this.buildData; } }

    [JsonIgnore]
    public RectangleBector2Int PossibleSpawnPositions { get { return this.possibleSpawnPositions; } }

    public BuildOnBattleSpawnData() { }

    public BuildOnBattleSpawnData(BuildOnBattle buildData, RectangleBector2Int possibleSpawnPositions = null)
    {
        this.buildData = buildData;
        this.possibleSpawnPositions = possibleSpawnPositions;
    }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        BuildOnBattleSpawnData other = (BuildOnBattleSpawnData)obj;
        return PossibleSpawnPositions.Equals(other.PossibleSpawnPositions) && BuildOnBattle.Equals(other.BuildOnBattle);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + PossibleSpawnPositions.GetHashCode();
            hash = hash * 23 + BuildOnBattle.GetHashCode();
            return hash;
        }
    }
}

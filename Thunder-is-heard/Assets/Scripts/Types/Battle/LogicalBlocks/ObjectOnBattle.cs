using Newtonsoft.Json;

[System.Serializable]
public abstract class ObjectOnBattle : IObjectOnBattle
{
    public string coreId;

    [JsonIgnore]
    public string CoreId { get  { return coreId; } }

    public string idOnBattle;

    [JsonIgnore]
    public string IdOnBattle { get { return idOnBattle; } }

    public Bector2Int[] position;

    [JsonIgnore]
    public Bector2Int[] Position { get { return position; } }

    public int rotation;

    [JsonIgnore]
    public int Rotation { get { return rotation; } }

    public int maxHealth;

    [JsonIgnore]
    public int MaxHealth { get { return maxHealth; } }

    public int health;

    [JsonIgnore]
    public int Health { get { return health; } }

    public int damage;

    [JsonIgnore]
    public int Damage { get { return damage; } }

    public int distance;
    public int Distance { get { return distance; } }

    public string side;

    [JsonIgnore]
    public string Side { get { return side; } }

    public string doctrine;

    [JsonIgnore]
    public string Doctrine {  get { return doctrine; } }



    public ObjectOnBattle() { }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ObjectOnBattle other = (ObjectOnBattle)obj;
        return IdOnBattle == other.IdOnBattle && CoreId == other.CoreId;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + IdOnBattle.GetHashCode();
            hash = hash * 23 + CoreId.GetHashCode();
            return hash;
        }
    }

    public abstract ObjectOnBattle Clone();
}

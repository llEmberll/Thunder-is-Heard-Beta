

public interface IObjectOnBattle
{
    public string CoreId { get; }
    public string IdOnBattle { get; }

    public Bector2Int[] Position { get; }
    public int Rotation { get; }

    public int MaxHealth { get; }
    public int Health { get; }
    public int Damage { get; }
    public int Distance { get; }

    public string Side { get; }

    public string Doctrine { get; }


    public ObjectOnBattle Clone();
}



public class FrozenAI : AbstractAI
{
    public override TurnData GetTurn()
    {
        return new TurnData();
    }
}

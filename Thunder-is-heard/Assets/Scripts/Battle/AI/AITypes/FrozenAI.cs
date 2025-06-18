

public class FrozenAI : AbstractAI
{
    /// Застывшее поведение. Не двигается, не смотря ни на что

    public override TurnData GetTurn()
    {
        return new TurnData();
    }
}

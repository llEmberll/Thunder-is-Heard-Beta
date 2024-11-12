

[System.Serializable]
public class AISettings
{
    public string side;
    public string type;
    public Bector2Int[] targetPositions;
    public string[] targetObjectIds;


    public AISettings() { }

    public AISettings(string AIType, string AISide, Bector2Int[] AITargetPositions, string[] AITargetObjectIds)
    {
        this.type = AIType;
        this.side = AISide;
        this.targetPositions = AITargetPositions;
        this.targetObjectIds = AITargetObjectIds;
    }
}

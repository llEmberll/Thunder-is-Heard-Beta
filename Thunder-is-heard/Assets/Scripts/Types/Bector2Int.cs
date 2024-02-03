using UnityEngine;


[System.Serializable]
public class Bector2Int
{
    public int x, y;

    public Bector2Int(Vector2Int vector)
    {
        x = vector.x; 
        y = vector.y;
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(x, y);
    }
}

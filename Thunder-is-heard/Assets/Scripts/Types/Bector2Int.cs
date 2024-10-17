using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Bector2Int
{
    [JsonProperty("x")]
    public int _x;

    [JsonProperty("y")]
    public int _y;

    public Bector2Int() { }

    public Bector2Int(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public Bector2Int(Vector2Int vector)
    {
        _x = vector.x; 
        _y = vector.y;
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(_x, _y);
    }

    public static Vector2Int[] MassiveToVector2Int(Bector2Int[] massive)
    {
        Vector2Int[] positions = new Vector2Int[massive.Length];
        for (int i = 0; i < massive.Length; i++)
        {
            positions[i] = massive[i].ToVector2Int();
        }

        return positions;
    }

    public static Bector2Int[] GetVector2IntListAsBector(List<Vector2Int> vectors)
    {
        Bector2Int[] positions = new Bector2Int[vectors.Count];
        foreach (Vector2Int v in vectors)
        {
            positions[vectors.IndexOf(v)] = new Bector2Int(v);
        }

        return positions;
    }
}

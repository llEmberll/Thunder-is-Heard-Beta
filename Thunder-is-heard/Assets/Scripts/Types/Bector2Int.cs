using System.Collections.Generic;
using System.Linq;
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

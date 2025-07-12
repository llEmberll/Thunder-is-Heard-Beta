using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RectangleBector2Int
{
    [JsonProperty("startPosition")]
    public Bector2Int _startPosition;

    [JsonProperty("size")]
    public Bector2Int _size;


    public RectangleBector2Int() { }

    public RectangleBector2Int(Bector2Int startPosition, Bector2Int endPosition)
    {
        int minX = startPosition._x; 
        int minY = startPosition._y;
        int maxX = endPosition._x; 
        int maxY = endPosition._y;

        _startPosition = new Bector2Int(minX, minY);
        _size = new Bector2Int(maxX - minX + 1, maxY - minY + 1);
    }


    public RectangleBector2Int(Bector2Int[] positions)
    {
            // Ќаходим минимальные и максимальные координаты
            int minX = int.MaxValue; // »нициализируем минимальные значени€ максимально возможным числом
            int minY = int.MaxValue;
            int maxX = int.MinValue; // »нициализируем максимальные значени€ минимально возможным числом
            int maxY = int.MinValue;

            for (int i = 0; i < positions.Length; i++)
            {
                minX = Mathf.Min(minX, positions[i]._x);
                minY = Mathf.Min(minY, positions[i]._y);
                maxX = Mathf.Max(maxX, positions[i]._x);
                maxY = Mathf.Max(maxY, positions[i]._y);
            }   

            _startPosition = new Bector2Int(minX, minY);
            _size = new Bector2Int(maxX - minX + 1, maxY - minY + 1);
    }

    public Bector2Int[] GetPositions()
    {
        List<Bector2Int> positions = new List<Bector2Int>();

        for (int x = _startPosition._x; x < _startPosition._x + _size._x; x++)
        {
            for (int y = _startPosition._y; y < _startPosition._y + _size._y; y++)
            {
                positions.Add(new Bector2Int(x, y));
            }
        }

        return positions.ToArray();
    }

    public bool Contains(Bector2Int point)
    {
        // ѕровер€ем, находитс€ ли точка в пределах пр€моугольника
        return point._x >= _startPosition._x && point._x < _startPosition._x + _size._x &&
               point._y >= _startPosition._y && point._y < _startPosition._y + _size._y;
    }

    public Bector2Int FindCenter()
    {

    }
}

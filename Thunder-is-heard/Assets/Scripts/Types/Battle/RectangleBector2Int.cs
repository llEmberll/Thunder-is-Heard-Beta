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
            // ������� ����������� � ������������ ����������
            int minX = int.MaxValue; // �������������� ����������� �������� ����������� ��������� ������
            int minY = int.MaxValue;
            int maxX = int.MinValue; // �������������� ������������ �������� ���������� ��������� ������
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

    public static RectangleBector2Int FromStartAndSize(Bector2Int startPosition, Bector2Int size)
    {
        return new RectangleBector2Int { _startPosition = startPosition, _size = size };
    }

    public static RectangleBector2Int FromStartAndEnd(Bector2Int startPosition, Bector2Int endPosition)
    {
        int minX = startPosition._x; 
        int minY = startPosition._y;
        int maxX = endPosition._x; 
        int maxY = endPosition._y;

        return new RectangleBector2Int 
        { 
            _startPosition = new Bector2Int(minX, minY),
            _size = new Bector2Int(maxX - minX + 1, maxY - minY + 1)
        };
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
        // ���������, ��������� �� ����� � �������� ��������������
        return point._x >= _startPosition._x && point._x < _startPosition._x + _size._x &&
               point._y >= _startPosition._y && point._y < _startPosition._y + _size._y;
    }

    public Vector2 FindAbsoluteCenter()
    {
        float centerX = _startPosition._x + (_size._x / 2f);
        float centerY = _startPosition._y + (_size._y / 2f);
        return new Vector2(centerX, centerY);
    }

    public Vector2Int FindAbsoluteCenterAsInt()
    {
        Vector2 center = FindAbsoluteCenter();
        return new Vector2Int(Mathf.FloorToInt(center.x), Mathf.FloorToInt(center.y));
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        RectangleBector2Int other = (RectangleBector2Int)obj;
        return _size == other._size && _startPosition == other._startPosition;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + _size.GetHashCode();
            hash = hash * 23 + _startPosition.GetHashCode();
            return hash;
        }
    }
}

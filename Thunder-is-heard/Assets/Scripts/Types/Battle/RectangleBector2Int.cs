
using UnityEngine;


public class RectangleBector2Int
{
    public Bector2Int _startPosition;
    public Bector2Int _size;

    public RectangleBector2Int(Bector2Int startPosition, Bector2Int size)
    {
        _startPosition = startPosition;
        _size = size;
    }

    public RectangleBector2Int(Bector2Int[] positions)
    {
            // Находим минимальные и максимальные координаты
            int minX = int.MaxValue; // Инициализируем минимальные значения максимально возможным числом
            int minY = int.MaxValue;
            int maxX = int.MinValue; // Инициализируем максимальные значения минимально возможным числом
            int maxY = int.MinValue;

            for (int i = 0; i < positions.Length; i++)
            {
                minX = Mathf.Min(minX, positions[i].x);
                minY = Mathf.Min(minY, positions[i].y);
                maxX = Mathf.Max(maxX, positions[i].x);
                maxY = Mathf.Max(maxY, positions[i].y);
            }   

            _startPosition = new Bector2Int(minX, minY);
            _size = new Bector2Int(maxX - minX + 1, maxY - minY + 1);
    }
}

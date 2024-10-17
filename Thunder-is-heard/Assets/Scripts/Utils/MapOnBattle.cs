using System.Collections;
using System.Collections.Generic;

public class MapOnBattle
{

    public Dictionary<Bector2Int, EvaluateCellData> Cells { get; set; }

    public MapOnBattle(CellData[] cells)
    {
        InitCells(cells);
    }

    public void InitCells(CellData[] cells)
    {
        Cells = new Dictionary<Bector2Int, EvaluateCellData>();
        foreach (var cell in cells)
        {
            Cells.Add(cell._position, new EvaluateCellData(cell));
        }
    }


    public List<Bector2Int> GetReachablePositions(Bector2Int start, int maxMoves)
    {
        // Список доступных позиций
        List<Bector2Int> reachablePositions = new List<Bector2Int>();

        // Очередь для хранения позиций, которые нужно проверить
        Queue<Bector2Int> positionsToCheck = new Queue<Bector2Int>();
        positionsToCheck.Enqueue(start);

        // Множество для отслеживания уже проверенных позиций
        HashSet<Bector2Int> visitedPositions = new HashSet<Bector2Int>();
        visitedPositions.Add(start);

        // Проверка соседних клеток
        while (positionsToCheck.Count > 0 && maxMoves >= 0)
        {
            Bector2Int currentPosition = positionsToCheck.Dequeue();

            // Добавляем текущую клетку в список доступных
            reachablePositions.Add(currentPosition);

            // Если не достигли максимальной мобильности, проверяем соседние клетки
            if (maxMoves > 0)
            {
                List<EvaluateCellData> freeNeighborCells = GetValidNeighbors(currentPosition);
                foreach (EvaluateCellData cell in freeNeighborCells)
                {
                    Bector2Int nextPosition = cell._position;

                    // Проверка, находится ли следующая клетка в пределах карты и не была ли она уже проверена
                    if (!visitedPositions.Contains(nextPosition))
                    {
                        // Добавляем клетку в очередь для проверки
                        positionsToCheck.Enqueue(nextPosition);
                        visitedPositions.Add(nextPosition);
                    }
                }
            }

            // Уменьшаем оставшуюся мобильность
            maxMoves--;
        }

        return reachablePositions;
    }

    // Метод для поиска кратчайшего пути
    public List<Bector2Int> FindShortestPath(Bector2Int start, Bector2Int end)
    {
        // Очередь для хранения клеток, которые нужно проверить
        Queue<Bector2Int> positionsToCheck = new Queue<Bector2Int>();
        positionsToCheck.Enqueue(start);

        // Словарь для хранения предшественников для каждой клетки
        Dictionary<Bector2Int, Bector2Int> predecessors = new Dictionary<Bector2Int, Bector2Int>();

        // Множество для отслеживания уже проверенных клеток
        HashSet<Bector2Int> visitedPositions = new HashSet<Bector2Int>();
        visitedPositions.Add(start);

        // Проверка соседних клеток
        while (positionsToCheck.Count > 0)
        {
            Bector2Int currentPosition = positionsToCheck.Dequeue();

            // Если текущая клетка - конечная, то путь найден
            if (currentPosition == end)
            {
                return ReconstructPath(start, end, predecessors);
            }

            // Проверка соседей
            foreach (EvaluateCellData neighbor in GetValidNeighbors(currentPosition, excludes: visitedPositions))
            {
                // Добавляем соседа в очередь для проверки
                positionsToCheck.Enqueue(neighbor._position);
                // Записываем предшественника
                predecessors[neighbor._position] = currentPosition;
                // Отмечаем соседа как проверенный
                visitedPositions.Add(neighbor._position);
            }
        }

        // Путь не найден
        return null;
    }

    public List<Bector2Int> ReconstructPath(Bector2Int start, Bector2Int end, Dictionary<Bector2Int, Bector2Int> predecessors)
    {
        List<Bector2Int> path = new List<Bector2Int>();
        path.Add(end);
        Bector2Int currentPosition = end;

        while (predecessors.Count > 0) 
        {
            Bector2Int nextPosition = predecessors[currentPosition];
            path.Add(nextPosition);
            predecessors.Remove(currentPosition);
            currentPosition = nextPosition;
        }

        if (path.Contains(start)) 
        { 
            path.Remove(start);
        }

        path.Reverse();
        return path;
    }


    // Метод для получения списка доступных соседей
    private List<EvaluateCellData> GetValidNeighbors(Bector2Int position, HashSet<Bector2Int> excludes = null)
    {
        List<EvaluateCellData> neighbors = new List<EvaluateCellData>();
        // Проверка соседей по горизонтали и вертикали
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Bector2Int neighborPosition = new Bector2Int(position._x + x, position._y + z);

                // Не добавляем исключения
                if (excludes != null && excludes.Contains(neighborPosition)) continue;

                // Не добавляем несуществующие клетки
                if (!Cells.ContainsKey(neighborPosition)) continue;

                // Не добавляем препятствия
                EvaluateCellData neighbor = Cells[neighborPosition];
                if (neighbor._isOccypy) continue;
               
                // Клетка прошла все проверки, добавляем
                neighbors.Add(neighbor);
            }
        }

        // Целевая клетка не должна быть в списке соседних
        if (neighbors.Contains(Cells[position]))
        {
            neighbors.Remove(Cells[position]);
        }

        // Возвращаем список соседей
        return neighbors;
    }
}

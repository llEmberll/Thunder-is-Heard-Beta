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
        // ������ ��������� �������
        List<Bector2Int> reachablePositions = new List<Bector2Int>();

        // ������� ��� �������� �������, ������� ����� ���������
        Queue<Bector2Int> positionsToCheck = new Queue<Bector2Int>();
        positionsToCheck.Enqueue(start);

        // ��������� ��� ������������ ��� ����������� �������
        HashSet<Bector2Int> visitedPositions = new HashSet<Bector2Int>();
        visitedPositions.Add(start);

        // �������� �������� ������
        while (positionsToCheck.Count > 0 && maxMoves >= 0)
        {
            Bector2Int currentPosition = positionsToCheck.Dequeue();

            // ��������� ������� ������ � ������ ���������
            reachablePositions.Add(currentPosition);

            // ���� �� �������� ������������ �����������, ��������� �������� ������
            if (maxMoves > 0)
            {
                List<EvaluateCellData> freeNeighborCells = GetValidNeighbors(currentPosition);
                foreach (EvaluateCellData cell in freeNeighborCells)
                {
                    Bector2Int nextPosition = cell._position;

                    // ��������, ��������� �� ��������� ������ � �������� ����� � �� ���� �� ��� ��� ���������
                    if (!visitedPositions.Contains(nextPosition))
                    {
                        // ��������� ������ � ������� ��� ��������
                        positionsToCheck.Enqueue(nextPosition);
                        visitedPositions.Add(nextPosition);
                    }
                }
            }

            // ��������� ���������� �����������
            maxMoves--;
        }

        return reachablePositions;
    }

    // ����� ��� ������ ����������� ����
    public List<Bector2Int> FindShortestPath(Bector2Int start, Bector2Int end)
    {
        // ������� ��� �������� ������, ������� ����� ���������
        Queue<Bector2Int> positionsToCheck = new Queue<Bector2Int>();
        positionsToCheck.Enqueue(start);

        // ������� ��� �������� ���������������� ��� ������ ������
        Dictionary<Bector2Int, Bector2Int> predecessors = new Dictionary<Bector2Int, Bector2Int>();

        // ��������� ��� ������������ ��� ����������� ������
        HashSet<Bector2Int> visitedPositions = new HashSet<Bector2Int>();
        visitedPositions.Add(start);

        // �������� �������� ������
        while (positionsToCheck.Count > 0)
        {
            Bector2Int currentPosition = positionsToCheck.Dequeue();

            // ���� ������� ������ - ��������, �� ���� ������
            if (currentPosition == end)
            {
                return ReconstructPath(start, end, predecessors);
            }

            // �������� �������
            foreach (EvaluateCellData neighbor in GetValidNeighbors(currentPosition, excludes: visitedPositions))
            {
                // ��������� ������ � ������� ��� ��������
                positionsToCheck.Enqueue(neighbor._position);
                // ���������� ���������������
                predecessors[neighbor._position] = currentPosition;
                // �������� ������ ��� �����������
                visitedPositions.Add(neighbor._position);
            }
        }

        // ���� �� ������
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


    // ����� ��� ��������� ������ ��������� �������
    private List<EvaluateCellData> GetValidNeighbors(Bector2Int position, HashSet<Bector2Int> excludes = null)
    {
        List<EvaluateCellData> neighbors = new List<EvaluateCellData>();
        // �������� ������� �� ����������� � ���������
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Bector2Int neighborPosition = new Bector2Int(position._x + x, position._y + z);

                // �� ��������� ����������
                if (excludes != null && excludes.Contains(neighborPosition)) continue;

                // �� ��������� �������������� ������
                if (!Cells.ContainsKey(neighborPosition)) continue;

                // �� ��������� �����������
                EvaluateCellData neighbor = Cells[neighborPosition];
                if (neighbor._isOccypy) continue;
               
                // ������ ������ ��� ��������, ���������
                neighbors.Add(neighbor);
            }
        }

        // ������� ������ �� ������ ���� � ������ ��������
        if (neighbors.Contains(Cells[position]))
        {
            neighbors.Remove(Cells[position]);
        }

        // ���������� ������ �������
        return neighbors;
    }
}

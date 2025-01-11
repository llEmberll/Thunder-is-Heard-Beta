using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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

    public List<Bector2Int> GetReachablePositions(Bector2Int start, int range)
    {
        Dictionary<Bector2Int, EvaluateCellData> possibleMovePositions = GetValidPositionsMapByRange(range, start);
        if (range == 1)
        {
            return possibleMovePositions.Keys.ToList();
        }

        Dictionary<Bector2Int, int> realMovePositions = new Dictionary<Bector2Int, int>();
        List<EvaluateCellData> firstPositions = GetValidNeighbors(possibleMovePositions, start);

        foreach (EvaluateCellData position in firstPositions)
        {
            realMovePositions.Add(position._position, 1);
        }

        EvaluateCellData[] possibleMovePositionsAsArray = possibleMovePositions.Values.ToArray();
        realMovePositions = FindAllMovablePositionsByRangeRecursive(possibleMovePositionsAsArray, realMovePositions, range, firstPositions, 2);
        return realMovePositions.Keys.ToList();
    }

    //����� ��������� ����� �� �������
    //rangeCenter - ����������� ����� �������
    public Bector2Int FindNearestPositionToOtherPositionFromRange(Bector2Int[] positionsRange, Bector2Int rangeCenter, Bector2Int targetPosition)
    {
        Bector2Int nearestPosition = positionsRange[0];
        int nearestDistanceToTarget = BattleEngine.GetDistanceBetweenPoints(nearestPosition, targetPosition);
        int distanceToCenter = BattleEngine.GetDistanceBetweenPoints(rangeCenter, nearestPosition);

        for (int index = 1; index < positionsRange.Length; index++)
        {
            // ������ ���������� �� ������ ������� �� ����
            Bector2Int currentPositionFromRange = positionsRange[index];
            if (currentPositionFromRange == null) continue;

            int currentDistanceToTarget = BattleEngine.GetDistanceBetweenPoints(currentPositionFromRange, targetPosition);
            if (currentDistanceToTarget < nearestDistanceToTarget)
            {

                // ������� �������, ������� ����� � ����, ��� ��������� �� �����
                nearestPosition = currentPositionFromRange;
                nearestDistanceToTarget = currentDistanceToTarget;
                distanceToCenter = BattleEngine.GetDistanceBetweenPoints(currentPositionFromRange, rangeCenter);
            }
            else if (currentDistanceToTarget == nearestDistanceToTarget)
            {
                // ������� �������������� ������� �� ���� �� ��������� � ��������� ��������� �� �����
                int distanceBetweenCenterAndCurrentPosition = BattleEngine.GetDistanceBetweenPoints(currentPositionFromRange, rangeCenter);
                if (distanceBetweenCenterAndCurrentPosition < distanceToCenter)
                {
                    // ������� ������� ����� � ������, ������ ���������� �������� - ������ ��� ����� �����������
                    nearestPosition = currentPositionFromRange;
                    nearestDistanceToTarget = currentDistanceToTarget;
                    distanceToCenter = distanceBetweenCenterAndCurrentPosition;
                }
            }
        }

        return nearestPosition;
    }

    public List<Bector2Int> BuildRoute(Bector2Int start, Bector2Int end, int maxRouteLength)
    {
        Dictionary<Bector2Int, EvaluateCellData> possibleOverRoutePositions = GetValidPositionsMapByRange(maxRouteLength, start);
        if (!possibleOverRoutePositions.ContainsKey(end)) {
            end = FindNearestPositionToOtherPositionFromRange(possibleOverRoutePositions.Keys.ToArray(), start, end);
        }

        return FindShortestPath(start, end);
    }

    public List<Bector2Int> BuildRouteForAttackTarget(Bector2Int start, Bector2Int targetPosition, int attackRange, int maxRouteLength)
    {
        if (attackRange < 1) return null;
        if (BattleEngine.GetDistanceBetweenPoints(start, targetPosition) <= attackRange) return new List<Bector2Int>();

        //��� �������, � ������� �������� ����� ����
        Dictionary<Bector2Int, EvaluateCellData> positionsForAttack = GetValidPositionsMapByRange(attackRange, targetPosition);

        //��������� ������� ��� �����
        Bector2Int nearestPositionForAttack = FindNearestPositionToOtherPositionFromRange(positionsForAttack.Keys.ToArray(), targetPosition, start);
        Bector2Int positionForBuildRoute = nearestPositionForAttack;

        //�������, �� ������� ������ �������� ���������
        Dictionary<Bector2Int, EvaluateCellData> possibleOverRoutePositions = GetValidPositionsMapByRange(maxRouteLength, start);

        //���� �� ��������� ������� ��� ����� ���������� ������ ���������
        if (!possibleOverRoutePositions.ContainsKey(nearestPositionForAttack))
        {
            //����� ���������� ����������� ������ �� ��������� ������� �����
            positionForBuildRoute = FindNearestPositionToOtherPositionFromRange(possibleOverRoutePositions.Keys.ToArray(), start, nearestPositionForAttack);
        }

        return FindShortestPath(start, positionForBuildRoute);
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

            // �������� �������
            foreach (EvaluateCellData neighbor in GetValidNeighbors(Cells, currentPosition, excludes: visitedPositions))
            {
                // ��������� ������ � ������� ��� ��������
                positionsToCheck.Enqueue(neighbor._position);
                // ���������� ���������������
                predecessors[neighbor._position] = currentPosition;
                // �������� ������ ��� �����������
                visitedPositions.Add(neighbor._position);
                // ���� ������� ������ - ��������, �� ���� ������
                if (neighbor._position.Equals(end))
                {
                    return ReconstructPath(start, end, predecessors);
                }
            }
        }

        // ���� �� ������
        throw new System.Exception("���� �� ������. �����: " + start._x + "|" + start._y + " �����: " + end._x + "|" + end._y);
    }

    public List<Bector2Int> ReconstructPath(Bector2Int start, Bector2Int end, Dictionary<Bector2Int, Bector2Int> predecessors)
    {
        List<Bector2Int> path = new List<Bector2Int>();
        path.Add(end);
        Bector2Int currentPosition = end;

        for (int index = 0; index < predecessors.Count; index++)
        {
            if (!predecessors.ContainsKey(currentPosition))
            {
                break;
            }
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
    private List<EvaluateCellData> GetValidNeighbors(Dictionary<Bector2Int, EvaluateCellData> positionsMap, Bector2Int position, HashSet<Bector2Int> excludes = null)
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
                if (!positionsMap.ContainsKey(neighborPosition)) continue;

                // �� ��������� �����������
                EvaluateCellData neighbor = positionsMap[neighborPosition];
                if (neighbor._isOccypy) continue;
               
                // ������ ������ ��� ��������, ���������
                neighbors.Add(neighbor);
            }
        }

        if (positionsMap.ContainsKey(position))
        {
            // ������� ������ �� ������ ���� � ������ ��������
            if (neighbors.Contains(positionsMap[position]))
            {
                neighbors.Remove(positionsMap[position]);
            }
        }

        // ���������� ������ �������
        return neighbors;
    }

    public Dictionary<Bector2Int, EvaluateCellData> GetValidPositionsMapByRange(int range, Bector2Int center, bool ignoreOccypy = false)
    {
        Dictionary<Bector2Int, EvaluateCellData> validPositionsMap = new Dictionary<Bector2Int, EvaluateCellData>();

        int rootOfCellsCount = (int)Math.Sqrt(Cells.Count);
        if (range >= rootOfCellsCount) range = rootOfCellsCount;
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                Bector2Int currentPosition = new Bector2Int(center._x + x, center._y + z);

                // �� ��������� �������������� ������
                if (!Cells.ContainsKey(currentPosition)) continue;

                // �� ��������� �����������
                EvaluateCellData cell = Cells[currentPosition];
                if (cell._isOccypy && ignoreOccypy == false) continue;

                // ������ ������ ��� ��������, ���������
                validPositionsMap.Add(currentPosition, cell);
            }
        }

        if (Cells.ContainsKey(center))
        {
            // ������� ������ �� ������ ���� � ������
            if (validPositionsMap.ContainsKey(center))
            {
                validPositionsMap.Remove(center);
            }
        }

        return validPositionsMap;
    }


    public Dictionary<Bector2Int, int> FindAllMovablePositionsByRangeRecursive(
        EvaluateCellData[] possiblePositions, 
        Dictionary<Bector2Int, int> resultPositionsMap, 
        int range, List<EvaluateCellData> previousPositions,
        int currentRange
        )
    {
        List<EvaluateCellData> newPositions = new List<EvaluateCellData>();

        foreach (EvaluateCellData currentPreviousPosition in previousPositions)
        {
            for (int index = 0; index < possiblePositions.Length; index++)
            {
                EvaluateCellData currentPossiblePosition = possiblePositions[index];
                if (currentPossiblePosition == null) { continue; }

                if (IsNeighbours(currentPreviousPosition._position, currentPossiblePosition._position))
                {
                    if (!resultPositionsMap.ContainsKey(currentPossiblePosition._position))
                    {
                        resultPositionsMap.Add(currentPossiblePosition._position, currentRange);
                        newPositions.Add(currentPossiblePosition);
                    }

                    possiblePositions[index] = null;
                }
            }
        }

        if (currentRange < range && possiblePositions.Length > 0) return FindAllMovablePositionsByRangeRecursive(possiblePositions, resultPositionsMap, range, newPositions, currentRange + 1);
        return resultPositionsMap;
    }

    public static bool IsNeighbours(Bector2Int position1, Bector2Int position2)
    {
        return Mathf.Max(Mathf.Abs(position1._x - position2._x), Mathf.Abs(position1._y - position2._y)) == 1;
    }
}

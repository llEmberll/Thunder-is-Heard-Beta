using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Map : MonoBehaviour
{
    public GameObject cellPrefab;

    public GameObject cellsParent;
    public GameObject terrainParent;

    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
    public Dictionary<Vector2Int, Cell> Cells {  get { return cells; } }

    public Cell centralCell;
    public Vector2Int size;

    public Terrain terrain;

	public virtual void Awake()
	{
	}

    public void InitPrefabs()
    {
        cellPrefab = Resources.Load<GameObject>(Config.mapResources["cellPrefab"]);
    }

    public void InitParents()
    {
        GameObject cellsParentPrefab = Resources.Load<GameObject>(Config.mapResources["cellsParent"]);
        cellsParent = Instantiate(cellsParentPrefab, cellsParentPrefab.transform.position, Quaternion.identity, transform);

        GameObject terrainParentPrefab = Resources.Load<GameObject>(Config.mapResources["terrainParent"]);
        terrainParent = Instantiate(terrainParentPrefab, terrainParentPrefab.transform.position, Quaternion.identity, transform);
    }

    public void Init(Vector2Int mapSize, string terrainPath)
    {
        size = mapSize;
        InitPrefabs();
        InitParents();
        GenerateCells(size);
        FindCentralCell();
        InitTerrain(terrainPath);
    }

    public void InitTerrain(string terrainPath)
    {
        Transform terrainObj = Instantiate(Resources.Load<Terrain>(terrainPath).transform, parent: terrainParent.transform);
        terrainObj.transform.position = new Vector3(0, 0, 0);

        terrainParent.transform.position += new Vector3(-5, 0, -5);

        terrain = terrainObj.GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        terrainData.size = new Vector3(5 * 2 + size.x, terrainData.size.y, 5 * 2 + size.y);
        terrain.terrainData = terrainData;
    }

    [ContextMenu("Generate grid")]
    public void GenerateCells(Vector2Int size)
    {
        DeleteCells();

        var cellsize = cellPrefab.GetComponent<MeshRenderer>().bounds.size;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var position = new Vector3(x * (cellsize.x), 0, y * (cellsize.z));
                var cell = Instantiate(cellPrefab, position, Quaternion.identity, cellsParent.transform);
                Cell currentCellComponent = cell.GetComponent<Cell>();
                cells.Add(currentCellComponent.position, currentCellComponent);

                cell.name = $"|X:{x}||Y:{y}|";
                cell.tag = "Cell";
            }
        }
    }

    [ContextMenu("Delete Cells")]
    public void DeleteCells()
    {
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
        foreach (GameObject obj in cells)
        {
            DestroyImmediate(obj);
        }
    }

    public void FindCentralCell()
	{
        int centralX = (int)Math.Floor(size.x / 2.0);
        int centralY = (int)Math.Floor(size.y / 2.0);
        centralCell = cells[new Vector2Int(centralX, centralY)];
	}

    public Dictionary<Vector2Int, Cell> FindCellsByPosition(List<Vector2Int> positions)
    {
        Dictionary<Vector2Int, Cell> founded = new Dictionary<Vector2Int, Cell> ();

        foreach (Vector2Int position in positions)
        {
            if (Cells.ContainsKey(position))
            {
                founded.Add(position, cells[position]);
            }
        }

        return founded;
    }

    public Dictionary<Bector2Int, Cell> FindCellsByPosition(List<Bector2Int> positions)
    {
        Dictionary<Bector2Int, Cell> founded = new Dictionary<Bector2Int, Cell>();

        foreach (Bector2Int position in positions)
        {
            Vector2Int currentPosition = position.ToVector2Int();
            if (Cells.ContainsKey(currentPosition))
            {
                founded.Add(position, cells[currentPosition]);
            }
        }

        return founded;
    }

    public static bool IsNeighnorCells(Cell cell1, Cell cell2)
    {
        return Mathf.Max(Mathf.Abs(cell1.position.x - cell2.position.y), Mathf.Abs(cell1.position.y - cell2.position.y)) == 1;
    }
    public Dictionary<Vector2Int, Cell> GetDisplayedCells()
    {
        Dictionary<Vector2Int, Cell> displayedCells = new Dictionary<Vector2Int, Cell>();

        foreach (var pare in Cells)
        {
            if (pare.Value.visible)
            {
                displayedCells.Add(pare.Key, pare.Value);
            }
        }

        return displayedCells;
    }

    public List<Cell> GetRange(Vector2Int center, int radius, bool ignoreOccypy, Dictionary<Vector2Int, Cell> cellsSet = null)
    {
        if (cellsSet == null)
        {
            cellsSet = Cells;
        }

        HashSet<Cell> resultCells = new HashSet<Cell>();

        // �������� �� ������� � ��������������
        for (int x = center.x - radius; x <= center.x + radius; x++)
        {
            for (int y = center.y - radius; y <= center.y + radius; y++)
            {
                // �������� ���������� ������
                Vector2Int cellPos = new Vector2Int(x, y);
                if (cellsSet.ContainsKey(cellPos))
                {
                    Cell currentCell = cellsSet[cellPos];

                    // ���������, ����� �� ������ ��� ���������� ���������
                    if (!currentCell.occupied || ignoreOccypy)
                    {
                        resultCells.Add(currentCell);
                    }
                }
            }
        }

        // ������� ����������� ������ �� HashSet, ���� ��� ������ � ���������
        if (resultCells.Contains(Cells[center]))
        {
            resultCells.Remove(Cells[center]);
        }

        return resultCells.ToList();
    }


    public void Occypy(List<Vector2Int> position)
    {
        Dictionary<Vector2Int, Cell> cells = FindCellsByPosition(position);
        foreach (var item in cells)
        {
            item.Value.Occupy();
            item.Value.RenderSwitch(false);
        }
    }

    public void Free(List<Vector2Int> position)
    {
        Dictionary<Vector2Int, Cell> cells = FindCellsByPosition(position);
        foreach (var item in cells)
        {
            item.Value.Free();
        }
    }

    public List<Vector2Int> GetOccypationPositionForObj(Vector2Int root, Vector2Int size)
    {
            List<Vector2Int> occypation = new List<Vector2Int>();

            int maxX = root.x + size.x;
            int maxZ = root.y + size.y;

            for (int currentX = root.x; currentX < maxX; currentX++)
            {
                for (int currentZ = root.y; currentZ < maxZ; currentZ++)
                {
                    Vector2Int currentCellPosition = new Vector2Int(currentX, currentZ);
                    occypation.Add(currentCellPosition);
                }
            }
            return occypation;
     }

    public bool isPositionFree(List<Vector2Int> position)
    {
        foreach (Vector2Int pos in position)
        {
            if (!Cells.ContainsKey(pos) || Cells[pos].occupied) return false;
        }

        return true;
    }

    public bool isPositionFreeAsBector2Int(Bector2Int[] position)
    {
        foreach (Bector2Int posAsBector2Int in position)
        {
            Vector2Int posAsVector2Int = posAsBector2Int.ToVector2Int();
            if (!Cells.ContainsKey(posAsVector2Int) || Cells[posAsVector2Int].occupied) return false;
        }

        return true;
    }

    public void DisplayAll()
    {
        foreach (var keyValuePair in Cells)
        {
            keyValuePair.Value.RenderSwitch(true);
        }
    }

    public void HideAll()
    {
        foreach (var keyValuePair in Cells)
        {
            keyValuePair.Value.RenderSwitch(false);
        }
    }

    public void Display(List<Vector2Int> cellsPosition)
    {
        foreach (Vector2Int position in cellsPosition)
        {
            if (Cells.ContainsKey(position))
            {
                Cells[position].RenderSwitch(true);
            }
        }
    }

    public void Hide(List<Vector2Int> cellsPosition)
    {
        foreach (Vector2Int position in cellsPosition)
        {
            if (Cells.ContainsKey(position))
            {
                Cells[position].RenderSwitch(false);
            }
        }
    }

    public void DisplayFree()
    {
        foreach (var keyValuePair in Cells)
        {
            if (!keyValuePair.Value.occupied)
            {
                keyValuePair.Value.RenderSwitch(true);
            }
        }
    }

    public void HideOccypied()
    {
        foreach (var keyValuePair in Cells)
        {
            if (keyValuePair.Value.occupied)
            {
                keyValuePair.Value.RenderSwitch(false);
            }
        }
    }

    public void SetActive(List<Vector2Int> cellsPosition)
    {
        foreach (Vector2Int position in cellsPosition)
        {
            if (Cells.ContainsKey(position))
            {
                Cells[position].gameObject.SetActive(true);
            }
        }
    }

    public void SetInactive(List<Vector2Int> cellsPosition)
    {
        foreach (Vector2Int position in cellsPosition)
        {
            if (Cells.ContainsKey(position))
            {
                Cells[position].gameObject.SetActive(false);
            }
        }
    }

    public void SetActiveAll()
    {
        foreach (var keyValuePair in Cells)
        {
            keyValuePair.Value.gameObject.SetActive(true);
        }
    }

    public void SetInactiveAll()
    {
        foreach (var keyValuePair in Cells)
        {
            keyValuePair.Value.gameObject.SetActive(false);
        }
    }

    public Cell GetCell(Vector2Int position)
    {
        if (Cells.ContainsKey(position))
        {
            return Cells[position];
        }
        return null;
    }

    public List<Cell> BuildRoute(Cell startPoint, Cell endPoint, int maxLenght)
    {
        Dictionary<Vector2Int, Cell> possibleCells = GetDisplayedCells();

        List<Cell> shortestPath = new List<Cell>();
        HashSet<Cell> visited = new HashSet<Cell>();
        List<Cell> currentPath = new List<Cell>();

        FindPath(startPoint, endPoint, maxLenght, visited, currentPath, shortestPath, possibleCells);
        return shortestPath;
    }

    private void FindPath(Cell currentCell, Cell endPoint, int maxLenght, HashSet<Cell> visited,
                          List<Cell> currentPath, List<Cell> shortestPath, Dictionary<Vector2Int, Cell> possibleCells)
    {
        if (currentPath.Count == maxLenght)
        {
            return; // Если длина текущего пути равна максимальной длине, то выходим
        }

        currentPath.Add(currentCell);
        visited.Add(currentCell);

        // �������� �� ���������� �������� �����
        if (currentCell.Equals(endPoint))
        {
            if (shortestPath.Count == 0 || currentPath.Count < shortestPath.Count)
            {
                shortestPath.Clear();
                shortestPath.AddRange(currentPath);
            }
        }
        else
        {
            // �������� ��������� ������
            List<Cell> neighborCells = GetRange(currentCell.position, 1, false, possibleCells);

            foreach (var neighbor in neighborCells)
            {
                if (!visited.Contains(neighbor))
                {
                    // ����������� ����� ��� �������� ������
                    FindPath(neighbor, endPoint, maxLenght, visited, currentPath, shortestPath, possibleCells);
                }
            }
        }

        // ������� ������� ������ �� ���� � ���������� ������
        visited.Remove(currentCell);
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    /// <summary>
    /// Находит максимально приближенную свободную область размера ideal внутри area.
    /// Если идеальная область свободна — возвращает её. Иначе ищет ближайшую свободную область такого же размера внутри area.
    /// </summary>
    /// <param name="ideal">Идеальная область (куда желательно поместить)</param>
    /// <param name="area">Возможная область поиска (ограничение)</param>
    /// <returns>RectangleBector2Int найденной области или null, если не найдено</returns>
    public RectangleBector2Int FindNearestFreeRectangle(RectangleBector2Int ideal, RectangleBector2Int area)
    {
        // 1. Проверяем, свободна ли идеальная область
        var idealPositions = ideal.GetPositions();
        bool idealFree = true;
        foreach (var pos in idealPositions)
        {
            Vector2Int v2 = pos.ToVector2Int();
            if (!Cells.ContainsKey(v2) || Cells[v2].occupied)
            {
                idealFree = false;
                break;
            }
        }
        if (idealFree)
            return ideal;

        if (area == null)
            return null;

        // 2. Ищем все возможные позиции для размещения области размера ideal внутри area
        int minX = area._startPosition._x;
        int minY = area._startPosition._y;
        int maxX = area._startPosition._x + area._size._x - ideal._size._x;
        int maxY = area._startPosition._y + area._size._y - ideal._size._y;

        RectangleBector2Int bestRect = null;
        float bestDistance = float.MaxValue;
        Vector2 idealCenter = ideal.FindAbsoluteCenter();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                // Формируем прямоугольник такого же размера
                var candidate = new RectangleBector2Int(new Bector2Int(x, y), new Bector2Int(x + ideal._size._x - 1, y + ideal._size._y - 1));
                var candidatePositions = candidate.GetPositions();
                bool free = true;
                foreach (var pos in candidatePositions)
                {
                    Vector2Int v2 = pos.ToVector2Int();
                    if (!Cells.ContainsKey(v2) || Cells[v2].occupied)
                    {
                        free = false;
                        break;
                    }
                }
                if (free)
                {
                    // Считаем расстояние между центрами
                    float dist = Vector2.Distance(candidate.FindAbsoluteCenter(), idealCenter);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestRect = candidate;
                    }
                }
            }
        }
        return bestRect;
    }
}

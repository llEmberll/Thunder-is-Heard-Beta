using System;
using System.Collections;
using System.Collections.Generic;
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

        terrainParent.transform.position -= new Vector3(5, 0, 5);

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
        Dictionary<Vector2Int, Cell> finded = new Dictionary<Vector2Int, Cell> ();

        foreach (Vector2Int position in positions)
        {
            if (Cells.ContainsKey(position))
            {
                finded.Add(position, cells[position]);
            }
        }

        return finded;
    }

    public void Occypy(List<Vector2Int> position)
    {
        Dictionary<Vector2Int, Cell> cells = FindCellsByPosition(position);
        foreach (var item in cells)
        {
            item.Value.Occupy();
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

    public void DisplayAll()
    {
        foreach (var keyValuePair in Cells)
        {
            keyValuePair.Value.renderSwitch(true);
        }
    }

    public void HideAll()
    {
        foreach (var keyValuePair in Cells)
        {
            keyValuePair.Value.renderSwitch(true);
        }
    }

    public void Display(List<Vector2Int> cellsPosition)
    {
        foreach (Vector2Int position in cellsPosition)
        {
            if (Cells.ContainsKey(position))
            {
                Cells[position].renderSwitch(true);
            }
        }
    }

    public void Hide(List<Vector2Int> cellsPosition)
    {
        foreach (Vector2Int position in cellsPosition)
        {
            if (Cells.ContainsKey(position))
            {
                Cells[position].renderSwitch(false);
            }
        }
    }

    public void DisplayFree()
    {
        foreach (var keyValuePair in Cells)
        {
            if (!keyValuePair.Value.occupied)
            {
                keyValuePair.Value.renderSwitch(true);
            }
        }
    }

    public void HideOccypied()
    {
        foreach (var keyValuePair in Cells)
        {
            if (keyValuePair.Value.occupied)
            {
                keyValuePair.Value.renderSwitch(false);
            }
        }
    }
}

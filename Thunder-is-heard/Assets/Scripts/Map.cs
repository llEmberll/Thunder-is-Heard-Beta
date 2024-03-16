using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
    public Dictionary<Vector2Int, Cell> Cells {  get { return cells; } }

    public Cell centralCell;
    public Vector2Int size;

	public virtual void Awake()
	{
        LoadCells();
        FindCentralCell();
	}

    public void LoadCells()
    {
        int sizeByX = 0;
        int sizeByY = 0;
        foreach (Transform child in transform)
        {
            Cell cell = child.GetComponent<Cell>();
            Vector2Int cellPosition = new Vector2Int((int)child.transform.position.x, (int)child.transform.position.z);
            if (cellPosition.x > sizeByX) { sizeByX = cellPosition.x; }
            if (cellPosition.y > sizeByY) { sizeByY = cellPosition.y; }
            cells.Add(cellPosition, cell);
        }

        size = new Vector2Int(sizeByX+1, sizeByY+1);
    }

    public void FindCentralCell()
	{
		int cellsCount = cells.Count;
		int mapSize = (int)Mathf.Sqrt(cellsCount);
		int offset = cellsCount % 2 == 0 ? 0 : -1;
		centralCell = cells[new Vector2Int(mapSize / 2 + offset, mapSize / 2 + offset)];
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
}

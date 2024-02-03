using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
    public Dictionary<Vector2Int, Cell> Cells {  get { return cells; } }

    public Cell centralCell;

	public virtual void Awake()
	{
        LoadCells();
        FindCentralCell();
	}

    public void LoadCells()
    {
        foreach (Transform child in transform)
        {
            Cell cell = child.GetComponent<Cell>();
            Vector2Int cellPosition = new Vector2Int((int)child.transform.position.x, (int)child.transform.position.z);
            cells.Add(cellPosition, cell);
        }
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
}

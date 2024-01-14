using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
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
}

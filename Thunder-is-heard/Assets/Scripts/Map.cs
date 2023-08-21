using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
    public Cell centralCell;

	public virtual void Awake()
	{
		FindCentralCell();
	}

	public void FindCentralCell()
	{
		int cellsCount = cells.Count;
		int mapSize = (int)Mathf.Sqrt(cellsCount);
		int offset = cellsCount % 2 == 0 ? 0 : -1;
		centralCell = cells[new Vector2Int(mapSize / 2 + offset, mapSize / 2 + offset)];
	}
}

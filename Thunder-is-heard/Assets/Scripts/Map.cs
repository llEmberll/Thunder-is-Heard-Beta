using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
    public Dictionary<Vector2Int, Cell> Cells {  get { return cells; } }

    public Cell centralCell;
    public Vector2Int size;

    public Terrain terrain;

	public virtual void Awake()
	{
        LoadCells();
        FindCentralCell();
	}

    public void LoadCells()
    {
        int sizeByX = 0;
        int sizeByY = 0;

        Transform cellsParent = GameObject.FindGameObjectWithTag("Cells").transform;
        foreach (Transform child in cellsParent)
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
        foreach (var keyValuePair in cells)
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
}

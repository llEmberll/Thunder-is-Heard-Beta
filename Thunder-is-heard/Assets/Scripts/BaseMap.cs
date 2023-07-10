using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : Map
{
    public override void Awake()
    {
        LoadCells();
        base.Awake();
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
}

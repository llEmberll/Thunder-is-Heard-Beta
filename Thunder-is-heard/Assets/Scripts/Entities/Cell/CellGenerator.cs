using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    [SerializeField] public Vector2Int _size;
    [SerializeField] private Cell _prefab;
    [SerializeField] private float _offset;
    [SerializeField] private Transform _parent;
    

    [ContextMenu("Generate grid")]
    public void GenerateGrid()
    {
        DeleteCells();

        var cellsize = _prefab.GetComponent<MeshRenderer>().bounds.size;

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                var position = new Vector3(x * (cellsize.x + _offset), 0, y * (cellsize.z + _offset));

                var cell = Instantiate(_prefab, position, Quaternion.identity, _parent);

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
    
    private void Awake()
    {
        GenerateGrid();
    }
}

using UnityEngine;


[CreateAssetMenu(menuName = "base cells table", fileName = "base cells")]
public class BaseCellsTable : Table<BaseCellsData>
{
    public override string name
    {
        get
        {
            return "BaseCells";
        }
    }

   public void GenerateDefaultCells(int size)
    {
        Debug.Log("size: " + size);

        this.items = new System.Collections.Generic.List<BaseCellsData> ();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                BaseCellsData data = new BaseCellsData ();
                data.position = new Vector2Int (x, y);

                this.items.Add(data);
            }
        }
    }
}
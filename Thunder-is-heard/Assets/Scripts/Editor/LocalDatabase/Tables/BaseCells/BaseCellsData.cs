using UnityEngine;



[System.Serializable]
public class BaseCellsData : TableItem
{
    [Tooltip("Расположение")]
    [SerializeField] public Vector2Int position;
    public Vector2Int Position
    {
        get { return position; }
        set { }
    }
}


using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class BaseCellsData : SomeTableItem
{
    [Tooltip("Расположение")]
    [SerializeField] public Vector2Int position;
    public Vector2Int Position
    {
        get { return position; }
        set { }
    }

    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "position", position },
        };
    }
}


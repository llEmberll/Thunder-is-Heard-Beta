using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MissionData : SomeTableItem
{
    [Tooltip("Имя")]
    [SerializeField] public string name;
    public string Name
    {
        get { return name; }
        set { }
    }

    [Tooltip("Иконка")]
    [SerializeField] public Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
        set { }
    }

    [Tooltip("Награда")]
    [SerializeField] public ResourcesData reward;
    public ResourcesData Reward
    {
        get { return reward; }
        set { }
    }

    [Tooltip("Карта(путь)")]
    [SerializeField] public string map;
    public string Map
    {
        get { return map; }
        set { }
    }

    [Tooltip("Местность(путь)")]
    [SerializeField] public string terrain;
    public string Terrain
    {
        get { return terrain; }
        set { }
    }

    [Tooltip("Игровые объекты(позиция и путь)")]
    [SerializeField] public Dictionary<Vector2Int, string> objects;
    public Dictionary<Vector2Int, string> Objects
    {
        get { return objects; }
        set { }
    }

    [Tooltip("Этапы(имена)")]
    [SerializeField] public List<string> stages;
    public List<string> Stages
    {
        get { return stages; }
        set { }
    }

    [Tooltip("Координаты для расстановки войск")]
    [SerializeField] public List<Vector2Int> landableCells;
    public List<Vector2Int> LandableCells
    {
        get { return landableCells; }
        set { }
    }

    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "name", name },
            { "icon", icon },
            { "map", map },
            { "terrain", terrain },
            { "objects", objects },
            { "stages", stages },
            { "landableCells", landableCells }
        };
    }
}

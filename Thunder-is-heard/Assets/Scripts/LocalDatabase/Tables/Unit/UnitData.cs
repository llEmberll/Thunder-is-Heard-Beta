using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class UnitData : SomeTableItem
{
    [Tooltip("Имя")]
    [SerializeField] public string name;
    public string Name
    {
        get { return name; }
        set { }
    }

    [Tooltip("Модель")]
    [SerializeField] public string modelPath;
    public string ModelPath
    {
        get { return modelPath; }
        set { }
    }

    [Tooltip("Иконка")]
    [SerializeField] public Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
        set { }
    }

    [Tooltip("Цена строительства")]
    [SerializeField] public ResourcesData cost;
    public ResourcesData Cost
    {
        get { return cost; }
        set { }
    }

    [Tooltip("Время строительства")]
    [SerializeField] public int buildTime;
    public int CreateTime
    {
        get { return buildTime; }
        set { }
    }

    [Tooltip("Вес")]
    [SerializeField] public int weight;
    public int Weight
    {
        get { return weight; }
        set { }
    }


    [Tooltip("Здоровье")]
    [SerializeField] public int health;
    public int Health
    {
        get { return health; }
        set { }
    }

    [Tooltip("Урон")]
    [SerializeField] public int damage;
    public int Damage
    {
        get { return damage; }
        set { }
    }

    [Tooltip("Дальность")]
    [SerializeField] public int distance;
    public int Distance
    {
        get { return distance; }
        set { }
    }

    [Tooltip("Мобильность")]
    [SerializeField] public int speed;
    public int Speed
    {
        get { return speed; }
        set { }
    }

    [Tooltip("Умение")]
    [SerializeField] public Skill skill;
    public Skill Skill
    {
        get { return skill; }
        set { }
    }

    [Tooltip("Расположение")]
    [SerializeField] public Vector2Int position;
    public Vector2Int Position
    {
        get { return position; }
        set { }
    }

    [Tooltip("Поворот")]
    [SerializeField] public int rotation;
    public int Rotation
    {
        get { return rotation; }
        set { }
    }


    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "name", name },
            { "modelPath", modelPath },
            { "icon", icon },
            { "weight", weight },
            { "health", health },
            { "damage", damage },
            { "speed", speed },
            { "distance", distance },
            { "skill", skill },
            { "position", position },
            { "rotation", rotation }
        };
    }
}

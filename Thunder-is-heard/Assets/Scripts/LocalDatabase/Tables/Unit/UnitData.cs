using UnityEngine;
using System.Collections.Generic;
using Org.BouncyCastle.Utilities;


[System.Serializable]
public class UnitData : SomeTableItem
{
    [Tooltip("externalId")]
    [SerializeField] public int externalId;
    public int ExternalId
    {
        get { return externalId; }
        set { }
    }

    [Tooltip("���")]
    [SerializeField] public string name;
    public string Name
    {
        get { return name; }
        set { }
    }

    [Tooltip("������")]
    [SerializeField] public string modelPath;
    public string ModelPath
    {
        get { return modelPath; }
        set { }
    }

    [Tooltip("���� �� ������")]
    [SerializeField] public string iconPath;
    public string IconPath
    {
        get { return iconPath; }
        set { }
    }

    [Tooltip("���� �������������")]
    [SerializeField] public ResourcesData cost;
    public ResourcesData Cost
    {
        get { return cost; }
        set { }
    }

    [Tooltip("����� �������������")]
    [SerializeField] public int buildTime;
    public int CreateTime
    {
        get { return buildTime; }
        set { }
    }

    [Tooltip("���")]
    [SerializeField] public int weight;
    public int Weight
    {
        get { return weight; }
        set { }
    }


    [Tooltip("��������")]
    [SerializeField] public int health;
    public int Health
    {
        get { return health; }
        set { }
    }

    [Tooltip("����")]
    [SerializeField] public int damage;
    public int Damage
    {
        get { return damage; }
        set { }
    }

    [Tooltip("���������")]
    [SerializeField] public int distance;
    public int Distance
    {
        get { return distance; }
        set { }
    }

    [Tooltip("�����������")]
    [SerializeField] public int speed;
    public int Speed
    {
        get { return speed; }
        set { }
    }

    [Tooltip("������")]
    [SerializeField] public Skill skill;
    public Skill Skill
    {
        get { return skill; }
        set { }
    }

    [Tooltip("������������")]
    [SerializeField] public Vector2Int position;
    public Vector2Int Position
    {
        get { return position; }
        set { }
    }

    [Tooltip("�������")]
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
            { "externalId", externalId },
            { "name", name },
            { "modelPath", modelPath },
            { "iconPath", iconPath },
            { "cost", cost },
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

    public override ITableItem Clone()
    {
        UnitData clone = new UnitData();
        clone.externalId = externalId;
        clone.name = name;
        clone.modelPath = modelPath;
        clone.iconPath = iconPath;
        clone.weight = weight;
        clone.cost = cost.Clone();
        clone.buildTime = buildTime;
        clone.health = health;
        clone.damage = damage;
        clone.speed = speed;
        clone.distance = distance;
        clone.skill = skill;
        clone.position = new Vector2Int(position.x, position.y);

        clone.rotation = rotation;
        return clone;
    }
}

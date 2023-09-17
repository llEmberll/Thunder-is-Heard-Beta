using UnityEngine;



[System.Serializable]
public class UnitData : TableItem
{
    [Tooltip("���")]
    [SerializeField] public string name;
    public string Name
    {
        get { return name; }
        set { }
    }

    [Tooltip("������")]
    [SerializeField] public Transform model;
    public Transform Model
    {
        get { return model; }
        set { }
    }

    [Tooltip("������")]
    [SerializeField] public Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
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
}

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

    [Tooltip("Сценарий")]
    [SerializeField] public Scenario scenaio;
    public Scenario Scenario
    {
        get { return scenaio; }
        set { }
    }

    public override Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>
        {
            { "name", name },
            { "icon", icon },
            { "scenario", scenaio }
        };
    }
}

using UnityEngine;



[System.Serializable]
public class PlayerResourceData : TableItem
{
    [Tooltip("�������")]
    [SerializeField] public ResourcesData resources;
    public ResourcesData Resources
    {
        get { return resources; }
        set { }
    }
}


using UnityEngine;



[System.Serializable]
public class PlayerResourceData : TableItem
{
    [Tooltip("Ресурсы")]
    [SerializeField] public ResourcesData resources;
    public ResourcesData Resources
    {
        get { return resources; }
        set { }
    }
}


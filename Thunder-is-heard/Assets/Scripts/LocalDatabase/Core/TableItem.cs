

using System.Collections.Generic;
using UnityEngine;

public class TableItem
{
    [Tooltip("Внутренний id")]
    [SerializeField] public int externalId = -1;
    public int ExternalId
    {
        get { return externalId; }
        set { externalId = value; }
    }

    public virtual Dictionary<string, object> GetFields()
    {
        return new Dictionary<string, object>()
        {
            { "id", externalId },
        };
    }
}

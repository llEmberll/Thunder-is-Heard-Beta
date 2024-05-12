using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ProcessSource
{
    public string type;
    public string id;

    public ProcessSource(string  processSourceType, string processSourceId)
    {
        type = processSourceType;
        id = processSourceId;
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class ConditionData
{
    [JsonProperty("Type")]
    public string Type;

    [JsonProperty("Data")]
    public Dictionary<string, object> Data;

    public ConditionData() { }

    public ConditionData(string type, Dictionary<string, object> data)
    {
        Type = type;
        Data = data;
    }
}

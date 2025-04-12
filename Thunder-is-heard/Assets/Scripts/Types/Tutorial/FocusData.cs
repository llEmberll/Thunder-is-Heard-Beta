using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class FocusData
{
    [JsonProperty("Type")]
    public string Type;

    [JsonProperty("Data")]
    public Dictionary<string, object> Data;


    public FocusData() { }

    public FocusData(string type, Dictionary<string, object> data)
    {
        Type = type;
        Data = data;
    }
}

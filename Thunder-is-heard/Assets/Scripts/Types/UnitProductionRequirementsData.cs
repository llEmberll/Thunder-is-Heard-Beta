using Newtonsoft.Json;


[System.Serializable]
public class UnitProductionRequirementsData
{
    [JsonProperty("sourceBuildLevel")]
    public int? _sourceBuildLevel;

    [JsonProperty("circumstances")]
    public string[] _circumstances; // "Tutorial", "Happy New Year"

    [JsonIgnore]
    public int? SourceBuildLevel
    {
        get { return _sourceBuildLevel; }
        set { }
    }

    [JsonIgnore]
    public string[] Circumstances
    {
        get { return _circumstances; }
        set { }
    }


    public UnitProductionRequirementsData() { }

    public UnitProductionRequirementsData(int? sourceBuildLevel, string[] circumstances)
    {
        this._sourceBuildLevel = sourceBuildLevel;
        this._circumstances = circumstances;
    }
}

using Newtonsoft.Json;


[System.Serializable]
public class Replic
{
    [JsonProperty("charName")]
    public string _charName;

    [JsonProperty("charSide")]
    public string _charSide;

    [JsonProperty("text")]
    public string _text;

    [JsonProperty("focus")]
    public Bector2Int _focus = null;

    public Replic() { }

    public Replic(string charName, string charSide, string text, Bector2Int focus = null)
    {
        _charName = charName;
        _charSide = charSide;
        _text = text;
        _focus = focus;
    }
}

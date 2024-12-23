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

    public Replic() { }

    public Replic(string charName, string charSide, string text)
    {
        _charName = charName;
        _charSide = charSide;
        _text = text;
    }
}

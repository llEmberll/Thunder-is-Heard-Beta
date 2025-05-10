using Newtonsoft.Json;
using System.Collections.Generic;


[System.Serializable]
public class ActiveTutorialCacheItem : CacheItem
{
    public ActiveTutorialCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
    }

    public string GetTutorialId()
    {
        return (string)GetField("tutorialId");
    }

    public void SetTutorialId(string value)
    {
        SetField("tutorialId", value);
    }

    public TutorialStageData GetStage()
    {
        object value = GetField("stage");
        return JsonConvert.DeserializeObject<TutorialStageData>(value.ToString());
    }

    public void SetStage(TutorialStageData value)
    {
        SetField("stage", value);
    }

    public override CacheItem Clone()
    {
        ActiveTutorialCacheItem clone = new ActiveTutorialCacheItem(fields);
        return clone;
    }
}

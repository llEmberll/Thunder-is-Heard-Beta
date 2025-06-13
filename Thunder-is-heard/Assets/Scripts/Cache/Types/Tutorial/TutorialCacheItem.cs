using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class TutorialCacheItem : CacheItem
{
    public TutorialCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("conditionForStart"))
        {
            SetConditionForStart(new ConditionData());
        }

        if (!objFields.ContainsKey("passed"))
        {
            SetPassed(false);
        }
    }

    public bool GetPassed()
    {
        return (bool)GetField("passed");
    }

    public void SetPassed(bool value)
    {
        SetField("passed", value);
    }

    public ConditionData GetConditionForStart()
    {
        object value = GetField("conditionForStart");
        if (value == null)
        {
            return new ConditionData();
        }

        if (value is ConditionData typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<ConditionData>(value.ToString());
    }

    public void SetConditionForStart(ConditionData value)
    {
        SetField("conditionForStart", value);
    }

    public TutorialStageData GetFirstStage()
    {
        object value = GetField("firstStage");

        if (value is TutorialStageData typedValue)
        {
            return typedValue;
        }

        return JsonConvert.DeserializeObject<TutorialStageData>(value.ToString());
    }

    public void SetFirstStage(TutorialStageData value)
    {
        SetField("firstStage", value);
    }

    public override CacheItem Clone()
    {
        TutorialCacheItem clone = new TutorialCacheItem(fields);
        return clone;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;


[System.Serializable]
public class TutorialCacheItem : CacheItem
{
    public TutorialCacheItem(Dictionary<string, object> objFields) : base(objFields)
    {
        if (!objFields.ContainsKey("firstStage"))
        {
            throw new ArgumentException("Tutorial cannot be empty");
        }

        if (!objFields.ContainsKey("startDialogue"))
        {
            SetStartDialogue(new Replic[] {});
        }

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

        return JsonConvert.DeserializeObject<ConditionData>(value.ToString());
    }

    public void SetConditionForStart(ConditionData value)
    {
        SetField("conditionForStart", value);
    }

    public TutorialStageData GetFirstStage()
    {
        object value = GetField("firstStage");
        return JsonConvert.DeserializeObject<TutorialStageData>(value.ToString());
    }

    public void SetFirstStage(TutorialStageData value)
    {
        SetField("firstStage", value);
    }

    public Replic[] GetStartDialogue()
    {
        object value = GetField("startDialogue");
        if (value == null)
        {
            return new Replic[] {};
        }

        return JsonConvert.DeserializeObject<Replic[]>(value.ToString());
    }

    public void SetStartDialogue(Replic[] value)
    {
        SetField("startDialogue", value);
    }

    public override CacheItem Clone()
    {
        TutorialCacheItem clone = new TutorialCacheItem(fields);
        return clone;
    }
}

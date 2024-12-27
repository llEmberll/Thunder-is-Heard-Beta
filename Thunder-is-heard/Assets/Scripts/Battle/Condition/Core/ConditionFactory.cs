using System;
using System.Collections.Generic;
using Newtonsoft.Json;


public static class ConditionFactory
{
    public static ICondition CreateCondition(ConditionData conditionData)
    {
        switch (conditionData.Type)
        {
            case "DestroyAllEnemies":
                return new DestroyAllEnemiesCondition();
            case "DestroyAllAllies":
                return new DestroyAllAlliesCondition();
            case "AttackObject":
                string targetObjectId = (string)conditionData.Data["targetObjectId"];
                return new AttackObjectCondition(targetObjectId);
            case "DestroyObjects":
                string[] targetObjectIds = JsonConvert.DeserializeObject<string[]>(conditionData.Data["targetObjectIds"].ToString());
                return new DestroyObjectsCondition(targetObjectIds);
            case "SideReachPosition":
                RectangleBector2Int positionRectangle = JsonConvert.DeserializeObject<RectangleBector2Int>(conditionData.Data["positionRectangle"].ToString());
                string side = (string)conditionData.Data["side"];
                return new SideReachPositionCondition(positionRectangle, side);
            case "And":
                ConditionData[] andConditionsData = JsonConvert.DeserializeObject<ConditionData[]>(conditionData.Data["conditions"].ToString());
                return new AndCondition(CreateConditions(andConditionsData));
            case "Or":
                ConditionData[] orConditionsData = JsonConvert.DeserializeObject<ConditionData[]>(conditionData.Data["conditions"].ToString());
                return new OrCondition(CreateConditions(orConditionsData));
            default:
                throw new ArgumentException("Неизвестный тип условия: " + conditionData.Type);
        }
    }

    public static List<ICondition> CreateConditions(ConditionData[] conditionsData)
    {
        List<ICondition> conditions = new List<ICondition>();
        foreach (var conditionData in conditionsData)
        {
            conditions.Add(CreateCondition(conditionData));
        }
        return conditions;
    }
}

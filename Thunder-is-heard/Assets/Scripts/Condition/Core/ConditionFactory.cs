using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;


public static class ConditionFactory
{
    public static ICondition CreateCondition(ConditionData conditionData)
    {
        switch (conditionData.Type)
        {
            case "AttackSide":
                string sideForAttack = (string)conditionData.Data["side"];
                return new AttackSideCondition(sideForAttack);
            case "NewTargetForAttack":
                string targetSide = (string)conditionData.Data["targetSide"];
                return new NewTargetForAttackCondition(targetSide);
            case "EnterOnFederationObject":
                int enterFederationtimes = Convert.ToInt32(conditionData.Data["times"]);
                return new EnterOnFederationObjectCondition(enterFederationtimes);
            case "EnterOnObject":
                string targetObjectIdForEnter = (string)conditionData.Data["targetObjectId"];
                int enterTimes = Convert.ToInt32(conditionData.Data["times"]);
                return new EnterOnObjectCondition(targetObjectIdForEnter, enterTimes);
            case "CameraMovement":
                float practiceDuration = Convert.ToSingle(conditionData.Data["duration"]);
                return new CameraPracticeCondition(practiceDuration);
            case "AlwaysTrue":
                return new AlwaysTrueCondition();
            case "AlwaysFalse":
                return new AlwaysFalseCondition();
            case "BaseNameChanged":
                return new BaseNameChangedCondition();
            case "ExistObject":
                string targetObjectIdForExisting = (string)conditionData.Data["targetObjectId"];
                return new ExistObjectCondition(targetObjectIdForExisting);
            case "AllUnitsCollected":
                return new AllUnitsCollectedCondition();
            case "AllResourcesCollected":
                return new AllResourcesCollectedCondition();
            case "ContractInProcess":
                string targetContractIdInProcess = (string)conditionData.Data["targetContractId"];
                return new ContractInProcessCondition(targetContractIdInProcess);
            case "ContractFinished":
                string targetContractIdForFinished = (string)conditionData.Data["targetContractId"];
                return new ContractFinishedCondition(targetContractIdForFinished);
            case "UnitProductionFinished":
                string targetUnitProductionIdForFinished = (string)conditionData.Data["targetUnitProductionId"];
                return new UnitProductionFinishedCondition(targetUnitProductionIdForFinished);
            case "UnitProductionInProcess":
                string targetUnitProductionIdInProcess = (string)conditionData.Data["targetUnitProductionId"];
                return new UnitProductionInProcessCondition(targetUnitProductionIdInProcess);
            case "PanelOpened":
                string panelTag = (string)conditionData.Data["tag"];
                return new PanelOpenedCondition(panelTag);
            case "MissionPassed":
                string missionNameForPass = (string)conditionData.Data["missionName"];
                return new MissionPassedCondition(missionNameForPass);
            case "DestroyAllEnemies":
                return new DestroyAllEnemiesCondition();
            case "DestroyAllAllies":
                return new DestroyAllAlliesCondition();
            case "AttackObject":
                string targetObjectIdForAttack = (string)conditionData.Data["targetObjectId"];
                return new AttackObjectCondition(targetObjectIdForAttack);
            case "ReachToAttackObject":
                string targetObjectIdForReachToAttack = (string)conditionData.Data["targetObjectId"];
                string attackerSide = (string)conditionData.Data["attackerSide"];
                return new ReachToAttackObjectCondition(targetObjectIdForReachToAttack, attackerSide);
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

    public static ConditionData SerializeCondition(ICondition condition)
    {
        if (condition is AlwaysTrueCondition)
        {
            return new ConditionData { Type = "AlwaysTrue", Data = new Dictionary<string, object>() };
        }
        else if (condition is AlwaysFalseCondition)
        {
            return new ConditionData { Type = "AlwaysFalse", Data = new Dictionary<string, object>() };
        }
        else if (condition is BaseNameChangedCondition)
        {
            return new ConditionData { Type = "BaseNameChanged", Data = new Dictionary<string, object>() };
        }
        else if (condition is ExistObjectCondition existObjectCondition)
        {
            return new ConditionData
            {
                Type = "ExistObject",
                Data = new Dictionary<string, object> { { "targetObjectId", existObjectCondition._targetObjectId } }
            };
        }
        else if (condition is AllUnitsCollectedCondition)
        {
            return new ConditionData { Type = "AllUnitsCollected", Data = new Dictionary<string, object>() };
        }
        else if (condition is AllResourcesCollectedCondition)
        {
            return new ConditionData { Type = "AllResourcesCollected", Data = new Dictionary<string, object>() };
        }
        else if (condition is ContractInProcessCondition contractInProcessCondition)
        {
            return new ConditionData
            {
                Type = "ContractInProcess",
                Data = new Dictionary<string, object> { { "targetContractId", contractInProcessCondition._targetContractId } }
            };
        }
        else if (condition is ContractFinishedCondition contractFinishedCondition)
        {
            return new ConditionData
            {
                Type = "ContractFinished",
                Data = new Dictionary<string, object> { { "targetContractId", contractFinishedCondition._targetContractId } }
            };
        }
        else if (condition is UnitProductionFinishedCondition unitProductionFinishedCondition)
        {
            return new ConditionData
            {
                Type = "UnitProductionFinished",
                Data = new Dictionary<string, object> { { "targetUnitProductionId", unitProductionFinishedCondition._targetUnitProductionId } }
            };
        }
        else if (condition is UnitProductionInProcessCondition unitProductionInProcessCondition)
        {
            return new ConditionData
            {
                Type = "UnitProductionInProcess",
                Data = new Dictionary<string, object> { { "targetUnitProductionId", unitProductionInProcessCondition._targetUnitProductionId } }
            };
        }
        else if (condition is PanelOpenedCondition panelOpenedCondition)
        {
            return new ConditionData
            {
                Type = "PanelOpened",
                Data = new Dictionary<string, object> { { "tag", panelOpenedCondition._tag } }
            };
        }
        else if (condition is MissionPassedCondition missionPassedCondition)
        {
            return new ConditionData
            {
                Type = "MissionPassed",
                Data = new Dictionary<string, object> { { "missionName", missionPassedCondition._missionName } }
            };
        }
        else if (condition is DestroyAllEnemiesCondition)
        {
            return new ConditionData { Type = "DestroyAllEnemies", Data = new Dictionary<string, object>() };
        }
        else if (condition is DestroyAllAlliesCondition)
        {
            return new ConditionData { Type = "DestroyAllAllies", Data = new Dictionary<string, object>() };
        }
        else if (condition is AttackObjectCondition attackObjectCondition)
        {
            return new ConditionData
            {
                Type = "AttackObject",
                Data = new Dictionary<string, object> { { "targetObjectId", attackObjectCondition._targetObjectId } }
            };
        }
        else if (condition is ReachToAttackObjectCondition reachToAttackObjectCondition)
        {
            return new ConditionData
            {
                Type = "ReachToAttackObject",
                Data = new Dictionary<string, object>
                {
                    { "targetObjectId", reachToAttackObjectCondition._targetObjectId },
                    { "attackerSide", reachToAttackObjectCondition._attackerSide }
                }
            };
        }
        else if (condition is DestroyObjectsCondition destroyObjectsCondition)
        {
            return new ConditionData
            {
                Type = "DestroyObjects",
                Data = new Dictionary<string, object> { { "targetObjectIds", JsonConvert.SerializeObject(destroyObjectsCondition._targetObjectIds) } }
            };
        }
        else if (condition is SideReachPositionCondition sideReachPositionCondition)
        {
            return new ConditionData
            {
                Type = "SideReachPosition",
                Data = new Dictionary<string, object>
                {
                    { "positionRectangle", JsonConvert.SerializeObject(sideReachPositionCondition._positionRectangle) },
                    { "side", sideReachPositionCondition._side }
                }
            };
        }
        else if (condition is AndCondition andCondition)
        {
            var conditionsData = andCondition._conditions.Select(SerializeCondition).ToArray();
            return new ConditionData
            {
                Type = "And",
                Data = new Dictionary<string, object> { { "conditions", JsonConvert.SerializeObject(conditionsData) } }
            };
        }
        else if (condition is OrCondition orCondition)
        {
            var conditionsData = orCondition._conditions.Select(SerializeCondition).ToArray();
            return new ConditionData
            {
                Type = "Or",
                Data = new Dictionary<string, object> { { "conditions", JsonConvert.SerializeObject(conditionsData) } }
            };
        }
        else if (condition is CameraPracticeCondition cameraPracticeCondition)
        {
            return new ConditionData
            {
                Type = "CameraMovementPractice",
                Data = new Dictionary<string, object> { { "duration", cameraPracticeCondition._practiceDuration } }
            };
        }
        else if (condition is EnterOnObjectCondition enterOnObjectCondition)
        {
            return new ConditionData
            {
                Type = "EnterOnObject",
                Data = new Dictionary<string, object> { { "targetObjectId", enterOnObjectCondition._targetObjectId }, { "times", enterOnObjectCondition._times } }
            };
        }
        else if (condition is EnterOnFederationObjectCondition enterOnFederationObjectCondition)
        {
            return new ConditionData
            {
                Type = "EnterOnFederationObject",
                Data = new Dictionary<string, object> { { "times", enterOnFederationObjectCondition._times } }
            };
        }
        else if (condition is NewTargetForAttackCondition newTargetForAttackCondition)
        {
            return new ConditionData
            {
                Type = "NewTargetForAttack",
                Data = new Dictionary<string, object> { { "targetSide", newTargetForAttackCondition._targetSide } }
            };
        }
        else if (condition is AttackSideCondition attackSideCondition)
        {
            return new ConditionData
            {
                Type = "AttackSide",
                Data = new Dictionary<string, object> { { "side", attackSideCondition._side } }
            };
        }

        throw new ArgumentException("Неизвестный тип условия: " + condition.GetType().Name);
    }
}

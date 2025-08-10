using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusFightController : FocusController
{
    public BuildsOnFight builds;
    public UnitsOnFight units;

    public BattleEngine _battleEngine;

    public Landing _landingUI;

    public override void Awake()
    {
        builds = GameObjectUtils.FindComponentByTagIncludingInactive<BuildsOnFight>(Tags.buildsOnScene);
        units = GameObjectUtils.FindComponentByTagIncludingInactive<UnitsOnFight>(Tags.unitsOnScene);
        _battleEngine = GameObjectUtils.FindComponentByTagIncludingInactive<BattleEngine>(Tags.battleEngine);
        base.Awake();
    }

    public override void InitButtons()
    {
        buttonImageByTag = new Dictionary<string, Image>();

        Image toBattleButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toBattleButton);
        buttonImageByTag.Add(Tags.toBattleButton, toBattleButtonImage);

        Image toBasePrepareButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toBasePrepareButton);
        buttonImageByTag.Add(Tags.toBasePrepareButton, toBasePrepareButtonImage);

        Image toBaseFightButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.toBaseFightButton);
        buttonImageByTag.Add(Tags.toBaseFightButton, toBaseFightButtonImage);

        Image cleanLandingButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.cleanLandingButton);
        buttonImageByTag.Add(Tags.cleanLandingButton, cleanLandingButtonImage);

        Image changeBaseButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.changeBaseButton);
        buttonImageByTag.Add(Tags.changeBaseButton, changeBaseButtonImage);

        Image passButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.passButton);
        buttonImageByTag.Add(Tags.passButton, passButtonImage);

        Image surrenderButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.surrenderButton);
        buttonImageByTag.Add(Tags.surrenderButton, surrenderButtonImage);

        Image supportButtonImage = GameObjectUtils.FindComponentByTagIncludingInactive<Image>(Tags.supportButton);
        buttonImageByTag.Add(Tags.supportButton, supportButtonImage);
    }

    public override void InitUI()
    {
        _landingUI = GameObjectUtils.FindComponentByTagIncludingInactive<Landing>(Tags.landableUnits);
    }

    public override void InitTexts()
    {
        // �����������
    }

    public override void OnFocus(FocusData focusData)
    {
        base.OnFocus(focusData);

        string targetType = focusData.Type;
        switch (targetType)
        {
            case "Unit":
                OnUnitFocus(focusData.Data);
                break;

            default:
                Debug.Log("Unexpected focus type: " +  targetType);
                break;
        }
    }

    public override Build FindBuildByFocusData(Dictionary<string, object> data)
    {
        Build build = null;
        if (data.ContainsKey("childId"))
        {
            string childId = (string)data["childId"];
            build = builds.FindObjectByChildId(childId) as Build;
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            build = builds.FindObjectByCoreId(coreId) as Build;
        }

        return build;
    }

    public void OnUnitFocus(Dictionary<string, object> data)
    {
       

        Unit unit = FindUnitByFocusData(data);
        if (unit == null) return;

        SetCameraFocus(unit.center, data);

        Debug.Log("Unit focused!");

        _targetEntity = unit;
        SaveMaterials(unit.gameObject);
    }

    public Unit FindUnitByFocusData(Dictionary<string, object> data)
    {
        if (data.ContainsKey("underAttack"))
        {
            bool isUnderAttack = (bool)data["underAttack"];
            if (isUnderAttack)
            {
                List<string> childIdsOfTargets = _battleEngine.currentBattleSituation.attackersByObjectId.Keys.ToList();
                List<Unit> targets = units.GetUnitsByChildIds(childIdsOfTargets);
                if (targets.Count < 1)
                {
                    return null;
                }

                if (data.ContainsKey("side"))
                {
                    string side = (string)data["side"];

                    foreach (Unit target in targets)
                    {
                        if (target.side == side)
                        {
                            return target;
                        }
                    }
                    return null;
                }

                return targets.First();
            }
        }

        if (data.ContainsKey("side"))
        {
            string side = (string)data["side"];
            List<Unit> unitsBySide = units.GetUnitsBySide(side);
            return FindUnitByChildIdOrCoreIdFromUnits(data, unitsBySide);
        }

        return FindUnitByChildIdOrCoreIdFromFocusData(data);
    }

    public Unit FindUnitByChildIdOrCoreIdFromUnits(Dictionary<string, object> data, List<Unit> possibleUnits)
    {
        if (possibleUnits.Count < 1)
        {
            return null;
        }

        if (data.ContainsKey("childId"))
        {
            string childId = (string)data["childId"];
            foreach (Unit unit in possibleUnits)
            {
                if (unit.childId == childId)
                {
                    return unit;
                }
            }
            return null;
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            foreach (Unit unit in possibleUnits)
            {
                if (unit.coreId == coreId)
                {
                    return unit;
                }
            }
            return null;
        }

        return possibleUnits.First();
    }

    public Unit FindUnitByChildIdOrCoreIdFromFocusData(Dictionary<string, object> data)
    {
        if (data.ContainsKey("childId"))
        {
            string childId = (string)data["childId"];
            return units.FindObjectByChildId(childId) as Unit;
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            return units.FindObjectByCoreId(coreId) as Unit;
        }

        return null;
    }


    public override void OnUIItemFocus(Dictionary<string, object> data)
    {
        string type = (string)data["UIType"];

        switch (type)
        {
            case "Landing":
                OnLandableUnitFocus(data);
                break;
            default:
                throw new System.Exception("Undefined UIType: " + type);
        }
    }

    public void OnLandableUnitFocus(Dictionary<string, object> data)
    {
        LandableUnit item = null;
        if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            item = _landingUI.FindItemByCoreId(coreId);
        }

        if (item == null)
        {
            Debug.Log("Focus on UIItem: item not found: " + data);
            return;
        }

        Transform bodyTransform = item.gameObject.transform.Find("Body");
        if (bodyTransform != null)
        {
            Image itemImage = bodyTransform.GetComponent<Image>();
            _targetImage = itemImage;
        }
        else
        {
            Debug.LogWarning("Body child object not found in LandableUnit: " + item.gameObject.name);
        }
    }
}

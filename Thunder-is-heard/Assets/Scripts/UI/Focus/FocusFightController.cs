using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FocusFightController : FocusController
{
    public BuildsOnFight builds;
    public UnitsOnFight units;

    public override void Awake()
    {
        builds = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnFight>();
        units = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
        base.Awake();
    }

    public override void InitButtons()
    {
        // реализовать
    }

    public override void InitUI()
    {
        // реализовать
    }

    public override void InitTexts()
    {
        // реализовать
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

        EventMaster.current.FocusCameraOnPosition(unit.center, true);

        SaveMaterials(unit.gameObject);
    }

    public Unit FindUnitByFocusData(Dictionary<string, object> data)
    {
        Unit unit = null;
        if (data.ContainsKey("childId"))
        {
            string childId = (string)data["childId"];
            unit = units.FindObjectByChildId(childId) as Unit;
        }
        else if (data.ContainsKey("coreId"))
        {
            string coreId = (string)data["coreId"];
            unit = units.FindObjectByCoreId(coreId) as Unit;
        }

        return unit;
    }

    public override void OnUIItemFocus(Dictionary<string, object> data)
    {
        throw new System.NotImplementedException();
    }
}

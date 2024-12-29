using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class MissionDetalization: UIElement
{
    public bool focusOn = false;

    public string _id = null;
    public TMP_Text TmpName, TmpDescription;
    public Transform _gives;

    public Image noReserveWarning;

    public bool IsClickedOutside()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return !focusOn;
        }

        return false;
    }

    public void UpdateDetalization(string id, string name, string description, ResourcesData givesData)
    {
        _id = id;
        TmpName.text = name;
        TmpDescription.text = description;

        ResourcesProcessor.UpdateResources(_gives, givesData);
    }

    public void Update()
    {
        if (IsClickedOutside())
        {
            OnClickOutside();
        }
    }

    public virtual void OnClickOutside()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        focusOn = false;
    }

    public void Load()
    {
        if (_id == null)
        {
            return;
        }

        Load(_id);
    }

    public void Load(string missionId)
    {
        if (!HaveReserve())
        {
            noReserveWarning.gameObject.SetActive(true);
            return;
        }

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem cacheItemMission = missionTable.GetById(missionId);
        MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

        ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
        CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
        ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(new Dictionary<string, object>());
        battleData.SetMissionId(missionId);
        battleData.SetUnits(scenarioData.GetUnits());
        battleData.SetBuilds(scenarioData.GetBuilds());
        battleData.SetObstacles(scenarioData.GetObstacles());
        battleTable.AddOne(battleData);
        Cache.Save(battleTable);

        SceneLoader.LoadFight(new FightSceneParameters(battleData.GetExternalId()));
    }

    public static bool HaveReserve()
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();
        foreach (CacheItem item in inventory.Items.Values)
        {
            InventoryCacheItem currentItemData = new InventoryCacheItem(item.Fields);
            if (currentItemData.GetType() == "Unit") return true;
        }

        return false;
    }
}

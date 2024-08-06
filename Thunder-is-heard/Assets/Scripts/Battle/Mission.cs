using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Mission : Item
{
    public string type = "Mission";

    public override string Type { get { return type;  } }

    public override void Awake()
    {
        type = "Mission";

        TmpName = transform.Find("Text").GetComponent<TMP_Text>();

        TmpName.text = _objName;
    }

    public override void Interact()
    {
        Load(_id);
    }

    public static void Load(string missionId)
    {
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
        battleTable.AddOne(battleData);
        Cache.Save(battleTable);

        SceneLoader.LoadFight(new FightSceneParameters(battleData.GetExternalId()));
    }
}

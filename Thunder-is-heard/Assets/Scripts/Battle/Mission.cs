using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Mission : Item
{
    public string type = "Mission";

    public override string Type { get { return type;  } }

    public override void Awake()
    {
        type = "Mission";

        TmpName = transform.Find("Text").GetComponent<TMP_Text>();

        TmpName.text = objName;
    }

    public override void Interact()
    {
        Load();
    }

    public void Load()
    {
        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem cacheItemMission = missionTable.GetById(id);
        MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

        ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
        CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
        ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(new Dictionary<string, object>());
        battleData.SetMissionId(id);
        battleData.SetMap(scenarioData.GetMap());
        battleData.SetUnits(scenarioData.GetUnits());
        battleData.SetBuilds(scenarioData.GetBuilds());
        battleTable.AddOne(battleData);
        Cache.Save(battleTable);


        GameObject prefab = Resources.Load<GameObject>(Config.resources["fightProcessorPrefab"]);
        GameObject fightProcessorObj = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        FightProcessor fightProcessorComponent = fightProcessorObj.GetComponent<FightProcessor>();
        fightProcessorComponent.Init(battleData.GetExternalId());

        DontDestroyOnLoad(fightProcessorObj);
        
        SceneManager.LoadScene("Fight");
    }
}

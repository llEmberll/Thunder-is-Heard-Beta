using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MissionDetalization: UIElement
{
    public bool focusOn = false;

    public string _id = null;
    public TMP_Text TmpName, TmpDescription;
    public Transform _gives;

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

        //TODO проверки, можно ли запустить миссию(нет ли активного сражения и т.п.)

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem cacheItemMission = missionTable.GetById(_id);
        MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

        ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
        CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
        ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(new Dictionary<string, object>());
        battleData.SetMissionId(_id);
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

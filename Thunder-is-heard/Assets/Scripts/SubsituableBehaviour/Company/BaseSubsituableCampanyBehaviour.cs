using System.Collections.Generic;
using UnityEngine;

public class BaseSubsituableCampanyBehaviour : ISubsituableCampanyBehaviour
{
    public virtual List<MissionItem> GetItems(Campany conductor)
    {
        return conductor.items;
    }

    public virtual void Init(Campany conductor)
    {
        FillContent(conductor);
    }

    public virtual void Load(Campany conductor, MissionDetalization missionDetalization)
    {
        if (MissionDetalization.HaveCurrentFightNow())
        {
            missionDetalization.finishCurrentFightWarning.gameObject.SetActive(true);
            return;
        }

        if (!MissionDetalization.HaveReserve())
        {
            missionDetalization.noReserveWarning.gameObject.SetActive(true);
            return;
        }

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem cacheItemMission = missionTable.GetById(missionDetalization._id);
        MissionCacheItem missionData = new MissionCacheItem(cacheItemMission.Fields);

        ScenarioCacheTable scenarioTable = Cache.LoadByType<ScenarioCacheTable>();
        CacheItem cacheItemScenario = scenarioTable.GetById(missionData.GetScenarioId());
        ScenarioCacheItem scenarioData = new ScenarioCacheItem(cacheItemScenario.Fields);

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        BattleCacheItem battleData = new BattleCacheItem(new Dictionary<string, object>());
        battleData.SetMissionId(missionDetalization._id);
        battleData.SetUnits(scenarioData.GetUnits());
        battleData.SetBuilds(scenarioData.GetBuilds());
        battleData.SetObstacles(scenarioData.GetObstacles());
        battleTable.AddOne(battleData);
        Cache.Save(battleTable);

        SceneLoader.LoadFight(new FightSceneParameters(battleData.GetExternalId()));
    }

    public virtual void Toggle(Campany conductor)
    {
        if (conductor.gameObject.activeSelf)
        {
            conductor.Hide();
        }
        else
        {
            conductor.Show();
        }
    }

    public virtual void FillContent(Campany conductor)
    {
        conductor.items = new List<MissionItem>();

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();

        //TODO фильтровать по доступности миссии
        foreach (var m in missionTable.Items)
        {
            GameObject missionObject = GameObject.Instantiate(conductor.missionPrefab);
            missionObject.transform.SetParent(conductor.content, false);

            MissionItem mission = missionObject.GetComponent<MissionItem>();

            MissionCacheItem missionData = new MissionCacheItem(m.Value.Fields);
            mission.Init(
                conductor._missionDetalization,
                missionData.GetExternalId(),
                missionData.GetName(),
                missionData.GetExternalId(),
                missionData.GetPassed(),
                missionData.GetPoseOnMap().ToVector2Int(),
                missionData.GetGives(),
                missionData.GetDescription()
                );

            conductor.items.Add(mission);
        }
    }
}

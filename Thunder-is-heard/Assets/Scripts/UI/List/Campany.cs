using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campany : ItemList
{
    public List<MissionItem> items;
    public GameObject missionPrefab;
    public MissionDetalization _missionDetalization;

    public override void Start()
    {
        missionPrefab = Resources.Load<GameObject>(Config.resources["UIMissionItemPrefab"]);
        base.Start();
    }

    public override void FillContent()
    {
        items = new List<MissionItem>();

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();

        //TODO фильтровать по доступности миссии
        foreach (var m in missionTable.Items)
        {
            GameObject missionObject = GameObject.Instantiate(missionPrefab);
            missionObject.transform.SetParent(content, false);

            MissionItem mission = missionObject.GetComponent<MissionItem>();

            MissionCacheItem missionData = new MissionCacheItem(m.Value.Fields);
            mission.Init(
                _missionDetalization,
                missionData.GetExternalId(),
                missionData.GetName(), 
                missionData.GetExternalId(), 
                missionData.GetPassed(),
                missionData.GetPoseOnMap().ToVector2Int(), 
                missionData.GetGives(), 
                missionData.GetDescription()                
                );

            items.Add(mission);
        }
    }

    public override void OnClickOutside()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : ItemList
{
    public List<Mission> source;
    public GameObject missionPrefab;
    
    public override void FillContent()
    {
        foreach (var m in source)
        {
            GameObject missionObject = GameObject.Instantiate(missionPrefab);
            missionObject.transform.SetParent(this.transform, false);

            Mission mission = missionObject.GetComponent<Mission>();
            
            mission = m;
        }
        
        
    }
}

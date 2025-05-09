
public class MissionPassedCondition: BasicCondition
{
    public string _missionName;
    public bool passed = false;


    public MissionPassedCondition(string missionName) 
    {
        _missionName = missionName;

        passed = IsMissionPassed();
    }

    public bool IsMissionPassed()
    {
        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        MissionCacheItem missionData = missionTable.FindMissionByName( _missionName);
        return missionData.GetPassed();
    }


    public override bool IsComply()
    {
        return passed;
    }
}

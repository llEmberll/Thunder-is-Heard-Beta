
public class MissionPassedCondition: BasicCondition
{
    public bool firstCheck = true;

    public string _missionName;
    public bool passed = false;


    public MissionPassedCondition(string missionName) 
    {
        _missionName = missionName;
    }

    public void FirstComplyCheck()
    {
        firstCheck = false;

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
        if (firstCheck)
        {
            FirstComplyCheck();
        }

        return passed;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}

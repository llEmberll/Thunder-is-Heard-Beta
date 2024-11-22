using UnityEngine;
using UnityEngine.UI;

public class Logs : ItemList
{
    public bool fightIsOver = false;
    public Image victoryLog;
    public Transform victoryGives;
    public ResourcesData victoryGivesData;

    public Image defeatLog;

    public override void Start()
    {
        InitMissionRewards();
        EnableListeners();
        HideLogs();
    }

    public void InitMissionRewards()
    {
        string battleId = FightSceneLoader.parameters._battleId;
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem battleCacheItem = battleTable.GetById(battleId);
        BattleCacheItem battleData = new BattleCacheItem(battleCacheItem.Fields);

        string missionId = battleData.GetMissionId();

        MissionCacheTable missionTable = Cache.LoadByType<MissionCacheTable>();
        CacheItem missionCacheItem = missionTable.GetById(missionId);
        MissionCacheItem missionData = new MissionCacheItem(missionCacheItem.Fields);

        victoryGivesData = missionData.GetGives();
    }

    public void HideLogs()
    {
        victoryLog.gameObject.SetActive(false);
        defeatLog.gameObject.SetActive(false);
    }

    public void DisplayVictoryLog()
    {
        victoryLog.gameObject.SetActive(true);
        ResourcesProcessor.UpdateResources(victoryGives, victoryGivesData);
    }

    public void DisplayDefeatLog()
    {
        defeatLog.gameObject.SetActive(true);
    }

    public override void EnableListeners()
    {
        EventMaster.current.FightWon += Victory;
        EventMaster.current.FightLost += Defeat;
    }

    public override void DisableListeners()
    {
        EventMaster.current.FightWon -= Victory;
        EventMaster.current.FightLost -= Defeat;
    }

    public void Victory()
    {
        DisplayVictoryLog();
        DisableListeners();
        fightIsOver = true;
    }

    public void Defeat()
    {
        DisplayDefeatLog();
        DisableListeners();
        fightIsOver = true;
    }

    public override void OnClickOutside()
    {
        if (fightIsOver)
        {
            SceneLoader.LoadHome();
        }
    }
}

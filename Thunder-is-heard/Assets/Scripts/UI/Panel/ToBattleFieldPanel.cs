using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBattleFieldPanel : Panel
{
    public string battleId = null;

    public void Start()
    {
        UpdateBattleInfo();

        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.ToggledOffBuildMode += Show;
    }

    public void UpdateBattleInfo()
    {
        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        if (battleTable.Items.Count > 0 ) 
        {
            battleId = battleTable.Items.First().Value.GetExternalId();
        }
        else
        {
            battleId = null;
        }
    }

    public override void Show()
    {
        if (battleId == null) return;
        base.Show();
    }

    public void Interact()
    {
        Load();
    }

    public void Load()
    {
        GameObject prefab = Resources.Load<GameObject>(Config.resources["fightProcessorPrefab"]);
        GameObject fightProcessorObj = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        FightProcessor fightProcessorComponent = fightProcessorObj.GetComponent<FightProcessor>();
        fightProcessorComponent.Init(battleId);

        DontDestroyOnLoad(fightProcessorObj);

        SceneManager.LoadScene("Fight");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Campany : ItemList
{
    public string ComponentType
    {
        get { return "Campany"; }
    }

    public List<MissionItem> items;
    public GameObject missionPrefab;
    public MissionDetalization _missionDetalization;
    public ToBattleFieldPanel toBattleFieldPanel;

    public ISubsituableCampanyBehaviour _behaviour;

    public override void Awake()
    {
        _missionDetalization.SetConductor(this);
        toBattleFieldPanel = GameObject.FindGameObjectWithTag(Tags.toBattlefieldButton).GetComponent<ToBattleFieldPanel>();
        toBattleFieldPanel.SetConductor(this);

        base.Awake();
    }

    public override void Start()
    {
        missionPrefab = Resources.Load<GameObject>(Config.resources["UIMissionItemPrefab"]);

        ChangeBehaviour();

        InitListeners();

        Hide();
    }

    public override void InitListeners()
    {
        base.InitListeners();
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset += OnResetBehaviour;
    }

    public override void Toggle()
    {
        _behaviour.Toggle(this);
    }

    public override void FillContent()
    {
        _behaviour.FillContent(this);
    }

    public void Load(MissionDetalization missionData)
    {
        _behaviour.Load(this, missionData);
    }

    public void BackToFight(string battleId)
    {
        _behaviour.BackToFight(battleId);
    }

    public override void OnClickOutside()
    {
        
    }

    public void OnSomeComponentChangeBehaviour(string componentName, string behaviourName)
    {
        if (componentName != ComponentType) return;
        ChangeBehaviour(behaviourName);
    }

    public void OnResetBehaviour()
    {
        ChangeBehaviour();
    }

    public void ChangeBehaviour(string name = "Base")
    {
        _behaviour = SubsituableCampanyFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }

    public MissionItem FindItemById(string id)
    {
        foreach (MissionItem i in items)
        {
            if (i._id == id) return i;
        }
        return null;
    }
}

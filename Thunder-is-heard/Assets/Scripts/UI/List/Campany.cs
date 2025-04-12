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

    public ISubsituableCampanyBehaviour _behaviour;

    public override void Awake()
    {
        _missionDetalization.SetConductor(this);
        base.Awake();
    }

    public override void Start()
    {
        missionPrefab = Resources.Load<GameObject>(Config.resources["UIMissionItemPrefab"]);

        ChangeBehaviour();

        InitListeners();
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
}

using System.Collections.Generic;
using UnityEngine;

public class UnitProductions : ItemList
{
    public string ComponentType
    {
        get { return "UnitProductions"; }
    }

    public List<UnitProductionItem> items;

    public string _unitProductionType;
    public string _sourceObjectId;

    public ISubsituableUnitProductionsBehaviour _behaviour;


    public override void Start()
    {   
        InitContent();

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

    public void Init(string contractType, string sourceObjectId)
    {
        _unitProductionType = contractType; _sourceObjectId = sourceObjectId;
        FillContent();
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.unitProductionItems).transform;
    }

    public override void Toggle()
    {
        _behaviour.Toggle(this);
    }

    public override void FillContent()
    {
        _behaviour.FillContent(this);
    }

    public UnitProductionItem CreateUnitProductionItem(UnitProductionCacheItem unitProductionData)
    {
        string unitId = unitProductionData.GetUnitId();

        CacheItem coreUnitAsCacheItem = Cache.LoadByType<UnitCacheTable>().GetById(unitId);
        UnitCacheItem coreUnitData = new UnitCacheItem(coreUnitAsCacheItem.Fields);
        int unitHealth = coreUnitData.GetHealth();
        int unitDamage = coreUnitData.GetDamage();
        int unitDistance = coreUnitData.GetDistance();
        int unitMobility = coreUnitData.GetMobility();

        string id = unitProductionData.GetExternalId();
        string name = unitProductionData.GetName();
        ResourcesData cost = unitProductionData.GetCost();
        int duration = unitProductionData.GetDuration();
        string description = unitProductionData.GetDescription();

        Sprite icon = ResourcesUtils.LoadIcon(unitProductionData.GetIconSection(), unitProductionData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "UnitProductionItemPrefab"], content);
        itemObject.name = name;
        UnitProductionItem unitProductionComponent = itemObject.GetComponent<UnitProductionItem>();

        unitProductionComponent.Init(
            id, 
            name, 
            _unitProductionType, 
            _sourceObjectId,
            duration, 
            cost, 
            unitId, 
            unitHealth, 
            unitDamage, 
            unitDistance, 
            unitMobility, 
            description, 
            icon
            );
        unitProductionComponent.SetConductor(this);
        return unitProductionComponent;
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
        _behaviour = SubsituableUnitProductionsFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }

    public bool IsAvailableToBuy(UnitProductionItem item)
    {
        return _behaviour.IsAvailableToBuy(item);
    }

    public void OnBuy(UnitProductionItem item)
    {
        _behaviour.OnBuy(item);
    }
}

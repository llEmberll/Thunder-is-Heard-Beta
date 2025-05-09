using System.Collections.Generic;
using UnityEngine;

public class Contracts : ItemList
{
    public static string ComponentType
    {
        get { return "Contracts"; }
    }

    public List<ContractItem> items;

    public string _contractType;
    public string _sourceObjectId;

    public ISubsituableContractsBehaviour _behaviour;


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
        _contractType = contractType; _sourceObjectId = sourceObjectId;

        FillContent();
    }

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.contractItems).transform;
    }

    public override void Toggle()
    {
        _behaviour.Toggle(this);
    }

    public override void FillContent()
    {
        _behaviour.FillContent(this);
    }


    public void Idle(ContractComponent component)
    {
        _behaviour.OnInteractWithIdleComponent(component);
    }

    public void Working(ContractComponent component)
    {
        _behaviour.OnInteractWithWorkingComponent(component);
    }

    public void Finished(ContractComponent component)
    {
        _behaviour.OnInteractWithFinishedComponent(component);
    }


    public ContractItem CreateContractItem(ContractCacheItem contractCacheData)
    {
        string id = contractCacheData.GetExternalId();
        string name = contractCacheData.GetName();
        ResourcesData cost = contractCacheData.GetCost();
        ResourcesData gives = contractCacheData.GetGives();
        int duration = contractCacheData.GetDuration();
        string description = contractCacheData.GetDescription();

        Sprite icon = ResourcesUtils.LoadIcon(contractCacheData.GetIconSection(), contractCacheData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "ContractItemPrefab"], content);
        itemObject.name = name;
        ContractItem contractComponent = itemObject.GetComponent<ContractItem>();

        contractComponent.Init(
            id, 
            name, 
            _contractType, 
            _sourceObjectId,
            duration, 
            cost, 
            gives, 
            description,
            icon
            );
        contractComponent.SetConductor(this);
        return contractComponent;
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
        _behaviour = SubsituableContractsFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }

    public bool IsAvailableToBuy(ContractItem item)
    {
        return _behaviour.IsAvailableToBuy(item);
    }

    public void OnBuy(ContractItem item)
    {
        _behaviour.OnBuy(item);
    }

    public ContractItem FindItemById(string id)
    {
        foreach (ContractItem i in items)
        {
            if (i._id == id) return i;
        }
        return null;
    }

    public ContractItem FindItemByType(string type)
    {
        foreach (ContractItem i in items)
        {
            if (i.Type == type) return i;
        }
        return null;
    }
}

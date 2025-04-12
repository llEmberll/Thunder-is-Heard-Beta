using System.Collections.Generic;
using UnityEngine;

public class Shop : ItemList
{
    public string ComponentType
    {
        get { return "Shop"; }
    }

    public List<ShopItem> items;

    public ISubsituableShopBehaviour _behaviour;


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
        EventMaster.current.ShopChanged += UpdateContent;
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset += OnResetBehaviour;
    }

    public void IncreaseItem(string id, string type, int count)
    {
        foreach (var item in _behaviour.GetItems(this))
        {
            if (item.coreId == id && item.Type == type)
            {
                item.Increment(count);
                break;
            }
        }
    }

    public override void Toggle()
    {
        _behaviour.Toggle(this);
    }

    public override void FillContent()
    {
        _behaviour.FillContent(this);
    }

    public void UpdateContent()
    {
        FillContent();
    }

    public BuildShopItem CreateBuild(ShopCacheItem shopItemData, BuildCacheItem buildData)
    {
        string id = shopItemData.GetExternalId();
        string name = buildData.GetName();
        ResourcesData cost = buildData.GetCost();
        ResourcesData gives = buildData.GetGives();
        int health = buildData.GetHealth();
        int damage = buildData.GetDamage();
        int distance = buildData.GetDistance();
        int count = shopItemData.GetCount();
        string description = buildData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(buildData.GetIconSection(), buildData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Build" + "ShopItemPrefab"], content);
        itemObject.name = name;
        BuildShopItem buildComponent = itemObject.GetComponent<BuildShopItem>();

        buildComponent.Init(
            id, 
            name, 
            cost, 
            gives, 
            health, 
            damage, 
            distance, 
            count, 
            description, 
            icon
            );
        buildComponent.SetConductor(this);
        return buildComponent;
    }

    public UnitShopItem CreateUnit(ShopCacheItem ShopItemData, UnitCacheItem unitData)
    {
        string id = ShopItemData.GetExternalId();
        string name = unitData.GetName();
        ResourcesData cost = unitData.GetCost();
        ResourcesData gives = unitData.GetGives();
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = ShopItemData.GetCount();
        string description = unitData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(unitData.GetIconSection(), unitData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Unit" + "ShopItemPrefab"], content);
        itemObject.name = name;
        UnitShopItem unitComponent = itemObject.GetComponent<UnitShopItem>();

        unitComponent.Init(
            id, 
            name, 
            cost,
            gives, 
            health, 
            damage, 
            distance, 
            mobility,
            count,
            description, 
            icon
            );
        unitComponent.SetConductor(this);
        return unitComponent;
    }

    public MaterialShopItem CreateMaterial(ShopCacheItem ShopItemData, MaterialCacheItem materialData)
    {
        string id = ShopItemData.GetExternalId();
        string name = materialData.GetName();
        ResourcesData cost = materialData.GetCost();
        int count = ShopItemData.GetCount();
        string description = materialData.GetDescription();
        Sprite icon = ResourcesUtils.LoadIcon(materialData.GetIconSection(), materialData.GetIconName());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Material" + "ShopItemPrefab"], content);
        itemObject.name = name;
        MaterialShopItem materialComponent = itemObject.GetComponent<MaterialShopItem>();

        materialComponent.Init(id, name, cost, count, description, icon);
        materialComponent.SetConductor(this);
        return materialComponent;
    }

    

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.shopItems).transform;
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
        _behaviour = SubsituableShopFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }

    public bool IsAvailableToBuy(ShopItem item)
    {
        return _behaviour.IsAvailableToBuy(item);
    }

    public void OnBuy(ShopItem item)
    {
        _behaviour.OnBuy(item);
    }
}

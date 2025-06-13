using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : ItemList, IItemConductor
{
    public string ComponentType
    {
        get { return "Inventory"; }
    }

    public List<InventoryItem> items;

    public ISubsituableInventoryBehaviour _behaviour;

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
        EventMaster.current.InventoryChanged += UpdateContent;
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

    public void InitContent()
    {
        content = GameObject.FindGameObjectWithTag(Tags.inventoryItems).transform;
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
        _behaviour = SubsituableInventoryFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }

    public void OnUse(InventoryItem item)
    {
        _behaviour.OnUse(item);
    }

    public void CreatePreview(ExposableInventoryItem item)
    {
        _behaviour.CreatePreview(this, item);
    }

    public void OnObjectExposed(ExposableInventoryItem item, Entity obj)
    {
        _behaviour.OnObjectExposed(this, item, obj);
    }

    public void Substract(InventoryItem item, int number = 1)
    {
        _behaviour.Substract(this, item, number);
    }

    public InventoryItem FindItemById(string id)
    {
        foreach (InventoryItem i in items)
        {
            if (i._id == id) return i;
        }
        return null;
    }

    public InventoryItem FindItemByCoreId(string id)
    {
        foreach (InventoryItem i in items)
        {
            if (i.coreId == id) return i;
        }
        return null;
    }

    public void OnPointerEnter(InventoryItem item, PointerEventData eventData)
    {
        if (item is LandableUnit landableUnit)
        {
            if (!landableUnit.focusOn)
            {
                landableUnit.focusOn = true;
                // Здесь можно добавить специфичную логику для Inventory
            }
        }
    }

    public void OnPointerExit(InventoryItem item, PointerEventData eventData)
    {
        if (item is LandableUnit landableUnit)
        {
            if (landableUnit.focusOn)
            {
                landableUnit.focusOn = false;
                // Здесь можно добавить специфичную логику для Inventory
            }
        }
    }
}

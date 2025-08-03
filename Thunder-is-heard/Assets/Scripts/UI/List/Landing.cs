using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Landing : ItemList, IItemConductor
{
    public string ComponentType
    {
        get { return "Landing"; }
    }

    public List<LandableUnit> items;

    public TMP_Text TmpHealth;
    public TMP_Text TmpDamage;
    public TMP_Text TmpDistance;
    public TMP_Text TmpMobility;

    public TMP_Text TmpStaff;
    public int _maxStaff;
    public int _landedStaff;

    public Map _map;
    public UnitsOnFight _unitsOnScene;

    public Image notLandedStaffForFightWarning;

    public ISubsituableLandingBehaviour _behaviour;


    public override void Awake()
    {
       
    }

    public override void EnableListeners()
    {
        EventMaster.current.ToggledOffBuildMode += Show;
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.StartLanding += StartLanding;
        EventMaster.current.ToBattleButtonPressed += OnPressedToBattleButton;
        EventMaster.current.FightIsContinued += FinishLanding;
        EventMaster.current.LandableUnitFocused += OnLandableUnitFocus;
        EventMaster.current.LandableUnitDefocused += OnLandableUnitDefocus;
        EventMaster.current.BattleObjectRemoved += OnObjectRemoved;
        EventMaster.current.ObjectExposed += OnObjectLanded;
        EventMaster.current.ComponentBehaviourChanged += OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset += OnResetBehaviour;
    }

    public override void DisableListeners()
    {
        EventMaster.current.ToggledOffBuildMode -= Show;
        EventMaster.current.ToggledToBuildMode -= Hide;
        EventMaster.current.StartLanding -= StartLanding;
        EventMaster.current.ToBattleButtonPressed -= OnPressedToBattleButton;
        EventMaster.current.FightIsContinued -= FinishLanding;
        EventMaster.current.LandableUnitFocused -= OnLandableUnitFocus;
        EventMaster.current.LandableUnitDefocused -= OnLandableUnitDefocus;
        EventMaster.current.BattleObjectRemoved -= OnObjectRemoved;
        EventMaster.current.ObjectExposed -= OnObjectLanded;
        EventMaster.current.ComponentBehaviourChanged -= OnSomeComponentChangeBehaviour;
        EventMaster.current.ComponentsBehaviourReset -= OnResetBehaviour;
    }

    public override void Start()
    {
        Debug.Log("Landing: start");

        InitContent();
        InitMap();
        InitUnitsOnScene();

        InitReadings();
        InitStaffIndicator();

        ChangeBehaviour("Disabled");

        InitListeners();

        Hide();
    }

    public void InitReadings()
    {
        TmpHealth.text = "";
        TmpDamage.text = "";
        TmpDistance.text = "";
        TmpMobility.text = "";
    }

    public void InitStaffIndicator()
    {
        UpdateLandedStaff();
        UpdateStaffText();
    }

    public void InitMap()
    {
        _map = GameObjectUtils.FindComponentByTagIncludingInactive<Map>(Tags.map);
    }

    public void InitUnitsOnScene()
    {
        _unitsOnScene = GameObjectUtils.FindComponentByTagIncludingInactive<UnitsOnFight>(Tags.unitsOnScene);
    }

    public void OnObjectRemoved(Entity entity)
    {
        _behaviour.OnObjectRemoved(this, entity);
    }

    public void OnObjectLanded(Entity entity)
    {
        _behaviour.OnObjectLanded(this, entity);
    }

    public bool IsProperType(string text)
    {
        return text.Contains("Unit");
    }

    public void UpdateObjects()
    {
        FillContent();
    }



    public void UpdateStaffText()
    {
        TmpStaff.text = _landedStaff.ToString() +"/" + _maxStaff.ToString();
    }

    public void ChangeLandedStaff(int value)
    {
        _landedStaff = value;
    }

    public void UpdateLandedStaff()
    {
        _landedStaff = 0;

        List<Unit> federationUnits = _unitsOnScene.GetUnitsBySide(Sides.federation);
        foreach (Unit unit in federationUnits)
        {
            _landedStaff += Unit.GetStaffByUnit(unit);
        }
    }

    public void StartLanding(LandingData landingData)
    {
        _behaviour.StartLanding(this, landingData);
    }

    public void OnPressedToBattleButton()
    {
        _behaviour.OnPressedToBattleButton(this);
    }

    public void FinishLanding()
    {
        _behaviour.FinishLanding(this);
    }

    public void OnLandableUnitFocus(LandableUnit target)
    {
        _behaviour.OnLandableUnitFocus(this, target);
    }

    public void OnLandableUnitDefocus(LandableUnit target)
    {
        _behaviour.OnLandableUnitDefocus(this, target);
    }

    public override void Update()
    {
        
    }

    public override void OnClickOutside()
    {
        
    }

    public override void FillContent()
    {
        _behaviour.FillContent(this);
    }

    public void InitContent()
    {
        content = GameObjectUtils.FindChildObjectByTag(this.gameObject, Tags.landableUnitsContent);
    }

    public override void Show()
    {
        this.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
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
        Debug.Log("Landing: ChangeBehaviour: " + name);
        _behaviour = SubsituableLandingFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }

    public void OnPointerEnter(InventoryItem item, PointerEventData eventData)
    {
        if (item is LandableUnit landableUnit)
        {
            if (!landableUnit.focusOn)
            {
                landableUnit.focusOn = true;
                EventMaster.current.OnLandableUnitFocus(landableUnit);
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
                EventMaster.current.OnLandableUnitDefocus(landableUnit);
            }
        }
    }

    public void OnUse(InventoryItem item)
    {
        _behaviour.OnUse(this, item);
    }

    public void CreatePreview(ExposableInventoryItem item)
    {
        _behaviour.CreatePreview(this, item);
    }

    public void OnObjectExposed(ExposableInventoryItem item, Entity obj)
    {
        _behaviour.OnObjectExposed(this, item, obj);
    }

    public void OnInventoryItemAdded(InventoryItem sourceItem, InventoryCacheItem addedItem)
    {
        _behaviour.OnInventoryItemAdded(this, sourceItem, addedItem);
    }

    public void Substract(InventoryItem item, int number = 1)
    {
        _behaviour.Substract(this, item, number);
    }

    public void Increment(InventoryItem item, int number = 1)
    {
        _behaviour.Increment(this, item, number);
    }

    public LandableUnit FindItemByCoreId(string coreId)
    {
        foreach (LandableUnit item in items)
        {
            if (item.coreId == coreId) return item;
        }
        return null;
    }
}

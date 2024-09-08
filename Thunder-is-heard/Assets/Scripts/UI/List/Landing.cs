using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Landing : ItemList
{
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


    public override void Awake()
    {
       
    }

    public override void EnableListeners()
    {
        EventMaster.current.ToggledOffBuildMode += Show;
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.StartLanding += StartLanding;
        EventMaster.current.FightIsStarted += FinishLanding;
        EventMaster.current.LandableUnitFocused += OnLandableUnitFocus;
        EventMaster.current.LandableUnitDefocused += OnLandableUnitDefocus;
        EventMaster.current.BattleObjectRemoved += OnObjectRemoved;
        EventMaster.current.ObjectExposed += OnObjectLanded;
    }

    public override void DisableListeners()
    {
        EventMaster.current.ToggledOffBuildMode -= Show;
        EventMaster.current.ToggledToBuildMode -= Hide;
        EventMaster.current.StartLanding -= StartLanding;
        EventMaster.current.FightIsStarted -= FinishLanding;
        EventMaster.current.LandableUnitFocused -= OnLandableUnitFocus;
        EventMaster.current.LandableUnitDefocused -= OnLandableUnitDefocus;
        EventMaster.current.BattleObjectRemoved -= OnObjectRemoved;
        EventMaster.current.ObjectExposed -= OnObjectLanded;
    }

    public override void Start()
    {
        Debug.Log("Landing: awake");

        InitContent();
        InitMap();
        InitUnitsOnScene();

        InitReadings();
        InitStaffIndicator();
        

        base.Start();

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
        _map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
    }

    public void InitUnitsOnScene()
    {
        _unitsOnScene = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
    }

    public void OnObjectRemoved(Entity entity)
    {
        if (!IsProperType(entity.Type)) return;
        ChangeLandedStaff(_landedStaff - Unit.GetStaffByUnit(entity.gameObject.GetComponent<Unit>()));
        UpdateStaffText();
    }

    public void OnObjectLanded(Entity entity)
    {
        if (!IsProperType(entity.Type)) return;
        ChangeLandedStaff(_landedStaff + Unit.GetStaffByUnit(entity.gameObject.GetComponent<Unit>()));
        UpdateStaffText();
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

    public void StartLanding(List<Vector2Int> landableZone, int maxStaff)
    {
        Debug.Log("Landing: start landing");

        _maxStaff = maxStaff;
        UpdateLandedStaff();
        UpdateStaffText();

        _map.SetInactiveAll();
        _map.SetActive(landableZone);
        _map.Display(landableZone);

        Show();
    }

    public void FinishLanding()
    {
        Debug.Log("Landing: finish landing");

        DisableListeners();

        _map.SetActiveAll();
        _map.HideAll();

        Hide();
    }

    public void OnLandableUnitFocus(LandableUnit target)
    {
        TmpHealth.text = target.health.ToString();
        TmpDamage.text = target.damage.ToString();
        TmpDistance.text = target.distance.ToString();
        TmpMobility.text = target.mobility.ToString();
    }

    public void OnLandableUnitDefocus(LandableUnit target)
    {
        InitReadings();
    }

    public override void Update()
    {
        
    }

    public override void OnClickOutside()
    {
        
    }

    public override void FillContent()
    {
        ClearItems();
        items = new List<LandableUnit>();

        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        foreach (var keyValuePair in inventoryTable.Items)
        {
            InventoryCacheItem inventoryItemData = new InventoryCacheItem(keyValuePair.Value.Fields);
            string type = inventoryItemData.GetType();

            CacheTable itemTable = Cache.LoadByName(type);
            CacheItem item = itemTable.GetById(inventoryItemData.GetCoreId());

            switch (type)
            {
                case "Unit":
                    UnitCacheItem unitData = new UnitCacheItem(item.Fields);
                    LandableUnit unit = CreateUnit(inventoryItemData, unitData);
                    items.Add(unit);
                    break;
            }
        }
    }

    public LandableUnit CreateUnit(InventoryCacheItem inventoryItemData, UnitCacheItem unitData)
    {
        string id = inventoryItemData.GetExternalId();
        string name = unitData.GetName();
        int staff = unitData.GetGives().staff;
        int health = unitData.GetHealth();
        int damage = unitData.GetDamage();
        int distance = unitData.GetDistance();
        int mobility = unitData.GetMobility();
        int count = inventoryItemData.GetCount();
        Sprite icon = Resources.Load<Sprite>(unitData.GetIconPath());

        GameObject itemObject = CreateObject(Config.resources["UI" + "Unit" + "LandablePrefab"], content);
        itemObject.name = name;
        LandableUnit unitComponent = itemObject.GetComponent<LandableUnit>();

        unitComponent.Init(
            id,
            name,
            staff,
            health,
            damage,
            distance,
            mobility,
            count,
            icon
            );
        return unitComponent;
    }

    public void InitContent()
    {
        content = GameObjectUtils.FindChildObjectByTag(this.gameObject, Tags.landableUnits);
    }

    public override void Show()
    {
        this.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        this.gameObject.SetActive(false);
    }
}

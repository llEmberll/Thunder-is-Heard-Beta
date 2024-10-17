using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsOnFight : ObjectsOnFight, IObjectsOnScene
{
    [SerializeField] public Dictionary<string, Unit> items = new Dictionary<string, Unit>();

    public string _battleId;


    public override void Awake()
    {
        
    }

    public override void Start()
    {
        this.content = this.transform;

        _battleId = FightSceneLoader.parameters._battleId;
        base.Start();
    }


    public override void EnableListeners()
    {
        EventMaster.current.BattleObjectRemoved += OnBattleObjectRemoved;
        EventMaster.current.ObjectExposed += OnBattleObjectExposed;
    }

    public override void DisableListeners()
    {
        EventMaster.current.BattleObjectRemoved -= OnBattleObjectRemoved;
        EventMaster.current.ObjectExposed -= OnBattleObjectExposed;
    }

    public void OnBattleObjectRemoved(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (!items.ContainsKey(obj.ChildId)) return;
        Destroy(items[obj.ChildId]);
        items.Remove(obj.ChildId);
    }

    public void OnBattleObjectExposed(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (items.ContainsKey(obj.ChildId)) return;
        items.Add(obj.ChildId, obj.gameObject.GetComponent<Unit>());
    }

    public override bool IsProperType(string type)
    {
        return type.Contains("Unit");
    }

    public void UpdateObjects()
    {
        FillContent();
    }

    public override void FillContent()
    {
        FillUnits();
    }

    public void FillUnits()
    {
        ClearItems();
        items = new Dictionary<string, Unit>();

        CacheItem battleCacheItem = Cache.LoadByType<BattleCacheTable>().GetById(_battleId);
        BattleCacheItem battleData = new BattleCacheItem(battleCacheItem.Fields);
        UnitOnBattle[] units = battleData.GetUnits();
        foreach (UnitOnBattle unit in units)
        {
            MappingUnit(unit);
        }
    }

    public void MappingUnit(UnitOnBattle battleUnitData)
    {
        string side = battleUnitData.side;

        UnitCacheTable unitsTable = Cache.LoadByType<UnitCacheTable>();
        CacheItem unitAsCacheItem = unitsTable.GetById(battleUnitData.coreId);
        UnitCacheItem coreUnitData = new UnitCacheItem(unitAsCacheItem.Fields);
        string modelPath = coreUnitData.GetModelPath() + "/" + side;
        Bector2Int size = coreUnitData.GetSize();
        int maxHealth = battleUnitData.maxHealth;
        int currentHealth = battleUnitData.health;
        int damage = battleUnitData.damage;
        int distance = battleUnitData.distance;
        int mobility = battleUnitData.mobility;
        string unitType = coreUnitData.GetUnitType();
        string doctrine = coreUnitData.GetDoctrine();
        float movementSpeed = coreUnitData.GetMovementSpeed();

        string name = coreUnitData.GetName();
        Bector2Int[] position = new Bector2Int[1] { battleUnitData.position };
        int rotation = battleUnitData.rotation;
        string coreId = battleUnitData.coreId;
        string childId = battleUnitData.idOnBattle;


        GameObject unitObj = ObjectProcessor.CreateUnitObject(position[0].ToVector2Int(), name, this.transform);
        GameObject unitModel = ObjectProcessor.CreateModel(modelPath, rotation, unitObj.transform);
        
        map.Occypy(Bector2Int.MassiveToVector2Int(position).ToList());

        ObjectProcessor.AddAndPrepareUnitComponent(
            unitObj,
            unitModel.transform,
            coreId,
            childId,
            name,
            size.ToVector2Int(),
            Bector2Int.MassiveToVector2Int(position),
            maxHealth,
            currentHealth,
            damage,
            distance,
            mobility,
            side,
            unitType,
            doctrine,
            movementSpeed
            );

        items.Add(childId, unitObj.GetComponent<Unit>());
    }

    public override Entity FindObjectByCoreId(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Unit childEntity = transform.GetChild(i).GetComponent<Unit>();
            if (childEntity != null && childEntity.CoreId == id)
            {
                return childEntity;
            }
        }
        return null;
    }

    public override Entity FindObjectByChildId(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Unit childEntity = transform.GetChild(i).GetComponent<Unit>();
            if (childEntity != null && childEntity.ChildId == id)
            {
                return childEntity;
            }
        }
        return null;
    }

    public List<Unit> GetUnitsBySide(string side)
    {
        List<Unit> units = new List<Unit>();

        foreach (var keyValuePair in items)
        {
            if (keyValuePair.Value.side == side)
            {
                units.Add(keyValuePair.Value);
            }
        }

        return units;
    }

    public List<Unit> GetUnitsByBattleUnitsData(UnitOnBattle[] unitsData)
    {
        List<Unit> units = new List<Unit>();

        foreach (var unitData in unitsData)
        {
            if (items.ContainsKey(unitData.idOnBattle))
            {
                units.Add(items[unitData.idOnBattle]);
            }
        }

        return units;
    }
}

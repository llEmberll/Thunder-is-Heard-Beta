using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsOnBase : ObjectsOnBase
{
    [SerializeField] public Dictionary<string, Unit> items = new Dictionary<string, Unit>();


    public override void Awake()
    {
        
    }

    public override void Start()
    {
        this.content = this.transform;
        base.Start();
    }

    public override void EnableListeners()
    {
        EventMaster.current.BaseObjectRemoved += OnBaseObjectRemoved;
        EventMaster.current.ObjectExposed += OnBaseObjectExposed;
    }

    public override void DisableListeners()
    {
        EventMaster.current.BaseObjectRemoved -= OnBaseObjectRemoved;
        EventMaster.current.ObjectExposed -= OnBaseObjectExposed;
    }

    public void OnBaseObjectRemoved(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (!items.ContainsKey(obj.ChildId)) return;
        items.Remove(obj.ChildId);
        Destroy(obj.gameObject);
    }

    public void OnBaseObjectExposed(Entity obj)
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
        Debug.Log("Update unit objects");

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

        PlayerUnitCacheTable playerUnitsTable = Cache.LoadByType<PlayerUnitCacheTable>();
        foreach (var pair in playerUnitsTable.Items)
        {
            PlayerUnitCacheItem currentPlayerBuild = new PlayerUnitCacheItem(pair.Value.Fields);
            MappingUnit(currentPlayerBuild);
        }
    }

    private void MappingUnit(PlayerUnitCacheItem playerUnitData)
    {
        UnitCacheTable unitsTable = Cache.LoadByType<UnitCacheTable>();
        CacheItem unitAsCacheItem = unitsTable.GetById(playerUnitData.GetCoreId());
        UnitCacheItem coreUnitData = new UnitCacheItem(unitAsCacheItem.Fields);
        string modelPath = coreUnitData.GetModelPath()  + "/" + Tags.federation;
        Bector2Int size = coreUnitData.GetSize();
        int health = coreUnitData.GetHealth();
        int damage = coreUnitData.GetDamage();
        int distance = coreUnitData.GetDistance();
        int mobility = coreUnitData.GetMobility();
        string unitType = coreUnitData.GetUnitType();
        string doctrine = coreUnitData.GetDoctrine();
        float movementSpeed = coreUnitData.GetMovementSpeed();

        string name = playerUnitData.GetName();
        Bector2Int[] position = playerUnitData.GetPosition();
        int rotation = playerUnitData.GetRotation();
        string coreId = playerUnitData.GetCoreId();
        string childId = playerUnitData.GetExternalId();

        GameObject unitObj = ObjectProcessor.CreateEntityObject(position[0].ToVector2Int(), name, this.transform);
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
            health, 
            health,
            damage, 
            distance, 
            mobility, 
            Sides.federation,
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
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsOnFight : ObjectsOnFight, IObjectsOnScene
{
    public Dictionary<string, Unit> items;

    public FightProcessor fightProcessor;


    public override void Start()
    {
        fightProcessor = GameObject.FindGameObjectWithTag(Tags.fightProcessor).GetComponent<FightProcessor>();
        map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        base.Start();
    }


    public override void OnBuildModeEnable()
    {
    }

    public override void FillContent()
    {
        base.FillContent();
        FillUnits();
    }

    public void FillUnits()
    {
        items = new Dictionary<string, Unit>();

        UnitOnBattle[] units = fightProcessor._battleData.GetUnits();
        foreach (UnitOnBattle unit in units)
        {
            MappingUnit(unit);
        }
    }

    private void MappingUnit(UnitOnBattle battleUnitData)
    {
        string side = battleUnitData.side;

        UnitCacheTable unitsTable = Cache.LoadByType<UnitCacheTable>();
        CacheItem unitAsCacheItem = unitsTable.GetById(battleUnitData.coreId);
        UnitCacheItem coreUnitData = new UnitCacheItem(unitAsCacheItem.Fields);
        string modelPath = coreUnitData.GetModelPath() + "/" + side;
        Bector2Int size = coreUnitData.GetSize();
        int maxHealth = coreUnitData.GetHealth();
        int currentHealth = battleUnitData.health;
        int damage = coreUnitData.GetDamage();
        int distance = coreUnitData.GetDistance();
        int mobility = coreUnitData.GetMobility();

        string name = coreUnitData.GetName();
        Bector2Int[] position = new Bector2Int[1] { battleUnitData.position };
        int rotation = battleUnitData.rotation;
        string coreId = battleUnitData.coreId;
        string childId = battleUnitData.idOnBattle;


        GameObject unitObj = ObjectProcessor.CreateUnitObject(position[0].ToVector2Int(), name, this.transform);
        GameObject unitModel = ObjectProcessor.CreateUnitModel(modelPath, rotation, unitObj.transform);

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
            side
            );
    }

    public override Entity FindObjectById(string id)
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
}

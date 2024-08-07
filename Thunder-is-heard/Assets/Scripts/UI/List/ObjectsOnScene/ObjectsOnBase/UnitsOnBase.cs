using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsOnBase : ObjectsOnBase
{
    public Dictionary<string, Unit> items;

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

        string name = playerUnitData.GetName();
        Bector2Int[] position = playerUnitData.GetPosition();
        int rotation = playerUnitData.GetRotation();
        string coreId = playerUnitData.GetCoreId();
        string childId = playerUnitData.GetExternalId();


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
            health, 
            health,
            damage, 
            distance, 
            mobility, 
            Sides.federation
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

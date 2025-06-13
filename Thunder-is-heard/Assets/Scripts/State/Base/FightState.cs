

using System.Collections.Generic;
using UnityEngine;

public class FightState: State
{
    public override string stateName
    {
        get {
            return "Fight";
        }
    }
    
    public override void Enter()
    {
        
    }

    public override void HandleInput()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void Exit()
    {

    }

    public override void OnBuildClick(Build build)
    {
        EventMaster.current.OnObjectClick(build);
    }

    public override void OnBuildMouseEnter(Build build)
    {
        EventMaster.current.OnObjectEnter(build);
    }

    public override void OnBuildMouseExit(Build build)
    {
        EventMaster.current.OnObjectExit(build);
    }


    public override void OnUnitClick(Unit unit)
    {
        EventMaster.current.OnObjectClick(unit);
    }

    public override void OnUnitMouseEnter(Unit unit)
    {
        EventMaster.current.OnObjectEnter(unit);
    }

    public override void OnUnitMouseExit(Unit unit)
    {
        EventMaster.current.OnObjectExit(unit);
    }


    public override void OnCellClick(Cell cell)
    {
        EventMaster.current.OnCellClick(cell);
    }

    public override void OnCellMouseEnter(Cell cell)
    {
        EventMaster.current.OnCellEnter(cell);
    }

    public override void OnCellMouseExit(Cell cell)
    {
        EventMaster.current.OnCellExit(cell);
    }


    public override void OnObstacleClick(Obstacle obstacle)
    {

    }

    public override void OnObstacleMouseEnter(Obstacle obstacle)
    {
        EventMaster.current.OnObjectEnter(obstacle);
    }

    public override void OnObstacleMouseExit(Obstacle obstacle)
    {
        EventMaster.current.OnObjectExit(obstacle);
    }


    public override bool IsCellMustBeVisible(Cell cell)
    {
        return false;
    }


    public override void OnCreatePreviewObject(ObjectPreview preview)
    {
        preview.CreateObjectOnBattle();
    }

    public override void OnReplacePreviewObject(ObjectPreview preview)
    {
        preview.ReplaceObjectOnBattle();
    }


    public override int GetMaxStaff()
    {
        string battleId = FightSceneLoader.parameters._battleId;

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        BattleCacheItem battleData = new BattleCacheItem(cacheItem.Fields);

        StageData currentStage = battleData.GetCurrentStage();
        if (currentStage == null) return 100;
        if (currentStage.landingData == null) return 100;
        return currentStage.landingData.maxStaff;
    }

    public override int GetStaff()
    {
        int staff = 0;

        UnitsOnFight unitsOnScene = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
        List<Unit> federationUnits = unitsOnScene.GetUnitsBySide(Sides.federation);
        foreach (Unit unit in federationUnits)
        {
            staff += Unit.GetStaffByUnit(unit);
        }

        return staff;
    }
}
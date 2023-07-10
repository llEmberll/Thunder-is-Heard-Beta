
using UnityEngine;

public class BuildingState : State
{
    public override string stateName
    {
        get {
            return "Building";
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
        
    }

    public override void OnBuildMouseEnter(Build build)
    {
        
    }

    public override void OnBuildMouseExit(Build build)
    {
        
    }

    public override void OnUnitClick(Unit unit)
    {
        
    }

    public override void OnUnitMouseEnter(Unit unit)
    {
        
    }

    public override void OnUnitMouseExit(Unit unit)
    {
        
    }


    public override void OnCellClick(Cell cell)
    {
        ObjectPreview preview = GameObject.FindWithTag("Preview").GetComponent<ObjectPreview>();
        preview.Expose();
    }

    public override void OnCellMouseEnter(Cell cell)
    {
        ObjectPreview preview = GameObject.FindWithTag("Preview").GetComponent<ObjectPreview>();
        preview.Move(cell.position);
    }

    public override void OnCellMouseExit(Cell cell)
    {
        
    }
}
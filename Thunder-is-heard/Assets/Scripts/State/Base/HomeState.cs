

using System.Drawing;

public class HomeState: State
{
    public override string stateName
    {
        get {
            return "Home";
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
        build.InteractionComponent.Interact(build.workStatus);
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
        
    }

    public override void OnCellMouseEnter(Cell cell)
    {
        
    }

    public override void OnCellMouseExit(Cell cell)
    {
        
    }

    public override bool IsCellMustBeVisible(Cell cell)
    {
        return false;
    }

    public override void OnCreatePreviewObject(ObjectPreview preview)
    {
        preview.CreateObjectOnBase();
    }

    public override void OnReplacePreviewObject(ObjectPreview preview)
    {
        preview.ReplaceObjectOnBattle();
    }
}


public abstract class State
{
    
    public abstract string stateName { get; }
    
    public virtual void Enter()
    {
        
    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void Exit()
    {

    }

    public abstract void OnBuildClick(Build build);

    public abstract void OnBuildMouseEnter(Build build);

    public abstract void OnBuildMouseExit(Build build);


    public abstract void OnUnitClick(Unit unit);

    public abstract void OnUnitMouseEnter(Unit unit);

    public abstract void OnUnitMouseExit(Unit unit);


    public abstract void OnCellClick(Cell cell);

    public abstract void OnCellMouseEnter(Cell cell);

    public abstract void OnCellMouseExit(Cell cell);

    public abstract bool IsCellMustBeVisible(Cell cell);


    public abstract void OnCreatePreviewObject(ObjectPreview preview);
    public abstract void OnReplacePreviewObject(ObjectPreview preview);

    public abstract int GetMaxStaff();
    public abstract int GetStaff();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : State
{
    public ObjectPreview preview;

    public override string stateName
    {
        get {
            return "Building";
        }
    }
    
    public void OnCreatePreview(ObjectPreview createdPreview)
    {
        preview = createdPreview;
    }

    public void OnDeletePreview()
    {
        preview = null;
    }

    public override void Enter()
    {
        EventMaster.current.PreviewCreated += OnCreatePreview;
        EventMaster.current.PreviewDeleted += OnDeletePreview;
    }

    public override void HandleInput()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void Exit()
    {
        EventMaster.current.PreviewCreated -= OnCreatePreview;
        EventMaster.current.PreviewDeleted -= OnDeletePreview;
    }

    public override void OnBuildClick(Build build)
    {
        if (preview == null && build.side == Sides.federation)
        {
            Transform model = build.model;

            ObjectPreview preview = ObjectPreview.Create();
            preview.Init(build.name, "Build", build.CoreId, build.currentSize, model);
        }
    }

    public override void OnBuildMouseEnter(Build build)
    {
        
    }

    public override void OnBuildMouseExit(Build build)
    {
        
    }

    public override void OnUnitClick(Unit unit)
    {
        {
            if (preview == null && unit.side == Sides.federation)
            {
                Transform model = unit.model;

                ObjectPreview preview = ObjectPreview.Create();
                preview.Init(unit.name, "Unit", unit.CoreId, unit.currentSize, model);
            }
        }
    }

    public override void OnUnitMouseEnter(Unit unit)
    {
        
    }

    public override void OnUnitMouseExit(Unit unit)
    {
        
    }


    public override void OnCellClick(Cell cell)
    {
        if (preview != null) 
        {
            preview.Expose();
        }
    }

    public override void OnCellMouseEnter(Cell cell)
    {
        if (preview != null) 
        {
            preview.Move(cell.position);
        }
    }

    public override void OnCellMouseExit(Cell cell)
    {
        
    }

    public override bool IsCellMustBeVisible(Cell cell)
    {
        return !cell.occupied;
    }

    public override void OnCreatePreviewObject(ObjectPreview preview)
    {
    }

    public override void OnReplacePreviewObject(ObjectPreview preview)
    {
    }

    public override int GetMaxStaff()
    {
        throw new System.NotImplementedException();
    }

    public override int GetStaff()
    {
        throw new System.NotImplementedException();
    }
}
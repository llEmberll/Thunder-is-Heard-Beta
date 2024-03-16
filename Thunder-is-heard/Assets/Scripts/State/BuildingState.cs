using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

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
        if (preview == null)
        {
            Transform model = build.transform.Find("Model");

            ObjectPreview preview = ObjectPreview.Create();
            preview.Init(build.name, "PlayerBuild", build.Id, build.currentSize, model);
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
}
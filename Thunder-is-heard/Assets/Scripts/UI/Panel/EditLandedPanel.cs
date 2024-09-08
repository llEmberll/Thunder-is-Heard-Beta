using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLandedPanel : Panel
{
    public void Start()
    {
        InitListeners();
        Hide();
    }

    public void InitListeners()
    {
        EnableListeners();
    }

    public void EnableListeners()
    {
        EventMaster.current.StartLanding += OnStartLanding;
        EventMaster.current.FightIsStarted += OnFinishLanding;
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.ToggledOffBuildMode += Show;
    }

    public void DisableListeners()
    {
        EventMaster.current.ToggledToBuildMode -= Hide;
        EventMaster.current.ToggledOffBuildMode -= Show;
        EventMaster.current.FightIsStarted -= OnFinishLanding;
    }

    public void OnStartLanding(List<Vector2Int> landableZone, int maxStaff)
    {
        Show();
    }

    public void OnFinishLanding()
    {
        DisableListeners();
        Hide();
    }
}

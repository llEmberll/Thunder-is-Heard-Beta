using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPanel : Panel
{
    public void Start()
    {
        EventMaster.current.ToggledToBuildMode += Show;
        EventMaster.current.ToggledOffBuildMode += Hide;

        Hide();
    }
}

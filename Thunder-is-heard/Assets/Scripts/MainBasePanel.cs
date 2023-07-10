using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBasePanel : Panel
{
    public void Start()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
        EventMaster.current.ToggledOffBuildMode += Show;
    }
}

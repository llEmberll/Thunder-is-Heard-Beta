using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerBuildCacheTable : CacheTable
{
    public string name = "PlayerBuild";

    public override string Name { get { return name; } }
}

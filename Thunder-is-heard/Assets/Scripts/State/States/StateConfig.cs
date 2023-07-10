using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateConfig
{
    public static readonly FightState fightState = new FightState();
    public static readonly HomeState homeState = new HomeState();
    public static readonly BuildingState buildingState = new BuildingState();

    public static readonly Dictionary<string, State> statesByScene = new Dictionary<string, State>()
        {
            {"Home", homeState},
            {"Fight", fightState}
        }
        ;
}

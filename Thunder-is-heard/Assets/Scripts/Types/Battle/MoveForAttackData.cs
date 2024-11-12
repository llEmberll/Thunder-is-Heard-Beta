using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveForAttackData
{
    public ObjectOnBattle _target;
    public UnitOnBattle _activeUnit;
    public List<Bector2Int> _fullRoute;
    public int _turnCountForReach;


    public MoveForAttackData() { }    

    public MoveForAttackData(ObjectOnBattle target, UnitOnBattle activeUnit, List<Bector2Int> fullRoute, int turnCountForReach)
    {
        this._target = target;
        this._activeUnit = activeUnit;
        this._fullRoute = fullRoute;
        this._turnCountForReach = turnCountForReach;
    }
}

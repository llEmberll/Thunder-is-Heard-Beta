using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class TurnController : MonoBehaviour
{
    public TurnData _turnData;

    public Map _map;

    public BattleEngine _battleEngine;

    public void EnableStartListeners()
    {
        EventMaster.current.FightIsStarted += OnStartFight;
    }

    public void DisableStartListeners()
    {
        EventMaster.current.FightIsStarted -= OnStartFight;
    }


    public void EnableNextTurnListener()
    {
        EventMaster.current.NextTurn += OnNextTurn;
    }

    public void DisableNextTurnListener()
    {
        EventMaster.current.NextTurn -= OnNextTurn;
    }


    public void EnableObjectClickListener()
    {
        EventMaster.current.ClickedOnObject += OnObjectClick;
    }

    public void DisableObjectClickListener()
    {
        EventMaster.current.ClickedOnObject -= OnObjectClick;
    }


    public void EnableBuildRouteListeners()
    {
        EventMaster.current.ClickedOnCell += OnCellClick;
        EventMaster.current.EnteredOnCell += OnCellEnter;
        EventMaster.current.ExitedOnCell += OnCellExit;
    }

    public void DisableBuildRouteListeners()
    {
        EventMaster.current.ClickedOnCell -= OnCellClick;
        EventMaster.current.EnteredOnCell -= OnCellEnter;
        EventMaster.current.ExitedOnCell -= OnCellExit;
    }


    public void InitMap()
    {
        _map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void Start()
    {
        InitMap();
        InitBattleEngine();

        EnableStartListeners();
        ClearTurnData();
    }

    public void OnStartFight()
    {
        DisableStartListeners();
        EnableNextTurnListener();
        OnNextTurn(_battleEngine.currentBattleSituation._sideTurn);
    }

    public void OnNextTurn(string side)
    {
        ClearTurnData();
        if (side == Sides.federation)
        {
            EnableObjectClickListener();
            EnableBuildRouteListeners();
        }
        else
        {
            DisableObjectClickListener();
            DisableBuildRouteListeners();
        }
    }

    public void OnObjectClick(Entity obj)
    {
        Debug.Log("Turn controller: OBJECT CLICK");

        if (obj.side == Sides.neutral)
        {
            return;
        }

        if (obj.side == Sides.empire)
        {
            OnEnemyObjectClick(obj);
        }

        if (obj.side == Sides.federation) 
        { 
            OnFriendlyObjectClick(obj);
        }
    }

    public void OnEnemyObjectClick(Entity obj)
    {
        if (!_battleEngine.IsPossibleToAttackTarget(obj)) return;

        SetTarget(obj);
        Execute();
    }

    public void OnFriendlyObjectClick(Entity obj)
    {
        if (obj is Build) return;

        if (obj is Unit unit)
        {
            if (_turnData != null && _turnData._activeUnit != null)
            {
                if (_turnData._activeUnit.ChildId != unit.ChildId)
                {
                    SetActiveUnit(unit);
                    return;
                }
            }

            ClearActiveUnit();
        }
    }

    public void OnCellClick(Cell cell)
    {
        Debug.Log("Turn controller: CELL CLICK");

        if (_turnData == null || _turnData._route == null)
        {
            return;
        }
        if (_turnData._route.Contains(cell))
        {
            Execute();
        }
    }

    public void OnCellEnter(Cell cell)
    {
        if (_turnData == null || _turnData._activeUnit == null || _turnData._route == null)
        {
            Debug.Log("route or active unit null -> exit");

            return;
        }

        Debug.Log("route or active unit not NULL!");

        if (_turnData._route.Contains(cell))
        {
            // Если клетка присоединена к маршруту, обрезаем маршрут
            CutRouteToCell(cell);
        }
        else
        {
            if (_turnData._activeUnit.mobility > _turnData._route.Count - 1)
            {
                int maxExtendLenght = _turnData._activeUnit.mobility - (_turnData._route.Count - 1);
                ExtendRouteToCell(cell, maxExtendLenght);
            }
            else
            {
                // Автоматическое построение маршрута от активного юнита до выделенной клетки
                RebuildRouteToCell(cell);
            }
        }
    }

    public void OnCellExit(Cell cell)
    {

    }

    public void SendRouteChangeEvent()
    {
        if (_turnData._route == null)
        {
            EventMaster.current.OnChangeRoute(null, null);
            return;
        }

        List<Bector2Int> routeAsBector2Int = new List<Bector2Int>();
        foreach (var routeCell in _turnData._route) routeAsBector2Int.Add(new Bector2Int(routeCell.position));

        EventMaster.current.OnChangeRoute(routeAsBector2Int, new Bector2Int(_turnData._activeUnit.center));
    }

    private void CutRouteToCell(Cell cell)
    {
        int index = _turnData._route.IndexOf(cell);
        if (index >= 0)
        {
            _turnData._route = _turnData._route.Take(index + 1).ToList();
            SendRouteChangeEvent();
        }
    }

    private void RebuildRouteToCell(Cell cell)
    {
        Cell activeUnitCell = _map.Cells[_turnData._activeUnit.occypiedPoses.First()];
        _turnData._route = _map.BuildRoute(activeUnitCell, cell, _turnData._activeUnit.mobility);
        SendRouteChangeEvent();
    }

    public void ExtendRouteToCell(Cell cell, int maxExtendLenght)
    {
        List<Cell> routeContinuation = _map.BuildRoute(_turnData._route.Last(), cell, maxExtendLenght);
        _turnData._route.AddRange(routeContinuation);
        SendRouteChangeEvent();
    }

    public void ClearTurnData()
    {
        _turnData = new TurnData();
        EventMaster.current.OnActiveUnitChanged(_turnData._activeUnit);
        SendRouteChangeEvent();
    }

    public void SetActiveUnit(Unit unit)
    {
        _turnData._activeUnit = unit;
        _turnData._route = new List<Cell>();
        EnableBuildRouteListeners();
        EventMaster.current.OnActiveUnitChanged(_turnData._activeUnit);
    }

    public void ClearActiveUnit()
    {
        if (_turnData != null)
        {
            _turnData._activeUnit = null;
            ClearRoute();
            DisableBuildRouteListeners();
            EventMaster.current.OnActiveUnitChanged(_turnData._activeUnit);
        }
    }

    public void SetTarget(Entity target)
    {
        _turnData._target = target;
    }
    public void ClearTarget()
    {
        _turnData._target = null;
    }

    public void ClearRoute()
    {
        _turnData._route = null;
        SendRouteChangeEvent();
    }

    public void Execute()
    {
        EventMaster.current.OnExecuteTurn(_turnData);
        ClearTurnData();
    }
}

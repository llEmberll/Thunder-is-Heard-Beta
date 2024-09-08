using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TurnController : MonoBehaviour
{
    public TurnData _turnData;

    public Map _map;


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
        EventMaster.current.EnteredOnObject += OnObjectClick;
    }

    public void DisableObjectClickListener()
    {
        EventMaster.current.EnteredOnObject -= OnObjectClick;
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

    public void Start()
    {
        InitMap();

        EnableStartListeners();
        ClearTurnData();
    }

    public void OnStartFight()
    {
        DisableStartListeners();
        EnableNextTurnListener();
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
        if (obj.side == Sides.neutral)
        {
            return;
        }

        if (obj.side == Sides.empire)
        {
            OnEnemyObjectClick(obj);
        }

        if (obj.side != Sides.federation) 
        { 
            OnFriendlyObjectClick(obj);
        }
    }

    public void OnEnemyObjectClick(Entity obj)
    {
        //Если возможно атаковать - атаковать, иначе return
    }

    public void OnFriendlyObjectClick(Entity obj)
    {
        if (obj is Build) return;

        if (obj is Unit unit)
        {
            if (_turnData._activeUnit != null && unit.ChildId == _turnData._activeUnit.ChildId)
            {
                ClearActiveUnit();
            }

            else
            {
                SetActiveUnit(unit);
            }
        }
    }

    public void OnCellClick(Cell cell)
    {
        if (_turnData._route.Contains(cell))
        {
            Execute();
        }
    }

    public void OnCellEnter(Cell cell)
    {
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

    private void CutRouteToCell(Cell cell)
    {
        int index = _turnData._route.IndexOf(cell);
        if (index >= 0)
        {
            _turnData._route = _turnData._route.Take(index + 1).ToList();
        }
    }

    private void RebuildRouteToCell(Cell cell)
    {
        Cell activeUnitCell = _map.Cells[_turnData._activeUnit.occypiedPoses.First()];
        _turnData._route = _map.BuildRoute(activeUnitCell, cell, _turnData._activeUnit.mobility);
    }

    public void ExtendRouteToCell(Cell cell, int maxExtendLenght)
    {
        List<Cell> routeContinuation = _map.BuildRoute(_turnData._route.Last(), cell, maxExtendLenght);
        _turnData._route.AddRange(routeContinuation);
    }

    public void ClearTurnData()
    {
        _turnData = new TurnData();
    }

    public void SetActiveUnit(Unit unit)
    {
        _turnData._activeUnit = unit;
        EnableBuildRouteListeners();
    }
    public void ClearActiveUnit()
    {
        _turnData._activeUnit = null;
        DisableBuildRouteListeners();
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
    }

    public void Execute()
    {
        EventMaster.current.OnExecuteTurn(_turnData);
        ClearTurnData();
    }
}

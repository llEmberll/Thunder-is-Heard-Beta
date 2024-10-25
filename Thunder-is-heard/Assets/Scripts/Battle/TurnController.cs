using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class TurnController : MonoBehaviour
{
    public string[] _controlSides = new string[] { Sides.federation };
    public bool isAllowedToControl = false;
    public TurnData _turnData;

    public Map _map;
    

    public BattleEngine _battleEngine;
    public MapOnBattle _mapProcessor;

    public void EnableStartListeners()
    {
        EventMaster.current.FightIsStarted += OnStartFight;
    }

    public void DisableStartListeners()
    {
        EventMaster.current.FightIsStarted -= OnStartFight;
    }

    public void EnableObjectClickListeners()
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
        EventMaster.current.EnteredOnObject += OnObjectEnter;
    }

    public void DisableBuildRouteListeners()
    {
        EventMaster.current.ClickedOnCell -= OnCellClick;
        EventMaster.current.EnteredOnCell -= OnCellEnter;
        EventMaster.current.ExitedOnCell -= OnCellExit;
        EventMaster.current.EnteredOnObject -= OnObjectEnter;
    }


    public void InitMap()
    {
        _map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void InitMapProcessor()
    {
        _mapProcessor = _battleEngine.currentBattleSituation._map;
    }

    public void Start()
    {
        InitMap();
        InitBattleEngine();
        InitMapProcessor();

        EnableStartListeners();
        ClearTurnData();
    }

    public void OnStartFight()
    {
        DisableStartListeners();
        EnableObjectClickListeners();
        EnableBuildRouteListeners();
    }

    public void OnNextTurn(string side)
    {
        Debug.Log("TurnController: side changes to " + side);

        ClearTurnData();
        if (_controlSides.Contains(side))
        {
            Debug.Log("Allowed to control");

            isAllowedToControl = true;
        }
        else
        {
            Debug.Log("Not allowed to control");
        }
    }

    public void OnObjectEnter(Entity obj)
    {
        if (isAllowedToControl == false) return;
        if (obj is Unit unit)
        {
            Debug.Log("TurnController: On unit enter");

            if (_turnData._activeUnit != null && _turnData._activeUnit.ChildId == unit.ChildId)
            {
                ClearRoute();
            }
        }
    }

    public void OnObjectClick(Entity obj)
    {
        if (isAllowedToControl == false) return;
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

        ClearRoute();
        SetTarget(obj);
        Execute();
    }

    public void OnFriendlyObjectClick(Entity obj)
    {
        if (obj is Build) return;

        if (obj is Unit unit)
        {
            OnFriendlyUnitClick(unit);
        }
    }

    public void OnFriendlyUnitClick(Unit unit)
    {
        if (_turnData._activeUnit == null || _turnData._activeUnit.ChildId != unit.ChildId)
        {
            SetActiveUnit(unit);
            return;
        }

        ClearActiveUnit();
    }

    public void OnCellClick(Cell cell)
    {
        if (isAllowedToControl == false) return;
        Debug.Log("Turn controller: CELL CLICK");

        if (!cell.visible) return;

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
        if (isAllowedToControl == false) return;
        if (!cell.visible) return;
        if (_turnData._activeUnit == null)
        {
            return;
        }

        if (_turnData._route.Contains(cell))
        {
            if (_turnData._route.Last() == cell) return;

            // Если клетка присоединена к маршруту, обрезаем маршрут
            CutRouteToCell(cell);
        }
        else
        {
            if (_turnData._activeUnit.mobility > _turnData._route.Count)
            {
                int maxExtendLenght = _turnData._activeUnit.mobility - _turnData._route.Count;
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
        if (isAllowedToControl == false) return;
    }

    public void SendRouteChangeEvent()
    {
        if (_turnData._route == null || _turnData._route.Count < 1)
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
        Bector2Int startPosition = new Bector2Int(_turnData._activeUnit.center);
        Bector2Int endPosition = new Bector2Int(cell.position);
        List<Bector2Int> routeAsBector2List = _mapProcessor.BuildRoute(startPosition, endPosition, _turnData._activeUnit.mobility);
        _turnData._route = _battleEngine.GetCellsByBector2IntPositions(routeAsBector2List);
        SendRouteChangeEvent();
    }

    public void ExtendRouteToCell(Cell cell, int maxExtendLength)
    {
        Bector2Int start = _turnData._route.Count > 0 ? new Bector2Int(_turnData._route.Last().position) : new Bector2Int(_turnData._activeUnit.center);
        Bector2Int end = new Bector2Int(cell.position);
        List<Bector2Int> routeAsBector2List = _mapProcessor.BuildRoute(start, end, maxExtendLength);
        _turnData._route.AddRange(_battleEngine.GetCellsByBector2IntPositions(routeAsBector2List));
        
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
        _turnData._activeUnit = null;
        ClearRoute();
        DisableBuildRouteListeners();
        EventMaster.current.OnActiveUnitChanged(_turnData._activeUnit);
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
        _turnData._route = new List<Cell>();
        SendRouteChangeEvent();
    }

    public void Pass()
    {
        ClearTurnData();
        Execute();
    }

    public void Execute()
    {
        Debug.Log("TurnController: Execute");

        isAllowedToControl = false;
        EventMaster.current.OnExecuteTurn(_turnData);
        ClearTurnData();
    }
}

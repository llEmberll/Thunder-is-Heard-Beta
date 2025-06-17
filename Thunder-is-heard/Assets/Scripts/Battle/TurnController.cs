using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class TurnController : MonoBehaviour
{
    public string[] _controlSides = new string[] { Sides.federation };
    public bool isAllowedToControl = false;

    public Unit _activeUnit = null;
    public Entity _target = null;
    public TurnData _turnData;

    public Map _map;
    

    public BattleEngine _battleEngine;
    public MapOnBattle _mapProcessor;

    public void EnableStartListeners()
    {
        EventMaster.current.FightIsStarted += OnStartFight;
        EventMaster.current.FightIsContinued += OnStartFight;

        EventMaster.current.PassedTurn += Pass;
    }

    public void DisableStartListeners()
    {
        EventMaster.current.FightIsStarted -= OnStartFight;
        EventMaster.current.FightIsContinued -= OnStartFight;

        EventMaster.current.PassedTurn -= Pass;
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
            if (_turnData._activeUnitIdOnBattle != null && _turnData._activeUnitIdOnBattle == unit.ChildId)
            {
                ClearRoute();
            }
        }
    }

    public void OnObjectClick(Entity obj)
    {
        if (isAllowedToControl == false) return;
        if (obj is Obstacle) return; // Нельзя взаимодействовать с препятствиями в бою

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
        if (CanMoveWithAttack(obj))
        {
            SetTarget(obj);
            Execute();
        }
        else
        {
            if (!BattleEngine.IsPossibleToAttackTarget(_battleEngine.currentBattleSituation, obj)) return;
            ClearRoute();
            ClearActiveUnit();
            SetTarget(obj);
            Execute();
        }
    }

    public bool CanMoveWithAttack(Entity obj)
    {
        if (_turnData._activeUnitIdOnBattle == null) return false;
        UnitOnBattle activeUnitOnBattle = _battleEngine.currentBattleSituation.GetUnitById(_turnData._activeUnitIdOnBattle);
        if (!_battleEngine.currentBattleSituation.CanMoveWithAttack(activeUnitOnBattle)) return false;
        if (_turnData._route == null || _turnData._route.Count < 1) return false;

        ObjectOnBattle targetObjectOnBattle = _battleEngine.currentBattleSituation.GetObjectById(obj.ChildId);
        return _battleEngine.currentBattleSituation.CanObjectAttackWithoutMovingObjectFromNewPosition(activeUnitOnBattle, targetObjectOnBattle, _turnData._route.Last());
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
        if (_turnData._activeUnitIdOnBattle == null || _turnData._activeUnitIdOnBattle != unit.ChildId)
        {
            SetActiveUnit(unit);
            return;
        }

        ClearActiveUnit();
    }

    public void OnCellClick(Cell cell)
    {
        if (isAllowedToControl == false) return;
        if (!cell.visible) return;

        if (_turnData == null || _turnData._route == null)
        {
            return;
        }
        if (_turnData._route.Contains(new Bector2Int(cell.position)))
        {
            Execute();
        }
    }

    public void OnCellEnter(Cell cell)
    {
        if (isAllowedToControl == false) return;
        if (!cell.visible) return;
        if (_turnData._activeUnitIdOnBattle == null)
        {
            return;
        }

        Bector2Int cellPosition = new Bector2Int(cell.position);

        if (_turnData._route.Contains(cellPosition))
        {
            if (_turnData._route.Last() == cellPosition) return;

            // Если клетка присоединена к маршруту, обрезаем маршрут
            CutRouteToCell(cell);
        }
        else
        {
            if (_activeUnit.mobility > _turnData._route.Count)
            {
                int maxExtendLenght = _activeUnit.mobility - _turnData._route.Count;
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
        foreach (var routeCell in _turnData._route) routeAsBector2Int.Add(routeCell);

        EventMaster.current.OnChangeRoute(routeAsBector2Int, new Bector2Int(_activeUnit.center));
    }

    private void CutRouteToCell(Cell cell)
    {
        int index = _turnData._route.IndexOf(new Bector2Int(cell.position));
        if (index >= 0)
        {
            _turnData._route = _turnData._route.Take(index + 1).ToList();
            SendRouteChangeEvent();
        }
    }

    private void RebuildRouteToCell(Cell cell)
    {
        Bector2Int startPosition = new Bector2Int(_activeUnit.center);
        Bector2Int endPosition = new Bector2Int(cell.position);
        _turnData._route = _mapProcessor.BuildRoute(startPosition, endPosition, _activeUnit.mobility);
        SendRouteChangeEvent();
    }

    public void ExtendRouteToCell(Cell cell, int maxExtendLength)
    {
        Bector2Int start = _turnData._route.Count > 0 ? _turnData._route.Last() : new Bector2Int(_activeUnit.center);
        Bector2Int end = new Bector2Int(cell.position);
        _turnData._route.AddRange(_mapProcessor.BuildRoute(start, end, maxExtendLength));
        SendRouteChangeEvent();
    }


    public void ClearTurnData()
    {
        _activeUnit = null;
        _target = null;
        _turnData = new TurnData();
        EventMaster.current.OnActiveUnitChanged(null);
        SendRouteChangeEvent();
    }

    public void SetActiveUnit(Unit unit)
    {
        _activeUnit = unit;
        _turnData._activeUnitIdOnBattle = unit.ChildId;
        _turnData._route = new List<Bector2Int>();
        EnableBuildRouteListeners();
        EventMaster.current.OnActiveUnitChanged(unit);
    }

    public void ClearActiveUnit()
    {
        _activeUnit = null;
        _turnData._activeUnitIdOnBattle = null;
        ClearRoute();
        DisableBuildRouteListeners();
        EventMaster.current.OnActiveUnitChanged(null);
    }

    public void SetTarget(Entity target)
    {
        _target = target;
        _turnData._targetIdOnBattle = target.ChildId;
    }
    public void ClearTarget()
    {
        _target = null;
        _turnData._targetIdOnBattle = null;
    }

    public void ClearRoute()
    {
        _turnData._route = new List<Bector2Int>();
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

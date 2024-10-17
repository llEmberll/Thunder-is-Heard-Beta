using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Transform routeParent;

    public GameObject _overRouteImage;
    public GameObject _routeImage;

    public Dictionary<Bector2Int, List<GameObject>> route = null;
    public List<Cell> reachableCells = null;

    public BattleEngine _battleEngine;

    public void Start()
    {
        InitPrefabs();
        InitBattleEngine();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void EnableListeners()
    {
        EventMaster.current.RouteChanged += OnRouteChanged;
        EventMaster.current.ActiveUnitChanged += OnActiveUnitChanged;
    }

    public void DisableListeners()
    {
        EventMaster.current.RouteChanged -= OnRouteChanged;
        EventMaster.current.ActiveUnitChanged -= OnActiveUnitChanged;
    }

    public void InitPrefabs()
    {
        _routeImage = Resources.Load<GameObject>(Config.resources["routeImage"]);
        _overRouteImage = Resources.Load<GameObject>(Config.resources["overRouteImage"]);
    }



    public void OnRouteChanged(List<Bector2Int> newPositionsForRoute, Bector2Int unitPosition)
    {
        ClearRoute();
        if (newPositionsForRoute != null && newPositionsForRoute.Count > 0 && unitPosition != null)
        {
            CreateRoute(newPositionsForRoute, unitPosition);
        }
    }

    public void ClearRoute()
    {
        if (route != null)
        {
            foreach (var listOfRouteObjectsByPosition in route.Values)
            {
                foreach (var routeObj in listOfRouteObjectsByPosition)
                {
                    Destroy(routeObj);
                }
            }
        }

        route = null;
    }


    public void CreateRoute(List<Bector2Int> newPositionsForRoute, Bector2Int unitPosition)
    {
        route = new Dictionary<Bector2Int, List<GameObject>>();

        Bector2Int finalPosition = newPositionsForRoute.Last();
        newPositionsForRoute.Remove(finalPosition);
        Bector2Int previousHandledPosition = unitPosition;
        foreach (var positionForRoute in newPositionsForRoute)
        {
            List<GameObject> currentRouteObjs = CreateRouteObjsForPosition(positionForRoute, previousHandledPosition, false);
            route.Add(positionForRoute, currentRouteObjs);
            previousHandledPosition = positionForRoute;
        }

        List<GameObject> routeFinalPositionObjs = CreateRouteObjsForPosition(finalPosition, previousHandledPosition, true);
        route.Add(finalPosition, routeFinalPositionObjs);
    }

    public List<GameObject> CreateRouteObjsForPosition(Bector2Int position, Bector2Int previousRoutePosition, bool isOver)
    {
        Vector3 positionForMainRouteObj = new Vector3(position._x, _routeImage.transform.position.y, position._x);
        GameObject routeObjPrefab = isOver == true ? _overRouteImage : _routeImage;
        var routeMainObj = Instantiate(routeObjPrefab, positionForMainRouteObj, Quaternion.identity, parent: routeParent);

        Vector2Int segmentOffset = (previousRoutePosition.ToVector2Int() - position.ToVector2Int()) / 3;
        Vector3 offsetForSegmentRouteObjs = new Vector3(segmentOffset.x, positionForMainRouteObj.y, segmentOffset.x);
        var routeSecondSegmentObj = Instantiate(_routeImage, positionForMainRouteObj + offsetForSegmentRouteObjs, Quaternion.identity, parent: routeParent);
        var routeFirstSegmentObj = Instantiate(_routeImage, positionForMainRouteObj + (offsetForSegmentRouteObjs * 2), Quaternion.identity, parent: routeParent);

        return new List<GameObject>
        {
            routeFirstSegmentObj,
            routeSecondSegmentObj,
            routeMainObj
        };
    }

    public void OnActiveUnitChanged(Unit activeUnit)
    {
        if (activeUnit == null)
        {
            ClearReachableCellsAndHide();
        }
        else
        {
            UpdateReachableCellsByUnitAndDisplay(activeUnit);
        }
    }

    public void ClearReachableCellsAndHide()
    {
        foreach (var cell in reachableCells)
        {
            cell.RenderSwitch(false);
        }

        reachableCells = null;
    }

    public void UpdateReachableCellsByUnitAndDisplay(Unit unit)
    {
        reachableCells = _battleEngine.GetReachableCellsByUnit(unit);
        foreach (var cell in reachableCells)
        {
            cell.RenderSwitch(true);
        }
    }
}

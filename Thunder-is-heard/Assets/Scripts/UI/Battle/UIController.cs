using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
        EnableListeners();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObjectUtils.FindComponentByTagIncludingInactive<BattleEngine>(Tags.battleEngine);
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

    public void AddToRoute(Bector2Int position, List<GameObject> objs)
    {
        if (route.ContainsKey(position))
        {
            List<GameObject> newValue = route[position];
            newValue.AddRange(objs);
            route[position] = newValue;
        }
        else
        {
            route.Add(position, objs);
        }
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
            AddToRoute(positionForRoute, currentRouteObjs);
            previousHandledPosition = positionForRoute;
        }

        List<GameObject> routeFinalPositionObjs = CreateRouteObjsForPosition(finalPosition, previousHandledPosition, true);
        AddToRoute(finalPosition, routeFinalPositionObjs);
    }

    public List<GameObject> CreateRouteObjsForPosition(Bector2Int position, Bector2Int previousRoutePosition, bool isOver)
    {
        Vector3 positionForMainRouteObj = new Vector3(position._x, _routeImage.transform.position.y, position._y);
        GameObject routeObjPrefab = isOver == true ? _overRouteImage : _routeImage;
        var routeMainObj = Instantiate(routeObjPrefab, positionForMainRouteObj, Quaternion.Euler(new Vector3(90f, 0f, 0f)), parent: routeParent);

        float segmentOffsetByX = (float)(previousRoutePosition._x - position._x) / 3;
        float segmentOffsetByZ = (float)(previousRoutePosition._y - position._y) / 3;
        Vector3 offsetForSegmentRouteObjs = new Vector3(segmentOffsetByX, 0, segmentOffsetByZ);
        var routeSecondSegmentObj = Instantiate(_routeImage, positionForMainRouteObj + offsetForSegmentRouteObjs, Quaternion.Euler(new Vector3(90f, 0f, 0f)), parent: routeParent);
        var routeFirstSegmentObj = Instantiate(_routeImage, positionForMainRouteObj + (offsetForSegmentRouteObjs * 2), Quaternion.Euler(new Vector3(90f, 0f, 0f)), parent: routeParent);

        return new List<GameObject>
        {
            routeFirstSegmentObj,
            routeSecondSegmentObj,
            routeMainObj
        };
    }

    public void OnActiveUnitChanged(Unit activeUnit)
    {
        ClearReachableCellsAndHide();
        if (activeUnit != null)
        {
            UpdateReachableCellsByUnitAndDisplay(activeUnit);
        }
    }

    public void ClearReachableCellsAndHide()
    {
        if (reachableCells == null) return;

        foreach (var cell in reachableCells)
        {
            cell.RenderSwitch(false);
        }

        reachableCells = null;
    }

    public void UpdateReachableCellsByUnitAndDisplay(Unit unit)
    {
        reachableCells = _battleEngine.GetReachableCellsByUnit(_battleEngine.currentBattleSituation, unit);
        foreach (var cell in reachableCells)
        {
            cell.RenderSwitch(true);
        }
    }
}

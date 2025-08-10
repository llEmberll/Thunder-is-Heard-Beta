using System.Linq;
using UnityEngine;


public class ReachDistanceBetweenUnitsAndObjectCondition : BasicCondition
{
    public int _minDistance;
    public string _targetObjectId;
    public string _unitsSide;
    public BattleEngine _battleEngine;

    public bool _isReached = false;
    private bool _initialCheckDone = false;

    public ReachDistanceBetweenUnitsAndObjectCondition(int minDistance, string targetObjectId, string unitsSide) 
    {
        _minDistance = minDistance;
        _targetObjectId = targetObjectId;
        _unitsSide = unitsSide;
        
        InitBattleEngine();
        // Убираем подписку на события из конструктора - теперь это будет в OnActivate
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void EnableListeners()
    {
        EventMaster.current.UnitMoveStarted += OnSomeUnitMove;
    }

    public void DisableListeners()
    {
        EventMaster.current.UnitMoveStarted -= OnSomeUnitMove;
    }

    public bool InitialCheck()
    {
        // Логируем _battleEngine
        Debug.Log($"[ReachDistanceCondition] _battleEngine: {(_battleEngine == null ? "NULL" : "NOT NULL")}");
        
        if (_battleEngine == null)
        {
            Debug.LogError("[ReachDistanceCondition] _battleEngine is null!");
            return false;
        }
        
        // Логируем currentBattleSituation
        Debug.Log($"[ReachDistanceCondition] currentBattleSituation: {(_battleEngine.currentBattleSituation == null ? "NULL" : "NOT NULL")}");
        
        if (_battleEngine.currentBattleSituation == null)
        {
            Debug.LogWarning("[ReachDistanceCondition] currentBattleSituation is null! Returning false.");
            return false;
        }
        
        // Логируем _targetObjectId
        Debug.Log($"[ReachDistanceCondition] _targetObjectId: '{_targetObjectId}'");
        
        if (string.IsNullOrEmpty(_targetObjectId))
        {
            Debug.LogError("[ReachDistanceCondition] _targetObjectId is null or empty!");
            return false;
        }
        
        ObjectOnBattle target = _battleEngine.currentBattleSituation.GetObjectById(_targetObjectId);
        
        // Логируем результат GetObjectById
        Debug.Log($"[ReachDistanceCondition] target from GetObjectById: {(target == null ? "NULL" : "NOT NULL")}");
        
        if (target == null)
        {
            Debug.LogError($"[ReachDistanceCondition] GetObjectById returned null for id: '{_targetObjectId}'");
            return false;
        }
        
        // Логируем target.Position
        Debug.Log($"[ReachDistanceCondition] target.Position: {(target.Position == null ? "NULL" : "NOT NULL")}");
        
        if (target.Position == null)
        {
            Debug.LogError("[ReachDistanceCondition] target.Position is null!");
            return false;
        }
        
        RectangleBector2Int rectangleTargetPosition = new RectangleBector2Int(target.Position);

        ObjectOnBattle nearestUnitBySide = _battleEngine.currentBattleSituation.FindNearestUnitByRectangleAndSide(rectangleTargetPosition, _unitsSide);
        if (nearestUnitBySide == null) return false;

        int nearestDistance = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(nearestUnitBySide.Position.First(), rectangleTargetPosition);
        if (nearestDistance <= _minDistance) return true;
        else return false;

    }

    public void OnSomeUnitMove(Unit unit)
    {
        Debug.Log($"[ReachDistanceCondition] OnSomeUnitMove вызван для юнита: side={unit.side}, id={unit.ChildId}");
        Debug.Log($"[ReachDistanceCondition] Текущая сторона юнитов: {_unitsSide}");
        
        if (unit.side != _unitsSide)
        {
            Debug.Log($"[ReachDistanceCondition] Сторона юнита ({unit.side}) не совпадает с требуемой ({_unitsSide}). Выход из метода.");
            return;
        }
        
        Debug.Log($"[ReachDistanceCondition] Сторона юнита совпадает. Продолжаем выполнение.");
        
        Bector2Int currentUnitPosition = new Bector2Int(unit.occypiedPoses.First());
        Debug.Log($"[ReachDistanceCondition] Позиция юнита: x={currentUnitPosition._x}, y={currentUnitPosition._y}");
        Debug.Log($"[ReachDistanceCondition] Ищем целевой объект по ID: {_targetObjectId}");
        
        ObjectOnBattle target = _battleEngine.currentBattleSituation.GetObjectById(_targetObjectId);
        Debug.Log($"[ReachDistanceCondition] Результат поиска целевого объекта: {(target == null ? "NULL" : $"NOT NULL, ID: {target.idOnBattle}")}");
        
        if (target == null)
        {
            Debug.LogError($"[ReachDistanceCondition] Целевой объект не найден! ID: {_targetObjectId}");
            return;
        }
        
        Debug.Log($"[ReachDistanceCondition] Целевой объект найден. Создаем RectangleBector2Int из позиции: {target.Position}");
        Debug.Log($"[ReachDistanceCondition] Первая позиция цели: x={target.Position.First()._x}, y={target.Position.First()._y}");
        RectangleBector2Int rectangleTargetPosition = new RectangleBector2Int(target.Position);
        Bector2Int[] positionsInRectange = rectangleTargetPosition.GetPositions();
        Debug.Log($"[ReachDistanceCondition] RectangleBector2Int создан. Позиции: {string.Join(", ", positionsInRectange.Select(p => $"({p._x}, {p._y})"))}");
        
        Debug.Log($"[ReachDistanceCondition] Вычисляем расстояние между юнитом и целью...");
        int currentDistanceToTarget = BattleEngine.GetDistanceBetweenPointAndRectangleOfPoints(currentUnitPosition, rectangleTargetPosition);
        Debug.Log($"[ReachDistanceCondition] Расстояние до цели: {currentDistanceToTarget}, минимальное требуемое расстояние: {_minDistance}");
        
        if (currentDistanceToTarget <= _minDistance)
        {
            Debug.Log($"[ReachDistanceCondition] Расстояние ({currentDistanceToTarget}) <= минимального ({_minDistance}). Условие выполнено!");
            _isReached = true;
            Debug.Log($"[ReachDistanceCondition] Устанавливаем _isReached = true");
            Debug.Log($"[ReachDistanceCondition] Отключаем слушатели событий");
            DisableListeners();
        }
        else
        {
            Debug.Log($"[ReachDistanceCondition] Расстояние ({currentDistanceToTarget}) > минимального ({_minDistance}). Условие не выполнено.");
        }
        
        Debug.Log($"[ReachDistanceCondition] OnSomeUnitMove завершен. _isReached = {_isReached}");
    }

    protected override void OnActivate()
    {
        // При активации проверяем текущее состояние
        if (!_initialCheckDone)
        {
            if (InitialCheck())
            {
                _isReached = true;
            }
            else
            {
                // Если условие не выполнено, подписываемся на события
                EnableListeners();
            }
            _initialCheckDone = true;
        }
        else if (!_isReached)
        {
            // Если уже проверяли и условие не выполнено, подписываемся на события
            EnableListeners();
        }
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        _isReached = false;
        _initialCheckDone = false;
        DisableListeners();
    }

    public override bool IsComply()
    {
        return _isReached;
    }

    public override bool IsRealTimeUpdate()
    {
        return false;
    }
}

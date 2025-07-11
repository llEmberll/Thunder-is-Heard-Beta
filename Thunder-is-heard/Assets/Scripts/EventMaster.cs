using System;
using System.Collections.Generic;
using UnityEngine;


public class EventMaster: MonoBehaviour
{
    public static EventMaster current;

    private void Awake()
    {
        current = this;
    }
    
    public event Action<string> SceneChanged;
    public void ChangeScene(string newScene)
    {
        SceneChanged?.Invoke(newScene);
    }

    public event Action ToggledToBaseMode;
    public void OnBaseMode()
    {
        ToggledToBaseMode?.Invoke();
    }

    public event Action ToggledToBuildMode;
    public void OnBuildMode()
    {
        ToggledToBuildMode?.Invoke();
    }
    
    public event Action ToggledOffBuildMode;
    public void OnExitBuildMode()
    {
        ToggledOffBuildMode?.Invoke();
    }


    public event Action PreviewRotated;
    public void OnRotatePreview()
    {
        PreviewRotated?.Invoke();
    }


    public event Action<State> StateChanged;
    public void OnChangeState(State newState)
    {
        StateChanged?.Invoke(newState);
    }

    public event Action<LandingData> StartLanding;
    public void Landing(LandingData landingData)
    {
        StartLanding?.Invoke(landingData);
    }

    public event Action ToBattleButtonPressed;
    public void OnPressToBattleButton()
    {
        ToBattleButtonPressed?.Invoke();
    }

    public event Action FightIsStarted;
    public void StartFight()
    {
        FightIsStarted?.Invoke();
    }

    public event Action FightIsContinued;
    public void ContinueFight()
    {
        FightIsContinued?.Invoke();
    }

    public event Action PassedTurn;
    public void OnPassTurn()
    {
        PassedTurn?.Invoke();
    }

    public event Action<string> NextTurn;
    public void OnNextTurn(string side)
    {
        NextTurn?.Invoke(side);
    }

    public event Action<IStage> NextStage;
    public void OnNextStage(IStage stage)
    {
        NextStage?.Invoke(stage);
    }

    public event Action<IStage> BeginStage;
    public void OnStageBegin(IStage stage)
    {
        BeginStage?.Invoke(stage);
    }

    public event Action<TurnData> TurnExecuted;
    public void OnExecuteTurn(TurnData turnData)
    {
        TurnExecuted?.Invoke(turnData);
    }

    public event Action FightLost;
    public void LoseFigth()
    {
        FightLost?.Invoke();
    }

    public event Action FightWon;
    public void WinFight()
    {
        FightWon?.Invoke();
    }

    public event Action<Entity> ObjectExposed;
    public void ExposeObject(Entity obj)
    {
        ObjectExposed?.Invoke(obj);
    }


    public event Action<ObjectPreview> PreviewCreated;
    public void OnCreatePreview(ObjectPreview preview)
    {
        PreviewCreated?.Invoke(preview);
    }

    public event Action PreviewDeleted;
    public void OnDeletePreview()
    {
        PreviewDeleted?.Invoke();
    }

    public event Action<InventoryCacheItem> InventoryItemAdded;
    public void OnAddInventoryItem(InventoryCacheItem item)
    {
        InventoryItemAdded?.Invoke(item);
    }

    public event Action InventoryChanged;
    public void OnChangeInventory()
    {
        InventoryChanged?.Invoke();
    }

    public event Action BaseObjectsChanged;
    public void OnChangeBaseObjects()
    {
        BaseObjectsChanged?.Invoke();
    }

    public event Action BattleObjectsChanged;
    public void OnChangeBattleObjects()
    {
        BattleObjectsChanged?.Invoke();
    }

    public event Action ShopChanged;
    public void OnChangeShop()
    {
        ShopChanged?.Invoke();
    }

    public event Action<ResourcesData> ResourcesChanged;
    public void OnChangeResources(ResourcesData newResources)
    {
        ResourcesChanged?.Invoke(newResources);
    }

    public event Action<string> ChangedBaseName;
    public void OnChangeBaseName(string value)
    {
        ChangedBaseName?.Invoke(value);
    }

    public event Action<string> InitializedBaseName;
    public void OnInitializeBaseName(string value)
    {
        InitializedBaseName?.Invoke(value);
    }

    public event Action<Entity> BaseObjectRemoved;
    public void OnRemoveBaseObject(Entity obj)
    {
        BaseObjectRemoved?.Invoke(obj);
    }

    public event Action<Entity> BattleObjectRemoved;
    public void OnRemoveBattleObject(Entity obj)
    {
        BattleObjectRemoved?.Invoke(obj);
    }

    public event Action<Entity> BaseObjectReplaced;
    public void OnReplaceBaseObject(Entity obj)
    {
        BaseObjectReplaced?.Invoke(obj);
    }

    public event Action<Entity> BattleObjectReplaced;
    public void OnReplaceBattleObject(Entity obj)
    {
        BattleObjectReplaced?.Invoke(obj);
    }

    public event Action<Entity> EnteredOnObject;
    public void OnObjectEnter(Entity obj)
    {
        EnteredOnObject?.Invoke(obj);
    }

    public event Action<Entity> ExitedOnObject;
    public void OnObjectExit(Entity obj)
    {
        ExitedOnObject?.Invoke(obj);
    }

    public event Action<Entity> ClickedOnObject;
    public void OnObjectClick(Entity obj)
    {
        ClickedOnObject?.Invoke(obj);
    }

    public event Action<Cell> EnteredOnCell;
    public void OnCellEnter(Cell cell)
    {
        EnteredOnCell?.Invoke(cell);
    }

    public event Action<Cell> ExitedOnCell;
    public void OnCellExit(Cell cell)
    {
        ExitedOnCell?.Invoke(cell);
    }

    public event Action<Cell> ClickedOnCell;
    public void OnCellClick(Cell cell)
    {
        ClickedOnCell?.Invoke(cell);
    }

    public event Action<ProcessOnBaseCacheItem> ProcessOnBaseFinished;
    public void OnProcessOnBaseFinish(ProcessOnBaseCacheItem process)
    {
        ProcessOnBaseFinished?.Invoke(process);
    }

    public event Action<ProcessOnBaseCacheItem> ProcessOnBaseStarted;
    public void OnProcessOnBaseStart(ProcessOnBaseCacheItem process)
    {
        ProcessOnBaseStarted?.Invoke(process);
    }

    public event Action<string> ProcessOnBaseHandled;
    public void OnProcessOnBaseHandle(string processId)
    {
        ProcessOnBaseHandled?.Invoke(processId);
    }

    public event Action<string, string> ObjectOnBaseWorkStatusChanged;
    public void OnChangeObjectOnBaseWorkStatus(string objectOnBaseId, string newStatus)
    {
        ObjectOnBaseWorkStatusChanged?.Invoke(objectOnBaseId, newStatus);
    }

    public event Action<ProductsNotificationCacheItem> ProductsNotificationDeleted;
    public void OnDeleteProductsNotification(ProductsNotificationCacheItem deletedProductsNotification)
    {
        ProductsNotificationDeleted?.Invoke(deletedProductsNotification);
    }

    public event Action<ProductsNotificationCacheItem> ProductsNotificationCreated;
    public void OnCreateProductsNotification(ProductsNotificationCacheItem createdProductsNotification)
    {
        ProductsNotificationCreated?.Invoke(createdProductsNotification);
    }

    public event Action<ProductsNotificationCacheItem> ProductsCollected;
    public void OnCollectProducts(ProductsNotificationCacheItem collected)
    {
        ProductsCollected?.Invoke(collected);
    }

    public event Action<ProductsNotificationCacheItem> UnitCollected;
    public void OnCollectUnit(ProductsNotificationCacheItem collected)
    {
        UnitCollected?.Invoke(collected);
    }

    public event Action<Obstacle> ObstacleDemolitionInitiated;
    public void OnInitiateObstacleDemolition(Obstacle obstacle)
    {
        ObstacleDemolitionInitiated?.Invoke(obstacle);
    }

    public event Action<bool> UIPanelToggled;
    public void OnUIPanelToggle(bool isNowActive)
    {
        UIPanelToggled?.Invoke(isNowActive);
    }

    public event Action<Vector2Int, bool> CameraNeedFocusOnPosition;
    public void FocusCameraOnPosition(Vector2Int position, bool lockCamera)
    {
        CameraNeedFocusOnPosition?.Invoke(position, lockCamera);
    }

    public event Action CameraFocusCanceled;
    public void CancelFocus()
    {
        CameraFocusCanceled?.Invoke();
    }

    public event Action<bool> CameraMovePermitToggled;
    public void ToggleCameraMovePermit(bool lockCamera)
    {
        CameraMovePermitToggled?.Invoke(lockCamera);
    }

    public event Action<LandableUnit> LandableUnitFocused;
    public void OnLandableUnitFocus(LandableUnit unit)
    {
        LandableUnitFocused?.Invoke(unit);
    }

    public event Action<LandableUnit> LandableUnitDefocused;
    public void OnLandableUnitDefocus(LandableUnit unit)
    {
        LandableUnitDefocused?.Invoke(unit);
    }

    public event Action<Unit> UnitMoveStarted;
    public void OnStartUnitMove(Unit unit)
    {
        UnitMoveStarted?.Invoke(unit);
    }

    public event Action<Unit> UnitMoveFinished;
    public void OnFinishUnitMove(Unit unit)
    {
        UnitMoveFinished?.Invoke(unit);
    }

    public event Action<List<Bector2Int>, Bector2Int> RouteChanged;
    public void OnChangeRoute(List<Bector2Int> route, Bector2Int unitPosition)
    {
        RouteChanged?.Invoke(route, unitPosition);
    }

    public event Action<Unit> ActiveUnitChanged;
    public void OnActiveUnitChanged(Unit unit)
    {
        ActiveUnitChanged?.Invoke(unit);
    }


    public event Action<IStage> CurrentStageChanged;
    public void OnCurrentStageChange(IStage currentStage)
    {
        CurrentStageChanged?.Invoke(currentStage);
    }

    public event Action<Entity> DestroyedObject;
    public void OnObjectDestroy(Entity obj)
    {
        DestroyedObject?.Invoke(obj);
    }

    public event Action<Entity> DamagedObject;
    public void OnObjectDamaged(Entity obj)
    {
        DamagedObject?.Invoke(obj);
    }

    public event Action<Replic[]> BegunDialogue;
    public void BeginDialogue(Replic[] replics)
    {
        BegunDialogue?.Invoke(replics);
    }

    public event Action DialogueEnd;
    public void OnEndDialogue()
    {
        DialogueEnd?.Invoke();
    }

    public event Action ReplicPassed;
    public void OnPassReplic()
    {
        ReplicPassed?.Invoke();
    }

    public event Action<MediaEventData> BegunMediaEvent;
    public void BeginMediaEvent(MediaEventData eventData)
    {
        BegunMediaEvent?.Invoke(eventData);
    }

    public event Action MediaEventEnd;
    public void OnEndMediaEvent()
    {
        MediaEventEnd?.Invoke();
    }

    public event Action StageUpdated;
    public void OnUpdateStage()
    {
        StageUpdated?.Invoke();
    }

    public event Action ScenarioUpdated;
    public void OnUpdateScenario()
    {
        ScenarioUpdated?.Invoke();
    }

    public event Action<FocusData> ObjectFocused;
    public void OnObjectFocused(FocusData focusData)
    {
        ObjectFocused?.Invoke(focusData);
    }

    public event Action ClearObjectFocused;
    public void OnClearObjectFocus()
    {
        ClearObjectFocused?.Invoke();
    }

    public event Action<string, string> ComponentBehaviourChanged;
    public void OnChangeComponentBehaviour(string componentName, string behaviourName)
    {
        ComponentBehaviourChanged?.Invoke(componentName, behaviourName);
    }

    public event Action ComponentsBehaviourReset;
    public void OnResetComponentsBehaviour()
    {
        ComponentsBehaviourReset?.Invoke();
    }

    public event Action<string> HintSetup;
    public void OnSetHint(string text)
    {
        HintSetup?.Invoke(text);
    }

    public event Action HintHidden;
    public void OnHideHint()
    {
        HintHidden?.Invoke();
    }

    public event Action CameraMoved;
    public void OnCameraMoved()
    {
        CameraMoved?.Invoke();
    }
}
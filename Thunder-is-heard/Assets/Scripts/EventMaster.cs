using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
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
    
    public event Action FightIsStarted;
    public void StartFight()
    {
        FightIsStarted?.Invoke();
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

    public event Action<string, string, Bector2Int[], int> ObjectExposed;
    public void ExposeObject(string objId, string objType, Bector2Int[] occypaton, int rotation)
    {
        ObjectExposed?.Invoke(objId, objType, occypaton, rotation);
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

    public event Action<string, string> BaseObjectRemoved;
    public void OnRemoveBaseObject(string id, string type)
    {
        BaseObjectRemoved?.Invoke(id, type);
    }

    public event Action<Entity> BaseObjectReplaced;
    public void OnReplaceBaseObject(Entity obj)
    {
        BaseObjectReplaced?.Invoke(obj);
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

    public event Action<bool> UIListToggled;
    public void OnUIListToggle(bool isNowActive)
    {
        UIListToggled?.Invoke(isNowActive);
    }
}
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
}
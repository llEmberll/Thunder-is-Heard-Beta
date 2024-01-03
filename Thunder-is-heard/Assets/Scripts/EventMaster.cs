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

    public event Action<int, string> ObjectExposed;
    public void ExposeObject(int objId, string objType)
    {
        ObjectExposed?.Invoke(objId, objType);
    }

}
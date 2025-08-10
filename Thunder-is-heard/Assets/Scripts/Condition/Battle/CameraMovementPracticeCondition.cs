using UnityEngine;


public class CameraMovementPracticeCondition : BasicCondition
{
    public float _practiceDuration;
    private float _initialPracticeDuration;

    public CameraMovementPracticeCondition(float practiceDuration) 
    { 
        _practiceDuration = practiceDuration;
        _initialPracticeDuration = practiceDuration;
        // Убираем подписку на события из конструктора - теперь это будет в OnActivate
    }

    public void EnableListeners()
    {
        EventMaster.current.CameraMoved += OnCameraMoved;
    }

    public void DisableListeners()
    {
        EventMaster.current.CameraMoved -= OnCameraMoved;
    }

    public void OnCameraMoved()
    {
        if (_practiceDuration > 0)
        {
            _practiceDuration -= Time.deltaTime;
        }
    }

    protected override void OnActivate()
    {
        // Подписываемся на события при активации
        EnableListeners();
    }
    
    protected override void OnDeactivate()
    {
        DisableListeners();
    }
    
    protected override void OnReset()
    {
        _practiceDuration = _initialPracticeDuration;
        DisableListeners();
    }

    public override bool IsComply()
    {
        return _practiceDuration <= 0;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}

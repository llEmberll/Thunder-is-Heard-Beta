

public class EnterOnObjectCondition : BasicCondition
{
    public string _targetObjectId;
    public float _times;
    private float _initialTimes;

    public EnterOnObjectCondition(string targetObjectId, int times) 
    { 
        _targetObjectId = targetObjectId;
        _times = times;
        _initialTimes = times;
        // Убираем EnableListeners() из конструктора - теперь это будет в OnActivate
    }

    public void EnableListeners()
    {
        EventMaster.current.EnteredOnObject += EnterOnObject;
    }

    public void DisableListeners()
    {
        EventMaster.current.EnteredOnObject -= EnterOnObject;
    }

    public void EnterOnObject(Entity obj)
    {
        if (obj.ChildId == _targetObjectId)
        {
            _times -= 1;
            if (_times < 1)
            {
                DisableListeners();
            }
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
        _times = _initialTimes;
        DisableListeners();
    }

    public override bool IsComply()
    {
        return _times < 1;
    }

    public override bool IsRealTimeUpdate()
    {
        return true;
    }
}

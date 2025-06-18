

public class EnterOnObjectCondition : BasicCondition
{
    public string _targetObjectId;
    public float _times;

    public EnterOnObjectCondition(string targetObjectId, int times) 
    { 
        _targetObjectId = targetObjectId;
        _times = times;
        EnableListeners();
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


    public override bool IsComply()
    {
        return _times < 1;
    }
}

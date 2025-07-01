

public class EnterOnFederationObjectCondition : BasicCondition
{
    public float _times;

    public EnterOnFederationObjectCondition( int times) 
    { 
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
        if (obj.side == Sides.federation)
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

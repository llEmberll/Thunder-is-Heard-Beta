public class EnterOnFederationUnitCondition : BasicCondition
{
    public float _times;

    public EnterOnFederationUnitCondition(int times) 
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
        // Проверяем, что объект является юнитом Федерации
        if (obj.side == Sides.federation && obj.Type == "Unit")
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
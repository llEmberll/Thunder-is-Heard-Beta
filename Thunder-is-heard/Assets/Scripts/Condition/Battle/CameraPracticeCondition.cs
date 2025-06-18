using System.Collections.Generic;
using UnityEngine;


public class CameraPracticeCondition : BasicCondition
{
    public float _practiceDuration;


    public CameraPracticeCondition(float practiceDuration) 
    { 
        _practiceDuration = practiceDuration;
        EventMaster.current.CameraMoved += OnCameraMoved;
    }

    public void OnCameraMoved()
    {
        if (_practiceDuration > 0)
        {
            _practiceDuration -= Time.deltaTime;
        }
    }


    public override bool IsComply()
    {
        return _practiceDuration > 0;
    }
}


using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public string _side;

    public BattleEngine _battleEngine;

    public void Start()
    {
        SetSide(Sides.empire);
        InitBattleEngine();
    }

    public void SetSide(string value)
    {
        _side = value;
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void EnableListeners()
    {
        EventMaster.current.NextTurn += OnNextTurn;
    }

    public void DisableListeners() 
    {
        EventMaster.current.NextTurn -= OnNextTurn;
    }

    public void OnNextTurn(string side)
    {
        if (side == _side)
        {
            // —генерировать ход
        }


    }


}

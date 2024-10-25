
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<string> _sides;

    public TurnData _turnData;

    public BattleEngine _battleEngine;
    public Map _map;

    public void Start()
    {
        SetSide(new List<string>() { Sides.empire, Sides.neutral });
        InitBattleEngine();
        InitMap();
        EnableListeners();
    }

    public void SetSide(List<string> value)
    {
        _sides = value;
    }

    public List<string> GetSide()
    {
        return _sides;
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public void InitMap()
    {
        _map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
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
        Debug.Log("AI: On next turn");

        if (_sides.Contains(side))
        {
            // —генерировать ход по стороне
            ClearTurnData();

            Debug.Log("AI: ходит за " + side);
            Execute();
        }
        else
        {
            Debug.Log("AI: side is federation");
        }
    }

    public void ClearTurnData()
    {
        _turnData = new TurnData();
    }

    public void Execute()
    {
        Debug.Log("AI: Execute");

        EventMaster.current.OnExecuteTurn(_turnData);
        ClearTurnData();
    }
}

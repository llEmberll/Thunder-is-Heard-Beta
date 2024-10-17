
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public string _side;

    public TurnData _turnData;

    public BattleEngine _battleEngine;
    public Map _map;

    public void Start()
    {
        SetSide(Sides.empire);
        InitBattleEngine();
        InitMap();
    }

    public void SetSide(string value)
    {
        _side = value;
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
        if (side == _side)
        {
            // —генерировать ход
            _turnData = new TurnData();
            Execute();
        }
    }

    public void ClearTurnData()
    {
        _turnData = new TurnData();
    }

    public void Execute()
    {
        EventMaster.current.OnExecuteTurn(_turnData);
        ClearTurnData();
    }
}

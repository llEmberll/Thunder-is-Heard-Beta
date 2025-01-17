
using System.Collections.Generic;
using UnityEngine;


public class AIAgent : MonoBehaviour
{
    public List<string> _sides;

    public TurnData _turnData;

    public BattleEngine _battleEngine;
    public Map _map;

    public Dictionary<string, AIInterface> AIBySide;


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
        EventMaster.current.NextStage += ResetAIByStage;
        EventMaster.current.BeginStage += ResetAIByStage;
    }

    public void DisableListeners() 
    {
        EventMaster.current.NextTurn -= OnNextTurn;
        EventMaster.current.NextStage -= ResetAIByStage;
        EventMaster.current.BeginStage -= ResetAIByStage;
    }

    public void ResetAIByStage(IStage nextStage)
    {
        if (AIBySide == null) AIBySide = new Dictionary<string, AIInterface>();

        AISettings[] AISettingsBySide = nextStage.AISettings;
        foreach (var item in AISettingsBySide)
        {
            if (_sides.Contains(item.side))
            {
                if (AIBySide.ContainsKey(item.side))
                {
                    AIBySide[item.side] = AIFactory.GetConfiguredAIByTypeAndSettings(item);
                }
                else
                {
                    AIBySide.Add(item.side, AIFactory.GetConfiguredAIByTypeAndSettings(item));
                }
            }
        }
    }

    public void OnNextTurn(string side)
    {
        if (_sides.Contains(side))
        {
            _turnData = AIBySide[side].GetTurn();
            Execute();
        }
        else
        {
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

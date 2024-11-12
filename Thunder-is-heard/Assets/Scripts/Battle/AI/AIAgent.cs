
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

        Dictionary<string, AISettings> AISettingsBySide = nextStage.AISettingsBySide;
        foreach (var item in AISettingsBySide)
        {
            if (_sides.Contains(item.Key))
            {
                if (AIBySide.ContainsKey(item.Key))
                {
                    AIBySide[item.Key] = AIFactory.GetConfiguredAIByTypeAndSettings(item.Value);
                }
                else
                {
                    AIBySide.Add(item.Key, AIFactory.GetConfiguredAIByTypeAndSettings(item.Value));
                }
            }
        }
    }

    public void OnNextTurn(string side)
    {
        Debug.Log("AI: On next turn");

        if (_sides.Contains(side))
        {
            _turnData = AIBySide[side].GetTurn();

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

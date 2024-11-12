using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAI : AIInterface
{
    public AISettings settings;
    public AISettings Settings {  get {  return settings; } }

    public BattleEngine _battleEngine;


    public virtual void Init()
    {
        InitBattleEngine();
    }

    public void InitBattleEngine()
    {
        _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
    }

    public abstract TurnData GetTurn();
}

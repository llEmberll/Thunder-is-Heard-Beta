using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AIInterface
{
    public AISettings Settings { get; }

    public TurnData GetTurn();
}

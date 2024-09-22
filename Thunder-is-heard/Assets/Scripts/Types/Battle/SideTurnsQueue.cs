using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SideTurnsQueue
{
    public static Dictionary<string, string> nextSideTurnByCurrentSide = new Dictionary<string, string>()
        {
            {Sides.federation, Sides.empire },
            {Sides.empire, Sides.neutral },
            {Sides.neutral, Sides.federation },
        };
}

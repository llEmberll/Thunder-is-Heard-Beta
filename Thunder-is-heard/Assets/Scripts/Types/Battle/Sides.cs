using System.Collections.Generic;


public static class Sides
{
    public static string federation = "Federation";
    public static string empire = "Empire";
    public static string neutral = "Neutral";

    public static Dictionary<string, string> nextSideTurnByCurrentSide = new Dictionary<string, string>()
        {
            {Sides.federation, Sides.empire },
            {Sides.empire, Sides.neutral },
            {Sides.neutral, Sides.federation },
        };

    public static Dictionary<string, string> enemySideBySide = new Dictionary<string, string>()
        {
            {Sides.federation, Sides.empire },
            {Sides.empire,  Sides.federation }
        };
}

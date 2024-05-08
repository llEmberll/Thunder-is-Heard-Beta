using MySqlX.XDevAPI.Common;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class TimeUtils
{
    public static string GetTimeAsStringByMinutes(int totalMinutes)
    {
        if (totalMinutes < 1)
        {
            return "мгновенно";
        }

        int days = totalMinutes / (24 * 60);
        int hours = (totalMinutes % (24 * 60)) / 60;
        int remainingMinutes = totalMinutes % 60;

        string result = "";
        if (days > 0)
        {
            result += $"{days}Д ";
        }
        if (hours > 0)
        {
            result += $"{hours}Ч ";
        }
        
        result += $"{remainingMinutes}М";
        
        return result.Trim();
    }
}

using MySqlX.XDevAPI.Common;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class TimeUtils
{
    public static string GetTimeAsStringBySeconds(int totalSeconds)
    {
        if (totalSeconds < 1)
        {
            return "���������";
        }

        int days = totalSeconds / (24 * 60 * 60);
        int hours = (totalSeconds % (24 * 60 * 60)) / 60;
        int remainingMinutes = totalSeconds % (60 * 60);

        string result = "";
        if (days > 0)
        {
            result += $"{days}� ";
        }
        if (hours > 0)
        {
            result += $"{hours}� ";
        }
        
        result += $"{remainingMinutes}�";
        
        return result.Trim();
    }
}

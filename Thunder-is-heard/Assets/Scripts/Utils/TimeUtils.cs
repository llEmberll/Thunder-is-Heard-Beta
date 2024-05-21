using MySqlX.XDevAPI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class TimeUtils
{
    public static string GetDHMTimeAsStringBySeconds(int totalSeconds)
    {
        if (totalSeconds < 1)
        {
            return "мгновенно";
        }

        int days = totalSeconds / (24 * 60 * 60);
        int hours = (totalSeconds % (24 * 60 * 60)) / 60;
        int remainingMinutes = totalSeconds % (60 * 60);

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

    public static string GetDHMSTimeAsStringBySeconds(int seconds)
    {
        if (seconds < 1)
        {
            return "мгновенно";
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string result = "";

        if (timeSpan.Days > 0)
        {
            result += $"{timeSpan.Days}Д ";
        }

        if (timeSpan.Hours > 0)
        {
            result += $"{timeSpan.Hours}Ч ";
        }

        if (timeSpan.Minutes > 0)
        {
            result += $"{timeSpan.Minutes}М ";
        }

        if (timeSpan.Seconds > 0)
        {
            result += $"{timeSpan.Seconds}С";
        }

        return result.TrimEnd();
    }
}

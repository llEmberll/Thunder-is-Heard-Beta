using MySqlX.XDevAPI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class TimeUtils
{
    public static string GetDHMTimeAsStringBySeconds(int seconds)
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
        else
        {
            if (result == "") 
            {
                result += $"{timeSpan.Seconds}С";
            }
        }

        return result.TrimEnd();
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

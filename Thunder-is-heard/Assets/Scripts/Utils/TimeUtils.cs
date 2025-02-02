using System;


public static class TimeUtils
{
    public static string GetDHMTimeAsStringBySeconds(int seconds)
    {
        if (seconds < 1)
        {
            return "���������";
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string result = "";

        if (timeSpan.Days > 0)
        {
            result += $"{timeSpan.Days}� ";
        }

        if (timeSpan.Hours > 0)
        {
            result += $"{timeSpan.Hours}� ";
        }

        if (timeSpan.Minutes > 0)
        {
            result += $"{timeSpan.Minutes}� ";
        }

        if (timeSpan.Seconds > 0) 
        {
            result += $"{timeSpan.Seconds}�";
        }

        return result.TrimEnd();
    }

    public static string GetDHMSTimeAsStringBySeconds(int seconds)
    {
        if (seconds < 1)
        {
            return "���������";
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string result = "";

        if (timeSpan.Days > 0)
        {
            result += $"{timeSpan.Days}� ";
        }

        if (timeSpan.Hours > 0)
        {
            result += $"{timeSpan.Hours}� ";
        }

        if (timeSpan.Minutes > 0)
        {
            result += $"{timeSpan.Minutes}� ";
        }

        if (timeSpan.Seconds > 0)
        {
            result += $"{timeSpan.Seconds}�";
        }

        return result.TrimEnd();
    }
}

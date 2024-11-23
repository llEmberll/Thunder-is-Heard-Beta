using System;


public static class TimeUtils
{
    public static string GetDHMTimeAsStringBySeconds(int seconds)
    {
        if (seconds < 1)
        {
            return "instantly";
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string result = "";

        if (timeSpan.Days > 0)
        {
            result += $"{timeSpan.Days}D ";
        }

        if (timeSpan.Hours > 0)
        {
            result += $"{timeSpan.Hours}H ";
        }

        if (timeSpan.Minutes > 0)
        {
            result += $"{timeSpan.Minutes}M ";
        }

        if (timeSpan.Seconds > 0) 
        {
            result += $"{timeSpan.Seconds}S";
        }

        return result.TrimEnd();
    }

    public static string GetDHMSTimeAsStringBySeconds(int seconds)
    {
        if (seconds < 1)
        {
            return "instantly";
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string result = "";

        if (timeSpan.Days > 0)
        {
            result += $"{timeSpan.Days}D ";
        }

        if (timeSpan.Hours > 0)
        {
            result += $"{timeSpan.Hours}H ";
        }

        if (timeSpan.Minutes > 0)
        {
            result += $"{timeSpan.Minutes}M ";
        }

        if (timeSpan.Seconds > 0)
        {
            result += $"{timeSpan.Seconds}S";
        }

        return result.TrimEnd();
    }
}

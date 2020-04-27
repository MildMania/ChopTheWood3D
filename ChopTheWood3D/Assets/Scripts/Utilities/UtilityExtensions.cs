using System;
using System.Collections.Generic;
using System.Linq;

static class UtilityExtensions
{
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }

    public static bool IsDefault<T>(this T value) where T : struct
    {
        bool isDefault = value.Equals(default(T));

        return isDefault;
    }

    public static string ToCascadedTimeString(this TimeSpan timeInterval)
    {
        if (timeInterval.TotalDays >= 1)
            return timeInterval.Days + "D:" + timeInterval.Hours + "H";
        if (timeInterval.TotalHours >= 1)
            return timeInterval.Hours + "H:" + timeInterval.Minutes + "M";
        else
            return timeInterval.Minutes + "M:" + timeInterval.Seconds + "S";
    }

    public static byte[] ToByteArray(this string str)
    {
        return System.Text.Encoding.ASCII.GetBytes(str);
    }
}
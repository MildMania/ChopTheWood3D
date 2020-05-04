using System;
using System.ComponentModel;
using UnityEngine;

static partial class Utilities
{

    public static T IdentifyObjectEnum<T>(string objectname)
    {
        T type = (T)Enum.Parse(typeof(T), objectname);
        return type;
    }

    public static IConvertible IdentifyObjectEnum(Type enumType, string objectname)
    {
        IConvertible type = (IConvertible)Enum.Parse(enumType, objectname);

        return type;
    }

    public static int HighestValueInEnum(Type enumType)
    {
        Array values = Enum.GetValues(enumType);
        int highestValue = (int)values.GetValue(0);
        for (int index = 0; index < values.Length; ++index)
        {
            if ((int)values.GetValue(index) > highestValue)
            {
                highestValue = (int)values.GetValue(index);
            }
        }

        return highestValue;
    }

    public static string GetValueAsString<T>(this T environment) where T : struct, IConvertible
    {
        // get the field 
        var field = environment.GetType().GetField(environment.ToString());
        var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (customAttributes.Length > 0)
        {
            return (customAttributes[0] as DescriptionAttribute).Description;
        }
        else
        {
            return environment.ToString();
        }
    }

    public static string GetFullName(this Enum myEnum)
    {
        return string.Format("{0}.{1}", myEnum.GetType().Name, myEnum.ToString());
    }
}

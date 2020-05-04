using System;
using UnityEngine;

public static partial class Utilities
{
    static System.Random _r;
    public static System.Random R
    {
        get
        {
            if (_r == null)
                _r = new System.Random();

            return _r;
        }
    }

    public static float NextFloat(float minValue = 0, float maxValue = 1)
    {
        double range = maxValue - (double)minValue;
        double sample = R.NextDouble();
        double scaled = (sample * range) + minValue;

        return (float)scaled;
    }

    public static int NextInt(int minValue, int maxValue)
    {
        return R.Next(minValue, maxValue);
    }

    public static Vector3 NextVector2(Vector2 min, Vector2 max)
    {
        Vector2 val = Vector2.zero;

        val.x = NextFloat(min.x, max.x);
        val.y = NextFloat(min.y, max.y);

        return val;
    }

    public static Vector3 NextVector3(Vector3 min, Vector3 max)
    {
        Vector3 val = Vector3.zero;

        val.x = NextFloat(min.x, max.x);
        val.y = NextFloat(min.y, max.y);
        val.z = NextFloat(min.z, max.z);

        return val;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            NextFloat(bounds.min.x, bounds.max.x),
            NextFloat(bounds.min.y, bounds.max.y),
            NextFloat(bounds.min.z, bounds.max.z)
        );
    }

    public static Vector2 RandomOnCircle(float radius)
    {
        return UnityEngine.Random.insideUnitCircle.normalized * radius;
    }

    public static Vector3 NextVector3(Bounds bounds)
    {
        return NextVector3(bounds.min, bounds.max);
    }

    public static int Combination(int n, int r)
    {  
        int Comb = 0;  
        Comb = (int)(Factorial(n) / (Factorial(r) * Factorial(n - r)));  
        return Comb;  
    }

    public static long Factorial(int num)
    {  
        long fact = 1;  
        for (int i = 2; i <= num; i++)
        {  
            fact = fact * i;  
        }  
        return fact;  
    }

    public static float Normalize(float val, float oldMax, float oldMin, float newMax, float newMin)
    {
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((val - oldMin) * newRange) / oldRange) + newMin;

        return newValue;
    }

    public static TimeSpan ConvertSecondsToTimeSpan(double value)
    {
        return TimeSpan.FromSeconds(value);
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }

    public static float Truncate(float value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (float)result;
    }


}

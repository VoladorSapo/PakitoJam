using UnityEngine;

public static class ConversionHelper
{
    public static float ToRadians(this float degrees) => degrees * Mathf.Deg2Rad;
    public static float ToDegrees(this float radians) => radians * Mathf.Rad2Deg;

    public static float Linear01ToDecibels(float t, float minDB = -80, float maxDB = 20)
    {
        t = Mathf.Clamp01(t);
        return minDB + (maxDB - minDB) * t;
    }
    public static float DecibelsToLinear01(float dB, float minDB = -80, float maxDB = 20)
    {
        dB = Mathf.Clamp(dB, minDB, maxDB);
        return (dB - minDB) / (maxDB - minDB);
    }

}
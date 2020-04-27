using UnityEngine;

public static class Vector3Extensions
{
    public static Vector2 GetXZPlanar(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static float GetXZPlanarDistance(Vector3 v1, Vector3 v2)
    {
        return Vector2.Distance(v1.GetXZPlanar(), v2.GetXZPlanar());
    }

    public static Vector2 GetXZPlanarDirecton(Vector3 v1, Vector3 v2)
    {
        return (v1.GetXZPlanar() - v2.GetXZPlanar()).normalized;
    }

    public static Vector3 GetXZPlanarWithSameY(this Vector3 v, Vector3 destination)
    {
        return new Vector3(
            destination.x,
            v.y,
            destination.z);
    }

    public static float AngleBetweenXZPlanar(Vector3 v1, Vector3 v2)
    {
        return Vector2.Angle(
            GetXZPlanar(v1),
            GetXZPlanar(v2));
    }

}

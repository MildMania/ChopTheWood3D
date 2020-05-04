﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Thrower))]
public class ThrowerEditor : Editor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
    static void DrawGizmo(Thrower t, GizmoType gizmoType)
    {
        DrawThrowPath(t);
        EditThrowableTransform(t);
    }

    private static void DrawThrowPath(Thrower t)
    {
        float step = 1.0f / t.Resolution;

        float curTime = 0;

        Vector3[] pointArr = new Vector3[(int)(t.SimulateDuration / step)];

        for(int i = 0; i < pointArr.Length; i++)
        {
            OrientedPoint p = t.GetOrientedPointAtTime(curTime);

            pointArr[i] = p.Position;

            curTime += step;
        }

        Handles.color = Color.red;
        Handles.DrawPolyLine(pointArr);
    }

    private static void EditThrowableTransform(Thrower t)
    {
        float time = t.SimulateTime;

        OrientedPoint p = t.GetOrientedPointAtTime(time);

        t.Throwable.transform.position = p.Position;

        t.Throwable.transform.rotation = p.Rotation;
    }
}
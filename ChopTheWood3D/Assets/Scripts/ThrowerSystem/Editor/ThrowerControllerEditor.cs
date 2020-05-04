using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThrowerController))]
public class ThrowerControllerEditor : Editor
{
    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
    static void DrawGizmo(ThrowerController tc, GizmoType gizmoType)
    {
        UpdateThrowers(tc);
    }

    private static void UpdateThrowers(ThrowerController tc)
    {
        if (tc.Throwers == null)
            return;

        foreach(Thrower t in tc.Throwers)
        {
            t.ShowSimulation = tc.ShowSimulation;
            t.SimulateDuration = tc.SimulateDuration;
            t.SimulateTime = tc.SimulateTime;
        }
    }
}

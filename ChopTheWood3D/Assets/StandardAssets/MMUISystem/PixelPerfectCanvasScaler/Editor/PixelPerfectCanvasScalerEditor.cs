using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PixelPerfectCanvasScaler))]
[CanEditMultipleObjects]
public class PixelPerfectCanvasScalerEditor : Editor
{
    PixelPerfectCanvasScaler _myTarget;

    private void OnEnable()
    {
        _myTarget = (PixelPerfectCanvasScaler)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        _myTarget.scaleFactor = EditorGUILayout.IntField("Scale Factor", (int)_myTarget.scaleFactor);
        GUI.enabled = true;

        EditorGUI.BeginChangeCheck();

        _myTarget.ScreenCameraToGetScreenSize = (Camera)EditorGUILayout.ObjectField("Camera To Get Screen Size", _myTarget.ScreenCameraToGetScreenSize, typeof(Camera), true);
        _myTarget.referenceResolution = EditorGUILayout.Vector2Field("Reference Resolution", _myTarget.referenceResolution);
        _myTarget.referencePixelsPerUnit = EditorGUILayout.IntField("Reference Pixels Per Unit", (int)_myTarget.referencePixelsPerUnit);

        if (EditorGUI.EndChangeCheck())
        {
            ((PixelPerfectCanvasScaler)target).CalculateNewScaleFactor(true);

            EditorUtility.SetDirty(_myTarget);
        }
    }
}

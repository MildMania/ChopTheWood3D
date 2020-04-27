using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldInputSettings", menuName = "Settings/Input Settings/World Input Settings", order = 1)]
public class WorldInputSettings : ScriptableObject
{
    public float MoveFromStartPosInchTreshold = 1.0f;
    [Space(10)]

    [Header("Tap Settings")]
    public float TapInchTreshold = 0.2f;
    public float TapDuration = 0.2f;
    [Space(10)]

    [Header("Drag Settings")]
    public float InitialDragInchTreshold;
    public float PerFrameDragInchTreshold;
    [Space(10)]

    [Header("Swipe Settings")]
    public float SwipeInchTreshold = 0.3f;
    public float MoveForSwipeInchTreshold = 0.2f;
    public float SwipeHorAngleTrashold = 45f;
}

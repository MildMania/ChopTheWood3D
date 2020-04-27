using UnityEngine;
using System.Collections;

public class Input_WI_OnDragEnd : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnDragEnd(int fingerIndex, Vector2 fingerPos)
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
    }
}

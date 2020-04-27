using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_WI_OnDragPaused : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnDragPaused(int fingerIndex, Vector2 fingerPos)
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
    }
}

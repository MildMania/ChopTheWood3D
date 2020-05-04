using UnityEngine;
using System.Collections;

public class Input_WI_OnFingerDown : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnFingerDown(int fingerIndex, Vector2 pos)
    {
        FingerIndex = fingerIndex;
        FingerPos = pos;
    }
}

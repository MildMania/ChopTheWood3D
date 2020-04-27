using UnityEngine;
using System.Collections;

public class Input_WI_OnTap : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnTap(int fingerIndex, Vector2 pos)
    {
        FingerIndex = fingerIndex;
        FingerPos = pos;
    }
}
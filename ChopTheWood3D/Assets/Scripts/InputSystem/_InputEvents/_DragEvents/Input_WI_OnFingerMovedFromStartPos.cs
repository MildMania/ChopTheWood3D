using UnityEngine;

public class Input_WI_OnFingerMovedFromStartPos : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnFingerMovedFromStartPos(int fingerIndex, Vector2 fingerPos)
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
    }
}

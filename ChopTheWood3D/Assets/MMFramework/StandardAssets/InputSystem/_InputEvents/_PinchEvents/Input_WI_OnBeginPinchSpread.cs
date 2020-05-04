using UnityEngine;

public class Input_WI_OnBeginPinchSpread : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnBeginPinchSpread(int fingerIndex, Vector2 pos)
    {
        FingerIndex = fingerIndex;
        FingerPos = pos;
    }
}

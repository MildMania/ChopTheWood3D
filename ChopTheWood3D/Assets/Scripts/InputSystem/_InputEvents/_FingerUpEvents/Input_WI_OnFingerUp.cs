using UnityEngine;

public class Input_WI_OnFingerUp : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }

    public Input_WI_OnFingerUp(int fingerIndex, Vector2 pos)
    {
        FingerIndex = fingerIndex;
        FingerPos = pos;
    }
}

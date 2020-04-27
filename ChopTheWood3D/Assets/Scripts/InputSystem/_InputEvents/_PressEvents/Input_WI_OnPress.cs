using UnityEngine;

public class Input_WI_OnPress : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }
    public float Duration { get; private set; }

    public Input_WI_OnPress(int fingerIndex, Vector2 fingerPos, float duration)
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
        Duration = duration;
    }
}

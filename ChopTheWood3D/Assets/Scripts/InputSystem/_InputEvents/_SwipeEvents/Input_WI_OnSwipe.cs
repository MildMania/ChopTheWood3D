using UnityEngine;

public class Input_WI_OnSwipe : InputEvent
{
    public int FingerIndex { get; private set; }
    public Vector2 FingerPos { get; private set; }
    public ESwipeDirection SwipeDirection { get; private set; }
    public float Velocity { get; private set; }

    public Input_WI_OnSwipe(int fingerIndex, Vector2 fingerPos, ESwipeDirection swipeDirection, float velocity)
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
        SwipeDirection = swipeDirection;
        Velocity = velocity;
    }
}

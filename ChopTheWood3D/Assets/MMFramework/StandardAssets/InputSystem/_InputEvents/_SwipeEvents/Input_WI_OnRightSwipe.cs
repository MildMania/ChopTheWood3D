using UnityEngine;

public class Input_WI_OnRightSwipe : Input_WI_OnSwipe
{
    public Input_WI_OnRightSwipe(int fingerIndex, Vector2 fingerPos, ESwipeDirection swipeDirection, float swipeDelta)
        : base(fingerIndex, fingerPos, swipeDirection, swipeDelta)
    {
    }
}

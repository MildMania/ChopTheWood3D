using UnityEngine;

public class Input_WI_OnLeftSwipe : Input_WI_OnSwipe
{
    public Input_WI_OnLeftSwipe(int fingerIndex, Vector2 fingerPos, ESwipeDirection swipeDirection, float swipeDelta) 
        : base(fingerIndex, fingerPos, swipeDirection, swipeDelta)
    {
    }
}

using System;
using UnityEngine;

public abstract class WorldInputControllerBase : InputControllerBase
{
    #region Tap Events
    public Action<Input_WI_OnTap> OnTap;

    protected void FireOnTap(int fingerIndex, Vector2 fingerPos)
    {
        if (OnTap != null)
            OnTap(new Input_WI_OnTap(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnLeftTap> OnLeftTap;

    protected void FireOnLeftTap(int fingerIndex, Vector2 fingerPos)
    {
        if (OnLeftTap != null)
            OnLeftTap(new Input_WI_OnLeftTap(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnRightTap> OnRightTap;

    protected void FireOnRightTap(int fingerIndex, Vector2 fingerPos)
    {
        if (OnRightTap != null)
            OnRightTap(new Input_WI_OnRightTap(fingerIndex, fingerPos));
    }
    #endregion

    #region FingerDown Events
    public Action<Input_WI_OnFingerDown> OnFingerDown;

    protected void FireOnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        if (OnFingerDown != null)
            OnFingerDown(new Input_WI_OnFingerDown(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnLeftFingerDown> OnLeftFingerDown;

    protected void FireOnLeftFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        if (OnLeftFingerDown != null)
            OnLeftFingerDown(new Input_WI_OnLeftFingerDown(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnRightFingerDown> OnRightFingerDown;

    protected void FireOnRightFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        if (OnRightFingerDown != null)
            OnRightFingerDown(new Input_WI_OnRightFingerDown(fingerIndex, fingerPos));
    }

    #endregion

    #region FingerUp Events
    public Action<Input_WI_OnFingerUp> OnFingerUp;

    protected void FireOnFingerUp(int fingerIndex, Vector2 fingerPos)
    {
        if (OnFingerUp != null)
            OnFingerUp(new Input_WI_OnFingerUp(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnLeftFingerUp> OnLeftFingerUp;

    protected void FireOnLeftFingerUp(int fingerIndex, Vector2 fingerPos)
    {
        if (OnLeftFingerUp != null)
            OnLeftFingerUp(new Input_WI_OnLeftFingerUp(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnRightFingerUp> OnRightFingerUp;

    protected void FireOnRightFingerUp(int fingerIndex, Vector2 fingerPos)
    {
        if (OnRightFingerUp != null)
            OnRightFingerUp(new Input_WI_OnRightFingerUp(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnFingerUpOnStartPos> OnFingerUpOnStartPos;

    protected void FireOnFingerUpOnStartPos(int fingerIndex, Vector2 fingerPos)
    {
        if (OnFingerUpOnStartPos != null)
            OnFingerUpOnStartPos(new Input_WI_OnFingerUpOnStartPos(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnFingerUpNotOnStartPos> OnFingerUpNotOnStartPos;

    protected void FireOnFingerUpNotOnStartPos(int fingerIndex, Vector2 fingerPos)
    {
        if (OnFingerUpNotOnStartPos != null)
            OnFingerUpNotOnStartPos(new Input_WI_OnFingerUpNotOnStartPos(fingerIndex, fingerPos));
    }

    #endregion

    #region PressEvents
    public Action<Input_WI_OnPress> OnPress;

    protected void FireOnPress(int fingerIndex, Vector2 fingerPos, float duration)
    {
        if (OnPress != null)
            OnPress(new Input_WI_OnPress(fingerIndex, fingerPos, duration));
    }

    public Action<Input_WI_OnLeftPress> OnLeftPress;

    protected void FireOnLeftPress(int fingerIndex, Vector2 fingerPos, float duration)
    {
        if (OnLeftPress != null)
            OnLeftPress(new Input_WI_OnLeftPress(fingerIndex, fingerPos, duration));
    }

    public Action<Input_WI_OnRightPress> OnRightPress;

    protected void FireOnRightPress(int fingerIndex, Vector2 fingerPos, float duration)
    {
        if (OnRightPress != null)
            OnRightPress(new Input_WI_OnRightPress(fingerIndex, fingerPos, duration));
    }
    #endregion

    #region Swipe Events
    public Action<Input_WI_OnSwipe> OnSwipe;

    protected void FireOnSwipe(int fingerIndex, Vector2 fingerPos, ESwipeDirection swipeDirection, float swipeDelta)
    {
        if (OnSwipe != null)
            OnSwipe(new Input_WI_OnSwipe(fingerIndex, fingerPos, swipeDirection, swipeDelta));
    }

    public Action<Input_WI_OnLeftSwipe> OnLeftSwipe;

    protected void FireOnLeftSwipe(int fingerIndex, Vector2 fingerPos, ESwipeDirection swipeDirection, float swipeDelta)
    {
        if (OnLeftSwipe != null)
            OnLeftSwipe(new Input_WI_OnLeftSwipe(fingerIndex, fingerPos, swipeDirection, swipeDelta));
    }

    public Action<Input_WI_OnRightSwipe> OnRightSwipe;

    protected void FireOnRightSwipe(int fingerIndex, Vector2 fingerPos, ESwipeDirection swipeDirection, float swipeDelta)
    {
        if (OnRightSwipe != null)
            OnRightSwipe(new Input_WI_OnRightSwipe(fingerIndex, fingerPos, swipeDirection, swipeDelta));
    }
    #endregion

    #region Drag Events

    public Action<Input_WI_OnDragBegin> OnDragBegin;

    protected void FireOnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 dragDelta)
    {
        if (OnDragBegin != null)
            OnDragBegin(new Input_WI_OnDragBegin(fingerIndex, fingerPos, dragDelta));
    }

    public Action<Input_WI_OnDragMove> OnDragMove;

    protected void FireOnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 dragDelta)
    {
        if (OnDragMove != null)
            OnDragMove(new Input_WI_OnDragMove(fingerIndex, fingerPos, dragDelta));
    }

    public Action<Input_WI_OnDragEnd> OnDragEnd;

    protected void FireOnDragEnd(int fingerIndex, Vector2 fingerPos)
    {
        if (OnDragEnd != null)
            OnDragEnd(new Input_WI_OnDragEnd(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnDragPaused> OnDragPaused;

    protected void FireOnDragPaused(int fingerIndex, Vector2 fingerPos)
    {
        if (OnDragPaused != null)
            OnDragPaused(new Input_WI_OnDragPaused(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnFingerMovedFromStartPos> OnFingerMovedFromStartPos;

    protected void FireOnFingerMovedFromStartPos(int fingerIndex, Vector2 fingerPos)
    {
        if (OnFingerMovedFromStartPos != null)
            OnFingerMovedFromStartPos(new Input_WI_OnFingerMovedFromStartPos(fingerIndex, fingerPos));
    }

    #endregion

    #region Pinch Events
    public Action<Input_WI_OnBeginPinchSpread> OnBeginPinchSpread;

    protected void FireOnBeginPinchSpread(int fingerIndex, Vector2 fingerPos)
    {
        if (OnBeginPinchSpread != null)
            OnBeginPinchSpread(new Input_WI_OnBeginPinchSpread(fingerIndex, fingerPos));
    }

    public Action<Input_WI_OnPinchSpread> OnPinchSpread;

    protected void FireOnPinchSpread(WorldInputManager.PinchSpreadData pinchData)
    {
        if (OnPinchSpread != null)
            OnPinchSpread(new Input_WI_OnPinchSpread(pinchData));
    }

    public Action<Input_WI_OnEndPinchSpread> OnEndPinchSpread;

    protected void FireOnEndPinchSpread(int fingerIndex)
    {
        if (OnEndPinchSpread != null)
            OnEndPinchSpread(new Input_WI_OnEndPinchSpread(fingerIndex));
    }
    #endregion

    #region Scroll Events
    public Action<Input_WI_OnMouseScroll> OnMouseScroll;

    protected void FireOnMouseScroll(float scrollAmount)
    {
        if (OnMouseScroll != null)
            OnMouseScroll(new Input_WI_OnMouseScroll(scrollAmount));
    }
    #endregion

    protected WorldInputControllerBase()
    {
        StartListeningEvents();
    }

    protected override void DisposeCustomActions()
    {
        FinishListeningEvents();

        base.DisposeCustomActions();
    }

    protected void StartListeningEvents()
    {
        WorldInputManager.OnTap += FireOnTap;
        WorldInputManager.OnLeftTap += FireOnLeftTap;
        WorldInputManager.OnRightTap += FireOnRightTap;

        WorldInputManager.OnFingerDown += FireOnFingerDown;
        WorldInputManager.OnLeftFingerDown += FireOnLeftFingerDown;
        WorldInputManager.OnRightFingerDown += FireOnRightFingerDown;

        WorldInputManager.OnFingerUp += FireOnFingerUp;
        WorldInputManager.OnLeftFingerUp += FireOnLeftFingerUp;
        WorldInputManager.OnRightFingerUp += FireOnRightFingerUp;
        WorldInputManager.OnFingerUpOnStartPos += FireOnFingerUpOnStartPos;
        WorldInputManager.OnFingerUpNotOnStartPos += FireOnFingerUpNotOnStartPos;

        WorldInputManager.OnPress += FireOnPress;
        WorldInputManager.OnLeftPress += FireOnLeftPress;
        WorldInputManager.OnRightPress += FireOnRightPress;

        WorldInputManager.OnSwipe += FireOnSwipe;
        WorldInputManager.OnLeftSwipe += FireOnLeftSwipe;
        WorldInputManager.OnRightSwipe += FireOnRightSwipe;

        WorldInputManager.OnDragBegin += FireOnDragBegin;
        WorldInputManager.OnDragMove += FireOnDragMove;
        WorldInputManager.OnDragEnd += FireOnDragEnd;
        WorldInputManager.OnDragPaused += FireOnDragPaused;
        WorldInputManager.OnFingerMovedFromStartPos += FireOnFingerMovedFromStartPos;

        WorldInputManager.OnBeginPinchSpread += FireOnBeginPinchSpread;
        WorldInputManager.OnPinchSpread += FireOnPinchSpread;
        WorldInputManager.OnEndPinchSpread += FireOnEndPinchSpread;

        WorldInputManager.OnMouseScroll += FireOnMouseScroll;

        StartListeningCustomEvents();
    }

    protected void FinishListeningEvents()
    {
        WorldInputManager.OnTap -= FireOnTap;
        WorldInputManager.OnLeftTap -= FireOnLeftTap;
        WorldInputManager.OnRightTap -= FireOnRightTap;

        WorldInputManager.OnFingerDown -= FireOnFingerDown;
        WorldInputManager.OnLeftFingerDown -= FireOnLeftFingerDown;
        WorldInputManager.OnRightFingerDown -= FireOnRightFingerDown;

        WorldInputManager.OnFingerUp -= FireOnFingerUp;
        WorldInputManager.OnLeftFingerUp -= FireOnLeftFingerUp;
        WorldInputManager.OnRightFingerUp -= FireOnRightFingerUp;
        WorldInputManager.OnFingerUpOnStartPos -= FireOnFingerUpOnStartPos;
        WorldInputManager.OnFingerUpNotOnStartPos -= FireOnFingerUpNotOnStartPos;

        WorldInputManager.OnPress -= FireOnPress;
        WorldInputManager.OnLeftPress -= FireOnLeftPress;
        WorldInputManager.OnRightPress -= FireOnRightPress;

        WorldInputManager.OnSwipe -= FireOnSwipe;
        WorldInputManager.OnLeftSwipe -= FireOnLeftSwipe;
        WorldInputManager.OnRightSwipe -= FireOnRightSwipe;

        WorldInputManager.OnDragBegin -= FireOnDragBegin;
        WorldInputManager.OnDragMove -= FireOnDragMove;
        WorldInputManager.OnDragEnd -= FireOnDragEnd;
        WorldInputManager.OnDragPaused -= FireOnDragPaused;
        WorldInputManager.OnFingerMovedFromStartPos -= FireOnFingerMovedFromStartPos;

        WorldInputManager.OnBeginPinchSpread -= FireOnBeginPinchSpread;
        WorldInputManager.OnPinchSpread -= FireOnPinchSpread;
        WorldInputManager.OnEndPinchSpread -= FireOnEndPinchSpread;

        WorldInputManager.OnMouseScroll -= FireOnMouseScroll;

        FinishListeningCustomEvents();
    }

    protected virtual void StartListeningCustomEvents()
    {

    }

    protected virtual void FinishListeningCustomEvents()
    {

    }
}

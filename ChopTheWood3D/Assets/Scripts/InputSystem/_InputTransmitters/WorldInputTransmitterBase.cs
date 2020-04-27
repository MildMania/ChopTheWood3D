public abstract class WorldInputTransmitterBase : InputTransmitter
{
    protected WorldInputControllerBase WIControllerBase { get { return (WorldInputControllerBase)GetInputController(); } }

    protected override void RegisterToInputControllerEvents()
    {
        WIControllerBase.OnTap += OnTap;
        WIControllerBase.OnLeftTap += OnLeftTap;
        WIControllerBase.OnRightTap += OnRightTap;

        WIControllerBase.OnFingerDown += OnFingerDown;
        WIControllerBase.OnLeftFingerDown += OnLeftFingerDown;
        WIControllerBase.OnRightFingerDown += OnRightFingerDown;

        WIControllerBase.OnFingerUp += OnFingerUp;
        WIControllerBase.OnLeftFingerUp += OnLeftFingerUp;
        WIControllerBase.OnRightFingerUp += OnRightFingerUp;
        WIControllerBase.OnFingerUpOnStartPos += OnFingerUpOnStartPos;
        WIControllerBase.OnFingerUpNotOnStartPos += OnFingerUpNotOnStartPos;

        WIControllerBase.OnPress += OnPress;
        WIControllerBase.OnLeftPress += OnLeftPress;
        WIControllerBase.OnRightPress += OnRightPress;

        WIControllerBase.OnSwipe += OnSwipe;
        WIControllerBase.OnLeftSwipe += OnLeftSwipe;
        WIControllerBase.OnRightSwipe += OnRightSwipe;

        WIControllerBase.OnDragBegin += OnDragBegin;
        WIControllerBase.OnDragMove += OnDragMove;
        WIControllerBase.OnDragEnd += OnDragEnd;
        WIControllerBase.OnDragPaused += OnDragPaused;
        WIControllerBase.OnFingerMovedFromStartPos += OnFingerMovedFromStartPos;

        WIControllerBase.OnBeginPinchSpread += OnBeginPinchSpread;
        WIControllerBase.OnPinchSpread += OnPinchSpread;
        WIControllerBase.OnEndPinchSpread += OnEndPinchSpread;

        WIControllerBase.OnMouseScroll += OnMouseScroll;
    }

    protected override void UnregisterToInputControllerEvents()
    {
        if (WIControllerBase == null)
            return;

        WIControllerBase.OnTap -= OnTap;
        WIControllerBase.OnLeftTap -= OnLeftTap;
        WIControllerBase.OnRightTap -= OnRightTap;

        WIControllerBase.OnFingerDown -= OnFingerDown;
        WIControllerBase.OnLeftFingerDown -= OnLeftFingerDown;
        WIControllerBase.OnRightFingerDown -= OnRightFingerDown;

        WIControllerBase.OnFingerUp -= OnFingerUp;
        WIControllerBase.OnLeftFingerUp -= OnLeftFingerUp;
        WIControllerBase.OnRightFingerUp -= OnRightFingerUp;
        WIControllerBase.OnFingerUpOnStartPos -= OnFingerUpOnStartPos;
        WIControllerBase.OnFingerUpNotOnStartPos -= OnFingerUpNotOnStartPos;

        WIControllerBase.OnPress -= OnPress;
        WIControllerBase.OnLeftPress -= OnLeftPress;
        WIControllerBase.OnRightPress -= OnRightPress;

        WIControllerBase.OnSwipe -= OnSwipe;
        WIControllerBase.OnLeftSwipe -= OnLeftSwipe;
        WIControllerBase.OnRightSwipe -= OnRightSwipe;

        WIControllerBase.OnDragBegin -= OnDragBegin;
        WIControllerBase.OnDragMove -= OnDragMove;
        WIControllerBase.OnDragEnd -= OnDragEnd;
        WIControllerBase.OnDragPaused -= OnDragPaused;
        WIControllerBase.OnFingerMovedFromStartPos -= OnFingerMovedFromStartPos;

        WIControllerBase.OnBeginPinchSpread -= OnBeginPinchSpread;
        WIControllerBase.OnPinchSpread -= OnPinchSpread;
        WIControllerBase.OnEndPinchSpread -= OnEndPinchSpread;

        WIControllerBase.OnMouseScroll -= OnMouseScroll;
    }

    void OnTap(Input_WI_OnTap e)
    {
        Raise(typeof(Input_WI_OnTap), e);
    }

    void OnLeftTap(Input_WI_OnLeftTap e)
    {
        Raise(typeof(Input_WI_OnLeftTap), e);
    }

    void OnRightTap(Input_WI_OnRightTap e)
    {
        Raise(typeof(Input_WI_OnRightTap), e);
    }

    void OnFingerDown(Input_WI_OnFingerDown e)
    {
        Raise(typeof(Input_WI_OnFingerDown), e);
    }

    void OnLeftFingerDown(Input_WI_OnLeftFingerDown e)
    {
        Raise(typeof(Input_WI_OnLeftFingerDown), e);
    }

    void OnRightFingerDown(Input_WI_OnRightFingerDown e)
    {
        Raise(typeof(Input_WI_OnRightFingerDown), e);
    }

    void OnFingerUp(Input_WI_OnFingerUp e)
    {
        Raise(typeof(Input_WI_OnFingerUp), e);
    }

    void OnLeftFingerUp(Input_WI_OnLeftFingerUp e)
    {
        Raise(typeof(Input_WI_OnLeftFingerUp), e);
    }

    void OnRightFingerUp(Input_WI_OnRightFingerUp e)
    {
        Raise(typeof(Input_WI_OnRightFingerUp), e);
    }

    void OnFingerUpOnStartPos(Input_WI_OnFingerUpOnStartPos e)
    {
        Raise(typeof(Input_WI_OnFingerUpOnStartPos), e);
    }

    void OnFingerUpNotOnStartPos(Input_WI_OnFingerUpNotOnStartPos e)
    {
        Raise(typeof(Input_WI_OnFingerUpNotOnStartPos), e);
    }

    void OnPress(Input_WI_OnPress e)
    {
        Raise(typeof(Input_WI_OnPress), e);
    }

    void OnLeftPress(Input_WI_OnLeftPress e)
    {
        Raise(typeof(Input_WI_OnLeftPress), e);
    }

    void OnRightPress(Input_WI_OnRightPress e)
    {
        Raise(typeof(Input_WI_OnRightPress), e);
    }

    void OnSwipe(Input_WI_OnSwipe e)
    {
        Raise(typeof(Input_WI_OnSwipe), e);
    }

    void OnLeftSwipe(Input_WI_OnLeftSwipe e)
    {
        Raise(typeof(Input_WI_OnLeftSwipe), e);
    }

    void OnRightSwipe(Input_WI_OnRightSwipe e)
    {
        Raise(typeof(Input_WI_OnRightSwipe), e);
    }

    void OnDragBegin(Input_WI_OnDragBegin e)
    {
        Raise(typeof(Input_WI_OnDragBegin), e);
    }

    void OnDragMove(Input_WI_OnDragMove e)
    {
        Raise(typeof(Input_WI_OnDragMove), e);
    }

    void OnDragEnd(Input_WI_OnDragEnd e)
    {
        Raise(typeof(Input_WI_OnDragEnd), e);
    }

    void OnDragPaused(Input_WI_OnDragPaused e)
    {
        Raise(typeof(Input_WI_OnDragPaused), e);
    }

    void OnFingerMovedFromStartPos(Input_WI_OnFingerMovedFromStartPos e)
    {
        Raise(typeof(Input_WI_OnFingerMovedFromStartPos), e);
    }

    void OnBeginPinchSpread(Input_WI_OnBeginPinchSpread e)
    {
        Raise(typeof(Input_WI_OnBeginPinchSpread), e);
    }

    void OnPinchSpread(Input_WI_OnPinchSpread e)
    {
        Raise(typeof(Input_WI_OnPinchSpread), e);
    }

    void OnEndPinchSpread(Input_WI_OnEndPinchSpread e)
    {
        Raise(typeof(Input_WI_OnEndPinchSpread), e);
    }

    void OnMouseScroll(Input_WI_OnMouseScroll e)
    {
        Raise(typeof(Input_WI_OnMouseScroll), e);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum Directions
{
    Vertical,
    Horizontal
}

public enum ScreenArea
{
    None,
    Left,
    Right
}

public class WorldInputManager : MonoBehaviour
{
    private class WorldInputTouch
    {
        public Vector2 StartPoint;
        public Vector2 PrevPoint;
        public Vector2 TouchPoint;
        public Vector2 DeltaMove;
        public bool IsTouchDown;
        public bool IsMovedForDrag;
        public bool IsMovedFromStartPos;
        public bool IsMovedForSwipe;
        public Vector2 SwipeStartPoint;
        public ESwipeDirection SwipeDirection;
        public bool IsSwipePossible;
        public float StartTime;
        public float LastMoveTime;
        public ScreenArea TouchStartScreenArea;
        public ScreenArea TouchContinueScreenArea;
    }

    public class PinchSpreadData
    {
        public int RefFingerID;
        public int FingerID;
        public Vector2 PinchSpreadAnchorInViewport;
        public Vector2 RefInputPrevPos, RefInputCurPos;
        public Vector2 InputPrevPos, InputCurPos;

        public void CalculatePinchSpreadAnchor(Vector2 refInputTouchPoint, Vector2 inputTouchPoint)
        {
            PinchSpreadAnchorInViewport = Camera.main.ScreenToViewportPoint((refInputTouchPoint + inputTouchPoint) / 2f);
        }

        public void UpdateRefInput(Vector2 prevPoint, Vector2 touchPoint)
        {
            RefInputPrevPos = prevPoint;
            RefInputCurPos = touchPoint;
        }

        public void UpdateInput(Vector2 prevPoint, Vector2 touchPoint)
        {
            InputPrevPos = prevPoint;
            InputCurPos = touchPoint;
        }
    }

    public WorldInputSettings InputSettings;

    float _tapPixelTreshold;

    float _initialDragPixelTreshold;

    float _perFrameDragPixelTreshold;

    float _swipePixelTreshold;

    float _moveForSwipePixelTreshold;

    float _moveFromStartPosPixelTreshold;

    public int MaxInputCount = 5;


    public bool IsContinuousSwipeEnabled;

    private static WorldInputManager _instance;

    public static WorldInputManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<WorldInputManager>();

            return _instance;
        }
    }

    #region Gesture Variables

    private Dictionary<int, WorldInputTouch> _inputDataDict = new Dictionary<int, WorldInputTouch>();
    private List<PinchSpreadData> _pinchSpreadDataList = new List<PinchSpreadData>();
    
    #endregion

    #region Events

    public static Action<int, Vector2> OnLeftTap;
    public static Action<int, Vector2> OnRightTap;
    public static Action<int, Vector2, float> OnPress;
    public static Action<int, Vector2> OnTap;
    public static Action<int, Vector2, float> OnLeftPress;
    public static Action<int, Vector2, float> OnRightPress;
    public static Action<int, Vector2> OnFingerDown;
    public static Action<int, Vector2> OnLeftFingerDown;
    public static Action<int, Vector2> OnRightFingerDown;
    public static Action<int, Vector2> OnLeftFingerUp;
    public static Action<int, Vector2> OnRightFingerUp;
    public static Action<int, Vector2> OnFingerUp;
    public static Action<int, Vector2> OnFingerMovedFromStartPos;
    public static Action<int, Vector2> OnFingerUpOnStartPos;
    public static Action<int, Vector2> OnFingerUpNotOnStartPos;

    public static Action<int, Vector2> OnBeginPinchSpread;
    public static Action<PinchSpreadData> OnPinchSpread;
    public static Action<int> OnEndPinchSpread;

    public static Action<int, Vector2, Vector2> OnDragBegin;
    public static Action<int, Vector2, Vector2> OnDragMove;
    public static Action<int, Vector2> OnDragEnd;
    public static Action<int, Vector2> OnDragPaused;


    public static Action<int, Vector2, ESwipeDirection, float> OnSwipe;
    public static Action<int, Vector2, ESwipeDirection, float> OnLeftSwipe;
    public static Action<int, Vector2, ESwipeDirection, float> OnRightSwipe;

    public static Action<float> OnMouseScroll;


    private void FireOnTap(int fingerId, Vector2 fingerPos)
    {
        if (OnTap != null)
            OnTap(fingerId, fingerPos);
    }

    private void FireOnLeftTap(int fingerId, Vector2 fingerPos)
    {
        if (OnLeftTap != null)
            OnLeftTap(fingerId, fingerPos);
    }

    private void FireOnRightTap(int fingerId, Vector2 fingerPos)
    {
        if (OnRightTap != null)
            OnRightTap(fingerId, fingerPos);
    }

    private void FireOnPress(int fingerId, Vector2 fingerPos, float duration)
    {
        if (OnPress != null)
            OnPress(fingerId, fingerPos, duration);
    }

    private void FireOnLeftPress(int fingerId, Vector2 fingerPos, float duration)
    {
        if (OnLeftPress != null)
            OnLeftPress(fingerId, fingerPos, duration);
    }

    private void FireOnRightPress(int fingerId, Vector2 fingerPos, float duration)
    {
        if (OnRightPress != null)
            OnRightPress(fingerId, fingerPos, duration);
    }

    private void FireOnFingerDown(int fingerId, Vector2 fingerPos)
    {
        if (OnFingerDown != null)
            OnFingerDown(fingerId, fingerPos);
    }

    private void FireOnLeftFingerDown(int fingerId, Vector2 fingerPos)
    {
        if (OnLeftFingerDown != null)
            OnLeftFingerDown(fingerId, fingerPos);
    }

    private void FireOnRightFingerDown(int fingerId, Vector2 fingerPos)
    {
        if (OnRightFingerDown != null)
            OnRightFingerDown(fingerId, fingerPos);
    }

    private void FireOnFingerUp(int fingerId, Vector2 fingerPos)
    {
        if (OnFingerUp != null)
            OnFingerUp(fingerId, fingerPos);
    }

    private void FireOnLeftFingerUp(int fingerId, Vector2 fingerPos)
    {
        if (OnLeftFingerUp != null)
            OnLeftFingerUp(fingerId, fingerPos);
    }

    private void FireOnRightFingerUp(int fingerId, Vector2 fingerPos)
    {
        if (OnRightFingerUp != null)
            OnRightFingerUp(fingerId, fingerPos);
    }

    private void FireOnFingerMovedFromStartPos(int fingerId, Vector2 fingerPos)
    {
        if (OnFingerMovedFromStartPos != null)
            OnFingerMovedFromStartPos(fingerId, fingerPos);
    }

    private void FireOnFingerUpOnStartPos(int fingerId, Vector2 fingerPos)
    {
        if (OnFingerUpOnStartPos != null)
            OnFingerUpOnStartPos(fingerId, fingerPos);
    }

    private void FireOnFingerUpNotOnStartPos(int fingerId, Vector2 fingerPos)
    {
        if (OnFingerUpNotOnStartPos != null)
            OnFingerUpNotOnStartPos(fingerId, fingerPos);
    }

    private void FireOnDragBegin(int fingerId, Vector2 fingerPos, Vector2 startPos)
    {
        if (OnDragBegin != null)
            OnDragBegin(fingerId, fingerPos, startPos);
    }

    private void FireOnDragMove(int fingerId, Vector2 fingerPos, Vector2 delta)
    {
        if (OnDragMove != null)
            OnDragMove(fingerId, fingerPos, delta);
    }

    private void FireOnDragEnd(int fingerId, Vector2 fingerPos)
    {
        if (OnDragEnd != null)
            OnDragEnd(fingerId, fingerPos);
    }

    private void FireOnDragPaused(int fingerId, Vector2 fingerPos)
    {
        if (OnDragPaused != null)
            OnDragPaused(fingerId, fingerPos);
    }

    private void FireOnSwipe(int fingerId, Vector2 startPos, ESwipeDirection direction, float velocity)
    {
        if (OnSwipe != null)
            OnSwipe(fingerId, startPos, direction, velocity);
    }

    private void FireOnLeftSwipe(int fingerId, Vector2 startPos, ESwipeDirection direction, float velocity)
    {
        if (OnLeftSwipe != null)
            OnLeftSwipe(fingerId, startPos, direction, velocity);
    }

    private void FireOnRightSwipe(int fingerId, Vector2 startPos, ESwipeDirection direction, float velocity)
    {
        if (OnRightSwipe != null)
            OnRightSwipe(fingerId, startPos, direction, velocity);
    }

    private void FireOnMouseScroll(float delta)
    {
        if (OnMouseScroll != null)
            OnMouseScroll(delta);
    }

    private void FireOnBeginPinchSpread(int refTouchIndex, Vector2 anchorPoint)
    {
        if (OnBeginPinchSpread != null)
            OnBeginPinchSpread(refTouchIndex, anchorPoint);
    }

    private void FireOnPinchSpread(PinchSpreadData pinchSpreadData)
    {
        if (OnPinchSpread != null)
            OnPinchSpread(pinchSpreadData);
    }

    private void FireOnEndPinchSpread(int refTouchIndex)
    {
        if (OnEndPinchSpread != null)
            OnEndPinchSpread(refTouchIndex);
    }

    #endregion

    private void Awake()
    {
        _instance = this;

        Input.multiTouchEnabled = true;

        SceneManager.sceneLoaded += OnSceneLoaded;

        InitDistanceTresholds();

        InitInputDataDict();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        InitDistanceTresholds();
    }

    void InitDistanceTresholds()
    {
        if (Camera.main == null)
            return;
        
        float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
        
        _tapPixelTreshold = InputSettings.TapInchTreshold * Screen.dpi;
        _tapPixelTreshold *= _tapPixelTreshold;

        _initialDragPixelTreshold = InputSettings.InitialDragInchTreshold * Screen.dpi;
        _initialDragPixelTreshold *= _initialDragPixelTreshold;

        _perFrameDragPixelTreshold = InputSettings.PerFrameDragInchTreshold * Screen.dpi;
        _perFrameDragPixelTreshold *= _perFrameDragPixelTreshold;

        _swipePixelTreshold = InputSettings.SwipeInchTreshold * Screen.dpi;
        _swipePixelTreshold *= _swipePixelTreshold;

        _moveForSwipePixelTreshold = InputSettings.MoveForSwipeInchTreshold * Screen.dpi;
        _moveForSwipePixelTreshold *= _moveForSwipePixelTreshold;

        _moveFromStartPosPixelTreshold = InputSettings.MoveFromStartPosInchTreshold * Screen.dpi;
        _moveFromStartPosPixelTreshold *= _moveFromStartPosPixelTreshold;
    }

    void InitInputDataDict()
    {
        _pinchSpreadDataList = new List<PinchSpreadData>();

        _inputDataDict = new Dictionary<int, WorldInputTouch>();

        for (int i = 0; i < MaxInputCount; i++)
            _inputDataDict.Add(i, new WorldInputTouch());
    }

    void Update()
    {
        ProcessInputDatas();
    }

    void ProcessInputDatas()
    {
        for (int i = 0; i < _inputDataDict.Count; i++)
            ProcessInputData(_inputDataDict[i], i);
    }

    void ProcessInputData(WorldInputTouch touch, int index)
    {
        Vector3 inputPos = Vector3.zero;

        touch.PrevPoint = touch.TouchPoint;

        if (Utilities.IsTouchPlatform())
            ProcessTouchInput(touch, index);
        else
            ProcessMouseInput(touch, index);
    }

    void ProcessMouseInput(WorldInputTouch touch, int index)
    {
        if (CheckIfUIObjectIsInIgnore())
            return;

        Vector3 inputPos = Input.mousePosition;

        touch.TouchPoint = inputPos;

        if (!touch.IsTouchDown)
        {
            if (Input.GetMouseButtonDown(index))
                InputStarted(touch, index);
        }
        else
        {
            if (Input.GetMouseButton(index))
                InputContinued(touch, index);
            else if (Input.GetMouseButtonUp(index))
                InputEnded(touch, index);
        }

        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollAmount) > 0)
            FireOnMouseScroll(scrollAmount);
    }

    void ProcessTouchInput(WorldInputTouch touch, int index)
    {
        if (index >= Input.touchCount)
            return;

        Touch unityTouch = Input.GetTouch(index);

        if (CheckIfUIObjectIsInIgnore())
            return;

        Vector3 inputPos = (Vector3)unityTouch.position;

        touch.TouchPoint = inputPos;

        if (!touch.IsTouchDown)
        {
            if (unityTouch.phase == TouchPhase.Began)
                InputStarted(touch, index);
        }
        else
        {
            if (unityTouch.phase == TouchPhase.Moved
                || unityTouch.phase == TouchPhase.Stationary)
                InputContinued(touch, index);
            else if (unityTouch.phase == TouchPhase.Ended)
                InputEnded(touch, index);
        }
    }

    private bool CheckIfUIObjectIsInIgnore()
    {
        if (MMStandaloneInputModule.CurrentInput == null)
            return false;

        GameObject objectUnderPointer = MMStandaloneInputModule.CurrentInput.GameObjectUnderPointer();

        return ProcessUIObjForInput(objectUnderPointer);
    }

    private bool ProcessUIObjForInput(GameObject pressedObj)
    {
        if (pressedObj == null)
            return false;

        IInputBlocker targetMenu = pressedObj.GetComponentInParent<IInputBlocker>();
        if (targetMenu == null)
            return false;

        return true;
    }

    void InputStarted(WorldInputTouch touch, int index)
    {
        touch.IsTouchDown = true;
        touch.StartPoint = touch.TouchPoint;
        touch.StartTime = Time.time;
        touch.PrevPoint = touch.TouchPoint;
        touch.IsSwipePossible = true;
        touch.SwipeStartPoint = touch.TouchPoint;
        touch.SwipeDirection = ESwipeDirection.None;
        touch.IsMovedForDrag = false;
        touch.IsMovedFromStartPos = false;
        touch.IsMovedForSwipe = false;

        touch.TouchStartScreenArea = touch.StartPoint.x <= Screen.width / 2.0f ? ScreenArea.Left : ScreenArea.Right;

        FireOnFingerDown(index, touch.TouchPoint);

        switch (touch.TouchStartScreenArea)
        {
            case ScreenArea.Left:
                FireOnLeftFingerDown(index, touch.TouchPoint);
                break;
            case ScreenArea.Right:
                FireOnRightFingerDown(index, touch.TouchPoint);
                break;
        }
    }

    void InputContinued(WorldInputTouch touch, int index)
    {
        touch.DeltaMove = touch.TouchPoint - touch.PrevPoint;
        touch.TouchContinueScreenArea = touch.TouchPoint.x <= Screen.width / 2f ? ScreenArea.Left : ScreenArea.Right;

        if (Utilities.IsTouchPlatform())
            CheckPinchSpread(touch, index);

        CheckMovedFromStartPos(touch, index);
        CheckSwipe(touch, index);
        CheckDrag(touch, index);

        CheckPressed(touch, index);
    }

    void InputEnded(WorldInputTouch touch, int index)
    {
        PinchSpreadUpdate(index);

        if (touch.IsMovedForDrag)
            DragEnded(touch, index);

        CheckSwipe(touch, index);
        CheckDrag(touch, index);
        CheckTapped(touch, index);
        CheckFingerUpOnStartPos(touch, index);

        FireOnFingerUp(index, touch.TouchPoint);

        switch (touch.TouchStartScreenArea)
        {
            case ScreenArea.Left:
                FireOnLeftFingerUp(index, touch.TouchPoint);
                break;
            case ScreenArea.Right:
                FireOnRightFingerUp(index, touch.TouchPoint);
                break;
        }

        touch.IsTouchDown = false;
    }

    #region Drag Methods
    void CheckDrag(WorldInputTouch touch, int index)
    {
        if (!touch.IsMovedForDrag)
        {
            float deltaMove = Vector2.SqrMagnitude(touch.TouchPoint - touch.StartPoint);

            if (deltaMove >= _initialDragPixelTreshold)
                DragBegan(touch, index);
        }
        else if (touch.IsMovedForDrag
            && touch.DeltaMove.sqrMagnitude > _perFrameDragPixelTreshold)
            DragContinued(touch, index);
        else if (touch.IsMovedForDrag)
            DragPaused(touch, index);
    }

    void DragBegan(WorldInputTouch touch, int index)
    {
        touch.IsMovedForDrag = true;
        touch.LastMoveTime = Time.time;

        FireOnDragBegin(index, touch.TouchPoint, touch.StartPoint);
    }

    void DragContinued(WorldInputTouch touch, int index)
    {
        touch.LastMoveTime = Time.time;

        FireOnDragMove(index, touch.TouchPoint, touch.DeltaMove);
    }

    void DragEnded(WorldInputTouch touch, int index)
    {
        touch.IsMovedForDrag = false;

        FireOnDragEnd(index, touch.TouchPoint);
    }

    void DragPaused(WorldInputTouch touch, int index)
    {
        if (IsContinuousSwipeEnabled)
            touch.IsSwipePossible = true;

        FireOnDragPaused(index, touch.TouchPoint);
    }
    #endregion

    #region Swipe Methods
    void CheckSwipe(WorldInputTouch touch, int index)
    {
        if (touch.IsSwipePossible)
        {
            float initialDeltaMove = Vector2.SqrMagnitude(touch.TouchPoint - touch.SwipeStartPoint);

            if (!touch.IsMovedForSwipe
                && initialDeltaMove > _moveForSwipePixelTreshold)
            {
                PossibleSwipeBegan(touch, index);
            }
            else if (touch.IsMovedForSwipe &&
                (touch.SwipeDirection == ESwipeDirection.Right && touch.DeltaMove.x < 0
                || touch.SwipeDirection == ESwipeDirection.Left && touch.DeltaMove.x > 0
                || touch.SwipeDirection == ESwipeDirection.Up && touch.DeltaMove.y < 0
                || touch.SwipeDirection == ESwipeDirection.Down && touch.DeltaMove.y > 0))
            {

                touch.SwipeStartPoint = touch.TouchPoint;
                touch.IsMovedForSwipe = false;

                return;
            }
            else if (!touch.IsMovedForSwipe)
                return;

            float deltaMoveSqr = Vector2.SqrMagnitude(touch.TouchPoint - touch.SwipeStartPoint);

            if (deltaMoveSqr > _swipePixelTreshold)
            {
                float swipeVelocity = Mathf.Sqrt(deltaMoveSqr) / (Time.time - touch.StartTime);

                FireOnSwipe(index, touch.SwipeStartPoint, touch.SwipeDirection, swipeVelocity);

                touch.IsSwipePossible = false;

                switch (touch.TouchStartScreenArea)
                {
                    case ScreenArea.Left:
                        FireOnLeftSwipe(index, touch.SwipeStartPoint, touch.SwipeDirection, swipeVelocity);
                        break;
                    case ScreenArea.Right:
                        FireOnRightSwipe(index, touch.SwipeStartPoint, touch.SwipeDirection, swipeVelocity);
                        break;
                }
            }
        }
    }

    void PossibleSwipeBegan(WorldInputTouch touch, int index)
    {
        touch.IsSwipePossible = true;
        touch.IsMovedForSwipe = true;

        touch.SwipeDirection = GetSwipeDirection(touch);
    }

    ESwipeDirection GetSwipeDirection(WorldInputTouch touch)
    {
        Vector2 totalDelta = touch.TouchPoint - touch.SwipeStartPoint;

        float aTan = Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(totalDelta.y), Mathf.Abs(totalDelta.x));

        if (aTan < InputSettings.SwipeHorAngleTrashold)
        {
            if (Mathf.Abs(totalDelta.x) <= Mathf.Epsilon)
                return ESwipeDirection.None;
            else if (totalDelta.x > 0)
                return ESwipeDirection.Right;
            else
                return ESwipeDirection.Left;
        }
        else
        {
            if (Mathf.Abs(totalDelta.y) <= Mathf.Epsilon)
                return ESwipeDirection.None;
            else if (totalDelta.y > 0)
                return ESwipeDirection.Up;
            else
                return ESwipeDirection.Down;
        }
    }
    #endregion

    void CheckMovedFromStartPos(WorldInputTouch touch, int index)
    {
        float deltaMoveSqr = Vector2.SqrMagnitude(touch.TouchPoint - touch.StartPoint);

        if (!touch.IsMovedFromStartPos
            && deltaMoveSqr > _moveFromStartPosPixelTreshold)
        {
            touch.IsMovedFromStartPos = true;

            FireOnFingerMovedFromStartPos(index, touch.TouchPoint);
        }

    }

    void CheckFingerUpOnStartPos(WorldInputTouch touch, int index)
    {
        float deltaMoveSqr = Vector2.SqrMagnitude(touch.TouchPoint - touch.StartPoint);

        if (!touch.IsMovedFromStartPos
            && deltaMoveSqr <= _moveFromStartPosPixelTreshold)
            FireOnFingerUpOnStartPos(index, touch.TouchPoint);
        else
            FireOnFingerUpNotOnStartPos(index, touch.TouchPoint);
    }

    void CheckTapped(WorldInputTouch touch, int index)
    {
        float deltaMoveSqr = Vector2.SqrMagnitude(touch.TouchPoint - touch.StartPoint);

        if (Time.time - touch.StartTime > InputSettings.TapDuration
            || deltaMoveSqr > _tapPixelTreshold)
            return;

        FireOnTap(index, touch.TouchPoint);

        switch (touch.TouchStartScreenArea)
        {
            case ScreenArea.Left:
                FireOnLeftTap(index, touch.TouchPoint);
                break;
            case ScreenArea.Right:
                FireOnRightTap(index, touch.TouchPoint);
                break;
        }
    }

    void CheckPressed(WorldInputTouch touch, int index)
    {
        float duration = Time.time - touch.StartTime;

        FireOnPress(index, touch.TouchPoint, duration);

        switch (touch.TouchStartScreenArea)
        {
            case ScreenArea.Left:
                FireOnLeftPress(index, touch.TouchPoint, duration);
                break;
            case ScreenArea.Right:
                FireOnRightPress(index, touch.TouchPoint, duration);
                break;
        }
    }

    void PinchSpreadUpdate(int index)
    {
        int removedEleCount = _pinchSpreadDataList.RemoveAll(psd => psd.RefFingerID == index);

        if(removedEleCount > 0)
            FireOnEndPinchSpread(index);
    }

    void CheckPinchSpread(WorldInputTouch touch, int index)
    {
        var pinchSpreadData = GetPinchSpreadData(index);
        if (pinchSpreadData == null)
            return;

        var refInputData = _inputDataDict[pinchSpreadData.RefFingerID];
        var inputData = _inputDataDict[index];

        pinchSpreadData.UpdateRefInput(refInputData.PrevPoint, refInputData.TouchPoint);
        pinchSpreadData.UpdateInput(inputData.PrevPoint, inputData.TouchPoint);
        
        pinchSpreadData.CalculatePinchSpreadAnchor(refInputData.TouchPoint, inputData.TouchPoint);

        FireOnPinchSpread(pinchSpreadData);
    }

    private PinchSpreadData GetPinchSpreadData(int index)
    {
        var refTouch = Input.touches[0];

        if (index == refTouch.fingerId || refTouch.phase == TouchPhase.Ended)
            return null;

        PinchSpreadData data = _pinchSpreadDataList.Find(psd => psd.FingerID == index && psd.RefFingerID == refTouch.fingerId);
        if (data == null)
        {
            data = new PinchSpreadData()
            {
                FingerID = index,
                RefFingerID = refTouch.fingerId,
            };

            data.CalculatePinchSpreadAnchor(_inputDataDict[refTouch.fingerId].TouchPoint, _inputDataDict[index].TouchPoint);

            _pinchSpreadDataList.Add(data);

            FireOnBeginPinchSpread(data.RefFingerID, data.PinchSpreadAnchorInViewport);
        }

        return data;
    }
}

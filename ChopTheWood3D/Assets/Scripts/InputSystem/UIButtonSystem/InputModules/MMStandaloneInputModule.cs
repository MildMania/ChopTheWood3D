using System;
using MMUISystem.UIButton;
using UnityEngine;
using UnityEngine.EventSystems;

public class MMStandaloneInputModule : PointerInputModule
{
    private float m_NextAction;

    private Vector2 m_LastMousePosition;
    private Vector2 m_MousePosition;

    [SerializeField]
    private string m_HorizontalAxis = "Horizontal";

    /// <summary>
    /// Name of the vertical axis for movement (if axis events are used).
    /// </summary>
    [SerializeField]
    private string m_VerticalAxis = "Vertical";

    /// <summary>
    /// Name of the submit button.
    /// </summary>
    [SerializeField]
    private string m_SubmitButton = "Submit";

    /// <summary>
    /// Name of the submit button.
    /// </summary>
    [SerializeField]
    private string m_CancelButton = "Cancel";

    [SerializeField]
    private float m_InputActionsPerSecond = 10;

    [SerializeField]
    private bool m_AllowActivationOnMobileDevice;

    public bool allowActivationOnMobileDevice
    {
        get { return m_AllowActivationOnMobileDevice; }
        set { m_AllowActivationOnMobileDevice = value; }
    }

    public float inputActionsPerSecond
    {
        get { return m_InputActionsPerSecond; }
        set { m_InputActionsPerSecond = value; }
    }

    /// <summary>
    /// Name of the horizontal axis for movement (if axis events are used).
    /// </summary>
    public string horizontalAxis
    {
        get { return m_HorizontalAxis; }
        set { m_HorizontalAxis = value; }
    }

    /// <summary>
    /// Name of the vertical axis for movement (if axis events are used).
    /// </summary>
    public string verticalAxis
    {
        get { return m_VerticalAxis; }
        set { m_VerticalAxis = value; }
    }

    public string submitButton
    {
        get { return m_SubmitButton; }
        set { m_SubmitButton = value; }
    }

    public string cancelButton
    {
        get { return m_CancelButton; }
        set { m_CancelButton = value; }
    }

    public override void UpdateModule()
    {
        m_LastMousePosition = m_MousePosition;
        m_MousePosition = Input.mousePosition;
    }

    public override bool IsModuleSupported()
    {
        // Check for mouse presence instead of whether touch is supported,
        // as you can connect mouse to a tablet and in that case we'd want
        // to use StandaloneInputModule for non-touch input events.
        return m_AllowActivationOnMobileDevice || Input.mousePresent;
    }

    //public bool AllowMouseAsTouch;

    #region Module Extension
    private static MMStandaloneInputModule _currentInput;
    public static MMStandaloneInputModule CurrentInput
    {
        get
        {
            if (_currentInput == null)
                _currentInput = EventSystem.current.currentInputModule as MMStandaloneInputModule;

            return _currentInput;
        }
    }

    public GameObject GameObjectUnderPointer(int pointerID)
    {
        PointerEventData lastPointer = GetLastPointerEventData(pointerID);

        if (lastPointer != null)
            return lastPointer.pointerCurrentRaycast.gameObject;

        return null;
    }

    public GameObject GameObjectUnderPointer()
    {
        return GameObjectUnderPointer(kMouseLeftId);
    }
    #endregion

    protected MMStandaloneInputModule()
    {
    }

    protected override void Awake()
    {
        //Input.simulateMouseWithTouches = AllowMouseAsTouch;
    }

    public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
            return false;

        var shouldActivate = Input.GetButtonDown(m_SubmitButton);
        shouldActivate |= Input.GetButtonDown(m_CancelButton);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(m_HorizontalAxis), 0.0f);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(m_VerticalAxis), 0.0f);
        shouldActivate |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
        shouldActivate |= Input.GetMouseButtonDown(0);
        return shouldActivate;
    }

    public override void ActivateModule()
    {
        base.ActivateModule();

        m_MousePosition = Input.mousePosition;
        m_LastMousePosition = Input.mousePosition;

        var toSelect = eventSystem.currentSelectedGameObject;
        if (toSelect == null)
            toSelect = eventSystem.firstSelectedGameObject;

        eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();

        ClearSelection();
    }

    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();

            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        MMProcessMouseEvent();

        ProcessTouches();
    }

    #region Touch Events - Overriden by MM events
    private void ProcessTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
            ProcessTouches(Input.GetTouch(i));
    }

    private void ProcessTouches(Touch touch)
    {
        PointerEventData eventData = GetTouchPointerEventData(touch, out bool pressed, out bool released);

        MMProcessTouchPress(eventData, pressed, released);
        MMProcessTouchDrag(eventData);
    }

    private void MMProcessTouchPress(PointerEventData pointerEvent, bool pressedThisFrame, bool releasedThisFrame)
    {
        var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

        // PointerDown notification
        if (pressedThisFrame)
        {
            pointerEvent.eligibleForClick = true;
            pointerEvent.delta = Vector2.zero;
            pointerEvent.dragging = false;
            pointerEvent.useDragThreshold = true;
            pointerEvent.pressPosition = pointerEvent.position;
            pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

            DeselectIfSelectionChanged(currentOverGo, pointerEvent);

            // search for the control that will receive the press
            // if we can't find a press handler set the press
            // handler to be what would receive a click.
            var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

            ExecuteEvents.ExecuteHierarchy<IUIInputPointerDown>(currentOverGo, pointerEvent, (x, y) => x.OnPointerDown(new Input_UI_OnPointerDown(pointerEvent)));

            // didnt find a press handler... search for a click handler
            if (newPressed == null)
                newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            float time = Time.unscaledTime;

            if (newPressed == pointerEvent.lastPress)
            {
                var diffTime = time - pointerEvent.clickTime;
                if (diffTime < 0.3f)
                    ++pointerEvent.clickCount;
                else
                    pointerEvent.clickCount = 1;

                pointerEvent.clickTime = time;
            }
            else
            {
                pointerEvent.clickCount = 1;
            }

            pointerEvent.pointerPress = newPressed;
            pointerEvent.rawPointerPress = currentOverGo;

            pointerEvent.clickTime = time;

            // Save the drag handler as well
            pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IUIInputDrag>(currentOverGo);

            if (pointerEvent.pointerDrag == null || pointerEvent.pointerDrag.GetComponent<UnityUIDraggableButton>() == null)
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

            if (pointerEvent.pointerDrag != null)
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
        }

        // PointerUp notification
        if (releasedThisFrame)
        {
            // Debug.Log("Executing pressup on: " + pointer.pointerPress);
            ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

            ExecuteEvents.Execute<IUIInputPointerUp>(pointerEvent.pointerPress, pointerEvent, (x, y) => x.OnPointerUp(new Input_UI_OnPointerUp(pointerEvent)));

            // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

            // see if we mouse up on the same element that we clicked on...
            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            // PointerClick and Drop events
            if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
            }
            else if (pointerEvent.pointerDrag != null)
            {
                ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
            }

            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = null;
            pointerEvent.rawPointerPress = null;

            if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
            {
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
                ExecuteEvents.Execute<IUIInputEndDrag>(pointerEvent.pointerDrag, pointerEvent, (x, y) => x.OnEndDrag(new Input_UI_OnEndDrag(pointerEvent)));
            }

            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = null;

            // redo pointer enter / exit to refresh state
            // so that if we moused over somethign that ignored it before
            // due to having pressed on something else
            // it now gets it.
            if (currentOverGo != pointerEvent.pointerEnter)
            {
                HandlePointerExitAndEnter(pointerEvent, null);
                HandlePointerExitAndEnter(pointerEvent, currentOverGo);
            }
        }
    }

    private void MMProcessTouchDrag(PointerEventData pointerEvent)
    {
        bool moving = pointerEvent.IsPointerMoving();

        if (moving && pointerEvent.pointerDrag != null
            && !pointerEvent.dragging
            && MMShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
        {
            ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute<IUIInputBeginDrag>(pointerEvent.pointerDrag, pointerEvent, (x, y) => x.OnBeginDrag(new Input_UI_OnBeginDrag(pointerEvent)));

            pointerEvent.dragging = true;
        }

        // Drag notification
        if (pointerEvent.dragging && moving && pointerEvent.pointerDrag != null)
        {
            // Before doing drag we should cancel any pointer down state
            // And clear selection!
            if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                ExecuteEvents.Execute<IUIInputPointerUp>(pointerEvent.pointerPress, pointerEvent, (x, y) => x.OnPointerUp(new Input_UI_OnPointerUp(pointerEvent)));

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;
            }

            ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            ExecuteEvents.Execute<IUIInputDrag>(pointerEvent.pointerDrag, pointerEvent, (x, y) => x.OnDrag(new Input_UI_OnDrag(pointerEvent)));
        }
    }
    #endregion

    #region Keys and Keyboard Events - Original from https://github.com/tenpn/unity3d-ui/blob/master/UnityEngine.UI/EventSystem/InputModules/StandaloneInputModule.cs
    /// <summary>
    /// Process submit keys.
    /// </summary>
    private bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (Input.GetButtonDown(m_SubmitButton))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        if (Input.GetButtonDown(m_CancelButton))
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }

    /// <summary>
    /// Process keyboard events.
    /// </summary>
    private bool SendMoveEventToSelectedObject()
    {
        float time = Time.unscaledTime;

        if (!AllowMoveEventProcessing(time))
            return false;

        Vector2 movement = GetRawMoveVector();

        var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
        if (!Mathf.Approximately(axisEventData.moveVector.x, 0f)
            || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        }
        m_NextAction = time + 1f / m_InputActionsPerSecond;
        return axisEventData.used;
    }

    private bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }

    private bool AllowMoveEventProcessing(float time)
    {
        bool allow = Input.GetButtonDown(m_HorizontalAxis);
        allow |= Input.GetButtonDown(m_VerticalAxis);
        allow |= (time > m_NextAction);
        return allow;
    }

    private Vector2 GetRawMoveVector()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw(m_HorizontalAxis);
        move.y = Input.GetAxisRaw(m_VerticalAxis);

        if (Input.GetButtonDown(m_HorizontalAxis))
        {
            if (move.x < 0)
                move.x = -1f;
            if (move.x > 0)
                move.x = 1f;
        }
        if (Input.GetButtonDown(m_VerticalAxis))
        {
            if (move.y < 0)
                move.y = -1f;
            if (move.y > 0)
                move.y = 1f;
        }
        return move;
    }
    #endregion

    #region Mouse Events - Overriden by MM events
    private void MMProcessMouseEvent()
    {
        MouseState mouseData = GetMousePointerEventData();

        bool pressed = mouseData.AnyPressesThisFrame();
        bool released = mouseData.AnyReleasesThisFrame();

        MouseButtonEventData leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;

        if (!MMUseMouse(pressed, released, leftButtonData.buttonData))
            return;

        // Process the first mouse button fully
        MMProcessMousePress(leftButtonData);
        MMProcessMove(leftButtonData.buttonData);
        MMProcessDrag(leftButtonData.buttonData);

        // Now process right / middle clicks
        MMProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
        MMProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
        MMProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
        MMProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);

        if (!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
        {
            var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
            ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
        }
    }

    private static bool MMUseMouse(bool pressed, bool released, PointerEventData pointerData)
    {
        if (pressed || released || pointerData.IsPointerMoving() || pointerData.IsScrolling())
            return true;

        return false;
    }

    private void MMProcessMousePress(MouseButtonEventData data)
    {
        var pointerEvent = data.buttonData;
        var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

        // PointerDown notification
        if (data.PressedThisFrame())
        {
            pointerEvent.eligibleForClick = true;
            pointerEvent.delta = Vector2.zero;
            pointerEvent.dragging = false;
            pointerEvent.useDragThreshold = true;
            pointerEvent.pressPosition = pointerEvent.position;
            pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

            DeselectIfSelectionChanged(currentOverGo, pointerEvent);

            // search for the control that will receive the press
            // if we can't find a press handler set the press
            // handler to be what would receive a click.
            var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

            ExecuteEvents.ExecuteHierarchy<IUIInputPointerDown>(currentOverGo, pointerEvent, (x, y) => x.OnPointerDown(new Input_UI_OnPointerDown(pointerEvent)));

            // didnt find a press handler... search for a click handler
            if (newPressed == null)
                newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            float time = Time.unscaledTime;

            if (newPressed == pointerEvent.lastPress)
            {
                var diffTime = time - pointerEvent.clickTime;
                if (diffTime < 0.3f)
                    ++pointerEvent.clickCount;
                else
                    pointerEvent.clickCount = 1;

                pointerEvent.clickTime = time;
            }
            else
            {
                pointerEvent.clickCount = 1;
            }

            pointerEvent.pointerPress = newPressed;
            pointerEvent.rawPointerPress = currentOverGo;

            pointerEvent.clickTime = time;

            // Save the drag handler as well
            pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IUIInputDrag>(currentOverGo);

            if (pointerEvent.pointerDrag == null || pointerEvent.pointerDrag.GetComponent<UnityUIDraggableButton>() == null)
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

            if (pointerEvent.pointerDrag != null)
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
        }

        // PointerUp notification
        if (data.ReleasedThisFrame())
        {
            // Debug.Log("Executing pressup on: " + pointer.pointerPress);
            ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

            ExecuteEvents.Execute<IUIInputPointerUp>(pointerEvent.pointerPress, pointerEvent, (x, y) => x.OnPointerUp(new Input_UI_OnPointerUp(pointerEvent)));

            // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

            // see if we mouse up on the same element that we clicked on...
            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            // PointerClick and Drop events
            if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
            }
            else if (pointerEvent.pointerDrag != null)
            {
                ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
            }

            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = null;
            pointerEvent.rawPointerPress = null;

            if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
            {
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
                ExecuteEvents.Execute<IUIInputEndDrag>(pointerEvent.pointerDrag, pointerEvent, (x, y) => x.OnEndDrag(new Input_UI_OnEndDrag(pointerEvent)));
            }

            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = null;

            // redo pointer enter / exit to refresh state
            // so that if we moused over somethign that ignored it before
            // due to having pressed on something else
            // it now gets it.
            if (currentOverGo != pointerEvent.pointerEnter)
            {
                HandlePointerExitAndEnter(pointerEvent, null);
                HandlePointerExitAndEnter(pointerEvent, currentOverGo);
            }
        }
    }

    protected virtual void MMProcessMove(PointerEventData pointerEvent)
    {
        var targetGO = pointerEvent.pointerCurrentRaycast.gameObject;
        HandlePointerExitAndEnter(pointerEvent, targetGO);
    }

    protected virtual void MMProcessDrag(PointerEventData pointerEvent)
    {
        bool moving = pointerEvent.IsPointerMoving();

        if (moving && pointerEvent.pointerDrag != null
            && !pointerEvent.dragging
            && MMShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
        {
            ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute<IUIInputBeginDrag>(pointerEvent.pointerDrag, pointerEvent, (x, y) => x.OnBeginDrag(new Input_UI_OnBeginDrag(pointerEvent)));

            pointerEvent.dragging = true;
        }

        // Drag notification
        if (pointerEvent.dragging && moving && pointerEvent.pointerDrag != null)
        {
            // Before doing drag we should cancel any pointer down state
            // And clear selection!
            if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                ExecuteEvents.Execute<IUIInputPointerUp>(pointerEvent.pointerPress, pointerEvent, (x, y) => x.OnPointerUp(new Input_UI_OnPointerUp(pointerEvent)));

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;
            }

            ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            ExecuteEvents.Execute<IUIInputDrag>(pointerEvent.pointerDrag, pointerEvent, (x, y) => x.OnDrag(new Input_UI_OnDrag(pointerEvent)));
        }
    }

    private static bool MMShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
    {
        if (!useDragThreshold)
            return true;

        return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
    }
    #endregion
}

using System;
using System.Linq;
using System.Collections.Generic;

public abstract class InputTransmitter : IDisposable
{
    public delegate void EventDelegate<T>(T e) where T : InputEvent;

    public delegate void EventDelegate(InputEvent e);

    public bool IsTransmissionEnabled { get; private set; }

    protected Stack<bool> _transmissionStateStack;

    IInputReceiver _receiver;

    public InputControllerBase InputController
    {
        get
        {
            return GetInputController();
        }
    }

    public static T AttachInputTransmitter<T>(IInputReceiver r, bool isTransmissionEnabled = true)
        where T : InputTransmitter, new()
    {
        if (r.AttachedInputTransmitterList != null
            && r.AttachedInputTransmitterList.Any(val => val is T))
            return (T)r.AttachedInputTransmitterList.SingleOrDefault(val => val is T);

        T it = new T()
        {
            _receiver = r
        };

        it._receiver.AttachInputTransmitter(it);
        it.IsTransmissionEnabled = isTransmissionEnabled;

        return it;
    }

    public static void DeattachInputTransmitter<T>(IInputReceiver r)
        where T : InputTransmitter, new()
    {
        T transmitter = (T)r.AttachedInputTransmitterList.SingleOrDefault(val => val is T);

        if (transmitter == default(T))
            return;

        r.DeattachInputTransmitter(transmitter);
    }

    protected InputTransmitter()
    {
        InitStateStack();

        RegisterToInputControllerEvents();
    }

    void InitStateStack()
    {
        _transmissionStateStack = new Stack<bool>();
    }

    public void Dispose()
    {
        UnregisterToInputControllerEvents();
    }

    public void SetTransmissionEnabled(bool isEnabled, bool cacheCurState = true)
    {
        if(cacheCurState)
            _transmissionStateStack.Push(IsTransmissionEnabled);

        IsTransmissionEnabled = isEnabled;
    }

    public void SetTransmissionStateToPrevState()
    {
        if (_transmissionStateStack.Count == 0)
            return;

        IsTransmissionEnabled = _transmissionStateStack.Pop();
    }

    void OnTransmissionEnabled()
    {
        IsTransmissionEnabled = true;
    }

    void OnTransmissionDisabled()
    {
        IsTransmissionEnabled = false;
    }

    void OnPushStateRequestReceived()
    {
        _transmissionStateStack.Push(IsTransmissionEnabled);
    }

    void OnPopStateRequestReceived()
    {
        if (_transmissionStateStack.Count == 0)
            return;

        IsTransmissionEnabled = _transmissionStateStack.Pop();
    }

    protected abstract void RegisterToInputControllerEvents();
    protected abstract void UnregisterToInputControllerEvents();

    protected void Raise(Type type, InputEvent e)
    {
        /*if (InputController is WorldHighlighterInputController)
            UnityEngine.Debug.Log("is highlighter enabled: " + InputController.IsInputEnabled);*/

        if (!IsTransmissionEnabled
            || InputController == null
            || !InputController.IsInputEnabled)
            return;

        EventDelegate del;

        if (_receiver.Delegates == null)
            return;

        if (_receiver.Delegates.TryGetValue(type, out del))
        {
            del.Invoke(e);
        }
    }

    protected abstract InputControllerBase GetInputController();
}

using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class InputControllerBase : IDisposable
{
    Stack<bool> _transmissionStack;
    Stack<bool> _TransmissionStack
    {
        get
        {
            if (_transmissionStack == null)
                _transmissionStack = new Stack<bool>();

            return _transmissionStack;
        }
    }

    protected bool _isInputEnabled;

    public bool IsInputEnabled
    {
        get
        {
            return _isInputEnabled;
        }
    }

    bool _isInputControllerInited;

    protected InputControllerBase()
    {
        EnableInputTransmission();
    }

    public void EnableInputTransmission()
    {
        _TransmissionStack.Push(IsInputEnabled);

        _isInputEnabled = true;
    }

    public void DisableInputTransmission()
    {
        _TransmissionStack.Push(IsInputEnabled);

        _isInputEnabled = false;
    }

    public void RestoreInputTransmission()
    {
        if (_TransmissionStack.Count == 0)
        {
            Debug.Log("couldn't restore transmission");

            return;
        }

        _isInputEnabled = _TransmissionStack.Pop();
    }

    protected void ResetTransmissionStack()
    {
        _TransmissionStack.Clear();
    }

    #region IDisposable Support
    private bool disposedValue = false;

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                DisposeCustomActions();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void DisposeCustomActions()
    {
        InputControllerManager.Instance.UnregisterInputController(this);
    }
    #endregion
}

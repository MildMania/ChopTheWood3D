using System;

public class InitializableBase : IDisposable
{
    bool _isInited;

    public bool IsInited
    {
        get
        {
            return _isInited;
        }
        set
        {
            _isInited = value;

            if (_isInited)
                FireOnInited();
        }
    }

    Action _onInited;

    void FireOnInited()
    {
        if (_onInited != null)
            _onInited();

        ResetOnInitCallbacks();
    }

    public void WaitForInit(Action callback)
    {
        if (_isInited)
            return;

        _onInited += callback;
    }

    void ResetOnInitCallbacks()
    {
        if (_onInited == null)
            return;

        foreach (Action action in _onInited.GetInvocationList())
        {
            _onInited -= action;
        }
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                _onInited = null;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~InitializableBase() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }
    #endregion


}

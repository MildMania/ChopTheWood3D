using System;

public abstract class VMBase : IDisposable
{
    private bool disposedValue = false;

    public Action OnDataReceived;
    protected void FireOnDataReceived()
    {
        if (OnDataReceived != null)
            OnDataReceived();
    }

    public virtual void InitViewModel()
    {
        FireOnDataReceived();
    }

    public abstract void StartListeningEvents();
    public abstract void FinishListeningEvents();

    protected virtual void Dispose(bool disposing)
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

    protected abstract void DisposeCustomActions();
}

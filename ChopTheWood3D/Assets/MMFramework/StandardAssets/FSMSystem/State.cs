using System;
using UnityEngine;

public abstract class TransitionMessage
{

}

public abstract class State<T1, T2, T3> : MonoBehaviour, IState<T1, T2>
    where T1 : IConvertible
    where T2 : IConvertible
    where T3 : TransitionMessage
{
    public IMMFSM<T1, T2> FSM { get; set; }

    public T1 StateID { get { return GetStateID(); } }

    [SerializeField]
    protected bool _exitImmediately = true;

    protected T3 _m;

    public bool IsExiting { get; private set; }

    public bool IsInState { get; private set; }

    #region Events
    public Action OnExitCompleted { get; set; }

    void FireOnExitCompleted()
    {
        if (OnExitCompleted != null)
            OnExitCompleted();
    }
    #endregion

    public void InitState(IMMFSM<T1, T2> fsm)
    {
        FSM = fsm;
        InitStateCustomActions();
    }

    protected virtual void InitStateCustomActions()
    {

    }

    protected abstract T1 GetStateID();

    public void OnEnter(TransitionMessage m)
    {
        if (m != null
            && !(m is T3))
            throw new Exception("Invalid Message!");

        _m = (T3)m;

        IsInState = true;

        OnEnterCustomActions();
    }

    public virtual bool CanEnter(TransitionMessage m)
    {
        return true;
    }

    public virtual void OnEnterCustomActions()
    {

    }

    public void OnExit()
    {
        IsExiting = true;

        OnExitCustomActions();

        if (_exitImmediately)
            ExitCompleted();
    }

    protected virtual void OnExitCustomActions()
    {

    }

    protected void ExitCompleted()
    {
        IsExiting = false;

        IsInState = false;

        FireOnExitCompleted();
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                DisposeCustomActions(disposing);
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    protected virtual void DisposeCustomActions(bool disposing)
    {

    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~State() {
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

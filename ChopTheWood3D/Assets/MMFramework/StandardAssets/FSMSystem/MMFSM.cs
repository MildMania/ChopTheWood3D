using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class MMFSM<T1, T2> : MonoBehaviour, IMMFSM<T1, T2>
    where T1 : IConvertible
    where T2 : IConvertible
{
    public IFSMController<T1, T2> FSMController { get; private set; }

    protected List<IState<T1, T2>> _stateList;
    public List<IState<T1, T2>> StateList
    {
        get
        {
            if (_stateList == null)
                _stateList = GetComponentsInChildren<IState<T1, T2>>().ToList();

            return _stateList;
        }
    }

    Dictionary<StateTransition<T1, T2>, T1> _transitionList;
    public Dictionary<StateTransition<T1, T2>, T1> TransitionList
    {
        get
        {
            if (_transitionList == null)
                _transitionList = GetTransitionDict();

            return _transitionList;
        }
    }

    public IState<T1, T2> EnteranceState { get; protected set; }

    public IState<T1, T2> CurState { get; private set; }
    public IState<T1, T2> PrevState { get; private set; }

    IState<T1, T2> _nextState;
    TransitionMessage _nextMessage;

    TransitionMessage _prevMessage;
    TransitionMessage _popMessage;

    #region Events

    Action<T1> _onStateEntered;

    public void AddOnStateEntered(Action<T1> callback)
    {
        _onStateEntered += callback;
    }

    public void RemoveOnStateEntered(Action<T1> callback)
    {
        _onStateEntered -= callback;
    }

    void FireOnStateEntered(T1 stateID)
    {
        if (_onStateEntered != null)
            _onStateEntered(stateID);
    }

    Action<T1> _onStateExited;

    public void AddOnStateExited(Action<T1> callback)
    {
        _onStateExited += callback;
    }

    public void RemoveOnStateExited(Action<T1> callback)
    {
        _onStateExited -= callback;
    }

    void FireOnStateExited(T1 stateID)
    {
        if (_onStateExited != null)
            _onStateExited(stateID);
    }
    #endregion

    public void InitFSM(IFSMController<T1, T2> fsmController)
    {
        FSMController = fsmController;

        foreach (IState<T1, T2> state in StateList)
            state.InitState(this);
    }

    protected abstract Dictionary<StateTransition<T1, T2>, T1> GetTransitionDict();

    public void EnterFSM()
    {
        if (CurState != null)
            CurState.OnExit();

        PrevState = CurState;

        if (EnteranceState == null)
            EnteranceState = GetState(GetEnteranceStateID());

        EnterState(EnteranceState, null);
    }

    protected abstract T1 GetEnteranceStateID();

    public void ExitFSM()
    {
        if (CurState == null)
            return;

        _nextState = null;

        ExitState();
    }

    public void ForceEnterState(T1 state, TransitionMessage m)
    {
        if (CurState != null)
            CurState.OnExit();

        PrevState = CurState;

        EnterState(GetState(state), m);
    }

    public bool SetTransition(T2 t, TransitionMessage m)
    {
        _nextState = GetNextState(t);

        if (_nextState == null)
            return false;

        if (!_nextState.CanEnter(m))
            return false;

        _nextMessage = m;

        ExitState();

        return true;
    }

    void EnterState(IState<T1, T2> state, TransitionMessage m)
    {
        _prevMessage = m;

        CurState = state;

        FireOnStateEntered(CurState.StateID);

        CurState.OnEnter(m);
    }

    void ExitState()
    {
        if (CurState != null)
        {
            CurState.OnExitCompleted += OnCurStateExitCompleted;

            CurState.OnExit();
        }
    }

    void OnCurStateExitCompleted()
    {
        CurState.OnExitCompleted -= OnCurStateExitCompleted;

        PrevState = CurState;

        FireOnStateExited(PrevState.StateID);

        if (_nextState == null)
            return;

        EnterState(_nextState, _nextMessage);
    }

    public bool PushState(T1 t, TransitionMessage m)
    {
        T2 transition = GetTransitionFromTo(CurState.StateID, t);

        _popMessage = _prevMessage;

        return SetTransition(transition, m);
    }

    public bool PopState()
    {
        T2 transition = GetTransitionFromTo(CurState.StateID, PrevState.StateID);

        return SetTransition(transition, _popMessage);
    }

    protected IState<T1, T2> GetNextState(T2 t)
    {
        StateTransition<T1, T2> transition = new StateTransition<T1, T2>(CurState.StateID, t);

        try
        {
            T1 nextState = TransitionList[transition];

            for (int i = 0; i < StateList.Count; i++)
            {
                if (StateList[i].StateID.Equals(nextState))
                    return StateList[i];
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    public bool IsValidTransition(T2 t)
    {
        if (!t.GetType().Equals(typeof(T2)))
        {
            throw new Exception("Not a valid transition type: " + t.GetType());
        }

        StateTransition<T1, T2> transition = new StateTransition<T1, T2>(CurState.StateID, (T2)t);

        T1 nextState;

        if(!TransitionList.TryGetValue(transition, out nextState))
            return false;

        return true;
    }

    public bool HasTransitionFromTo(T1 to)
    {
        if (!to.GetType().Equals(typeof(T1)))
        {
            throw new Exception("Not a valid state: " + to.GetType());
        }

        return TransitionList.Any(val => val.Key.CurState.Equals(CurState.StateID) && val.Value.Equals((T1)to));

    }

    protected T2 GetTransitionFromTo(T1 from, T1 to)
    {
        if (!from.GetType().Equals(typeof(T1))
            || !to.GetType().Equals(typeof(T1)))
        {
            throw new Exception("Not a valid state: " + from.GetType());
        }

        try
        {
            return TransitionList.Single(val => val.Key.CurState.Equals((T1)from) && val.Value.Equals(to)).Key.Transition;
        }
        catch
        {
            throw new Exception("There is no transition from: " + from.ToString() + " to: " + to.ToString());
        }

    }

    public T1 GetCurStateID()
    {
        return CurState.StateID;
    }

    public IState<T1, T2> GetState(T1 stateID)
    {
        for (int i = 0; i < StateList.Count; i++)
        {
            if (StateList[i].StateID.Equals(stateID))
                return StateList[i];
        }

        return null;
    }


    public TState GetState<TState>()
        where TState : IState<T1, T2>
    {
        for(int i = 0; i < StateList.Count; i++)
        {
            if (StateList[i] is TState)
                return (TState)StateList[i];
        }

        return default(TState);
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
                foreach (IState<T1, T2> state in StateList)
                    state.Dispose();

                FSMController = null;
                _stateList = null;
                _transitionList = null;
                CurState = null;
                PrevState = null;

                _nextState = null;
                _nextMessage = null;

                _prevMessage = null;
                _popMessage = null;

                _onStateEntered = null;
                _onStateExited = null;

            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~MMFSM() {
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

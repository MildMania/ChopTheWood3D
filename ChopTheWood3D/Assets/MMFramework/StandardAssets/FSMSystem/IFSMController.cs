using System;

public interface IFSMController
{
    void InitFSMs();
    void EnterFSMs();
    void ExitFSMs();
}

public interface IFSMController<T1, T2> : IFSMController
    where T1 : IConvertible
    where T2 : IConvertible
{
    void AddOnStateEnteredListener<TFSM>(Action<T1> callback) where TFSM : IMMFSM<T1, T2>;
    void RemoveOnStateEnteredListener<TFSM>(Action<T1> callback) where TFSM : IMMFSM<T1, T2>;
    void AddOnStateExitedListener<TFSM>(Action<T1> callback) where TFSM : IMMFSM<T1, T2>;
    void RemoveOnStateExitedListener<TFSM>(Action<T1> callback) where TFSM : IMMFSM<T1, T2>;

    void ForceEnterState(T1 state, TransitionMessage m = null);
    bool SetTransition(T2 t, TransitionMessage m = null);
    bool PushState(T1 t, TransitionMessage m);
    bool PopState();
    bool IsValidTransition(T2 t);
    IState<T1, T2> GetState(T1 stateID);
    TState GetState<TState>() where TState : IState<T1, T2>;
    T1 GetCurStateIDOfFSM<TFSM>() where TFSM : IMMFSM<T1, T2>;
    TFSM GetFSM<TFSM>() where TFSM : IMMFSM<T1, T2>;
}

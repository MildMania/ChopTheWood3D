using System;

public interface IMMFSM
{

}

/// <summary>
/// </summary>
/// <typeparam name="T1">StateEnum</typeparam>
/// <typeparam name="T2">TransitionEnum</typeparam>
public interface IMMFSM<T1, T2> : IMMFSM, IDisposable
    where T1 : IConvertible
    where T2 : IConvertible
{
    IFSMController<T1, T2> FSMController { get; }

    void InitFSM(IFSMController<T1, T2> fsmController);

    void AddOnStateEntered(Action<T1> callback);
    void RemoveOnStateEntered(Action<T1> callback);
    void AddOnStateExited(Action<T1> callback);
    void RemoveOnStateExited(Action<T1> callback);
    void EnterFSM();
    void ExitFSM();
    void ForceEnterState(T1 state, TransitionMessage m = null);
    bool SetTransition(T2 transition, TransitionMessage m = null);
    bool IsValidTransition(T2 transition);
    bool HasTransitionFromTo(T1 to);
    bool PushState(T1 t, TransitionMessage m);
    bool PopState();
    T1 GetCurStateID();
    IState<T1, T2> GetState(T1 stateID);
    TState GetState<TState>() where TState : IState<T1, T2>;
}

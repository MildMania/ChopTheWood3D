using System;

public interface IState<T1, T2> : IDisposable
    where T1 : IConvertible
    where T2 : IConvertible
{
    IMMFSM<T1, T2> FSM { get; set; }

    Action OnExitCompleted { get; set; }
    T1 StateID { get; }
    void InitState(IMMFSM<T1, T2> mMFSM);
    bool CanEnter(TransitionMessage m);
    void OnEnter(TransitionMessage m);
    void OnExit();
}

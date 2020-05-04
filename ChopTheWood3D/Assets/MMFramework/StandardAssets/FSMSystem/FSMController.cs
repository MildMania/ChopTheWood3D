using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">State Enum</typeparam>
/// <typeparam name="T2">Transition Enum</typeparam>
/// <typeparam name="TSBImplementer"></typeparam>
public abstract class FSMController<T1, T2> : MonoBehaviour,
    IFSMController<T1, T2>,
    IResetable
    where T1 : IConvertible
    where T2 : IConvertible
{
    protected List<IMMFSM<T1, T2>> _fsmList;
    protected List<IMMFSM<T1, T2>> _FSMList
    {
        get
        {
            if (_fsmList == null)
                _fsmList = GetComponentsInChildren<IMMFSM<T1, T2>>().ToList();

            return _fsmList;
        }
    }

    public IConvertible LastValidTransition { get; private set; }

    #region Events

    public void AddOnStateEnteredListener<TFSM>(Action<T1> callback)
        where TFSM : IMMFSM<T1, T2>
    {
        IMMFSM<T1, T2> targetFSM = GetFSM<TFSM>();

        targetFSM.AddOnStateEntered(callback);
    }

    public void RemoveOnStateEnteredListener<TFSM>(Action<T1> callback)
        where TFSM : IMMFSM<T1, T2>
    {
        IMMFSM<T1, T2> targetFSM = GetFSM<TFSM>();

        targetFSM.RemoveOnStateEntered(callback);
    }

    public void AddOnStateExitedListener<TFSM>(Action<T1> callback)
        where TFSM : IMMFSM<T1, T2>
    {
        IMMFSM<T1, T2> targetFSM = GetFSM<TFSM>();

        targetFSM.AddOnStateExited(callback);
    }

    public void RemoveOnStateExitedListener<TFSM>(Action<T1> callback)
        where TFSM : IMMFSM<T1, T2>
    {
        IMMFSM<T1, T2> targetFSM = GetFSM<TFSM>();

        targetFSM.RemoveOnStateExited(callback);
    }

    #endregion

    void Awake()
    {
        InitFSMs();
        EnterFSMs();
    }

    public void InitFSMs()
    {
        foreach (IMMFSM<T1, T2> fsm in _FSMList)
            fsm.InitFSM(this);
    }

    public void EnterFSMs()
    {
        _FSMList.ForEach(val => val.EnterFSM());
    }

    public void ExitFSMs()
    {
        _FSMList.ForEach(val => val.ExitFSM());
    }

    public void ForceEnterState(T1 state, TransitionMessage m = null)
    {
        _FSMList.ForEach(val => val.ForceEnterState(state, m));
    }

    public bool SetTransition(T2 t, TransitionMessage m = null)
    {
        foreach (IMMFSM<T1, T2> fsm in _FSMList)
            fsm.SetTransition(t, m);

        return true;
    }

    public bool PushState(T1 t, TransitionMessage m)
    {
        foreach (IMMFSM<T1, T2> fsm in _FSMList)
            if (!fsm.HasTransitionFromTo(t))
            {
                return false;
            }

        foreach (IMMFSM<T1, T2> fsm in _FSMList)
            fsm.PushState(t, m);

        return true;
    }

    public bool PopState()
    {
        foreach (IMMFSM<T1, T2> fsm in _FSMList)
            fsm.PopState();

        return true;
    }

    public bool IsValidTransition(T2 t)
    {
        foreach (IMMFSM<T1, T2> fsm in _FSMList)
            if (!fsm.IsValidTransition(t))
                return false;

        return true;
    }

    public IState<T1, T2> GetState(T1 stateID)
    {
        IState<T1, T2> state;

        foreach(IMMFSM<T1, T2> fsm in _FSMList)
        {
            state = fsm.GetState(stateID);

            if (state != null)
                return state;
        }

        throw new Exception("No State found with id: " + stateID.ToString());
    }

    public TState GetState<TState>()
        where TState : IState<T1, T2>
    {
        TState state;

        foreach (IMMFSM<T1, T2> fsm in _FSMList)
        {
            state = fsm.GetState<TState>();

            if (state != null)
                return state;
        }

        throw new Exception("No State found!");
    }

    public T1 GetCurStateIDOfFSM<TFSM>()
        where TFSM : IMMFSM<T1, T2>
    {
        IMMFSM<T1, T2> fsm = _FSMList.Single(val => val is TFSM);

        return fsm.GetCurStateID();
    }

    public void ResetResetable()
    {
        EnterFSMs();
    }

    public TFSM GetFSM<TFSM>()
        where TFSM : IMMFSM<T1, T2>
    {
        return (TFSM)_FSMList.SingleOrDefault(val => val is TFSM);
    }
}

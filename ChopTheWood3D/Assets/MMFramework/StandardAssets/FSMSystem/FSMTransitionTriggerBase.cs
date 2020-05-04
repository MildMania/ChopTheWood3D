using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T1">StateID</typeparam>
/// <typeparam name="T2">TransitionID</typeparam>
/// 
///
public class FSMTransitionTriggerBase<T1, T2, T3> : MonoBehaviour
    where T1 : IConvertible
    where T2 : IConvertible
    where T3 : IFSMController<T1, T2>
{
    public T2 TargetTransition;

    T3 _fsmController;

    protected T3 _FSMController
    {
        get
        {
            if (_fsmController == null)
                _fsmController = GetComponent<T3>();

            return _fsmController;
        }
    }

    protected void TriggerTransition(TransitionMessage m)
    {
        _FSMController.SetTransition(TargetTransition, m);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class LevelEndPhase : PhaseConditionalNode
{
    private IEnumerator _autoCompleteRoutine;
    private float _delay;
    private Action<int> _callback;

    public LevelEndPhase(int id, float delay, params PhaseBaseNode[] childNodeArr) 
        : base(id, childNodeArr)
    {
        _delay = delay;
    }

    protected override void CheckConditions(Action<int> callback)
    {
        _callback = callback;

        if (_autoCompleteRoutine != null)
            CoroutineRunner.Instance.StopCoroutine(_autoCompleteRoutine);

        _autoCompleteRoutine = AutoCompleteRoutine();
        CoroutineRunner.Instance.StartCoroutine(_autoCompleteRoutine);
    }

    private IEnumerator AutoCompleteRoutine()
    {
        float startTime = Time.time;

        while (true)
        {
            if (Time.time - startTime >= _delay)
                break;

            yield return null;
        }

        CheckCondition();
    }

    private void CheckCondition()
    {
        int callbackNodeID = 7;

        if (ThrowerControllerManager.Instance.AnyThrowPhaseLeft)
            callbackNodeID = 6;

        _callback?.Invoke(callbackNodeID);
    }
}

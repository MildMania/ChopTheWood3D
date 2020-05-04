using System.Collections;
using UnityEngine;

public class LevelWinPhase : PhaseActionNode
{
    private IEnumerator _autoCompleteRoutine;
    private float _delay;

    public LevelWinPhase(int id, float autoCompleteDelay) 
        : base(id)
    {
        _delay = autoCompleteDelay;
    }

    protected override void ProcessFlow()
    {
        if (_autoCompleteRoutine != null)
            CoroutineRunner.Instance.StopCoroutine(_autoCompleteRoutine);

        _autoCompleteRoutine = AutoCompleteRoutine();
        CoroutineRunner.Instance.StartCoroutine(_autoCompleteRoutine);
    }

    private IEnumerator AutoCompleteRoutine()
    {
        float startTime = Time.time;

        while(true)
        {
            if (Time.time - startTime >= _delay)
                break;

            yield return null;
        }

        TraverseCompleted();
    }
}

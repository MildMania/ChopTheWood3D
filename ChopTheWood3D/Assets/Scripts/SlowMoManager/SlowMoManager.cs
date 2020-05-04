using System.Collections;
using UnityEngine;

public class SlowMoManager : MonoBehaviour
{
    private static SlowMoManager _instance;
    public static SlowMoManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SlowMoManager>();
            }
            return _instance;
        }
    }

    private AnimationCurve _slowMoCurve;

    private IEnumerator _slowmoRoutine;

    private bool _hasActiveSlowMo;

    private float _timeScaleSpeed;

    public void FinishSlowMo()
    {
        if (_slowmoRoutine != null)
            StopCoroutine(_slowmoRoutine);

        _hasActiveSlowMo = false;
    }

    public void StartSlowMo(AnimationCurve slowMoCurve)
    {
        FinishSlowMo();

        _slowMoCurve = slowMoCurve;

        _slowmoRoutine = SlowMoRoutine();
        StartCoroutine(_slowmoRoutine);
    }

    private IEnumerator SlowMoRoutine()
    {
        _hasActiveSlowMo = true;

        float passedTime = 0.0f;
        Keyframe lastKeyFrame = _slowMoCurve.keys[_slowMoCurve.keys.Length - 1];
        float curveDuration = lastKeyFrame.time;

        while (passedTime < curveDuration)
        {
            SetTimeScaleTo(passedTime);

            passedTime += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        SetTimeScaleTo(curveDuration);

        _hasActiveSlowMo = false;
    }

    private void SetTimeScaleTo(float passedTime)
    {
        float timeScale = _slowMoCurve.Evaluate(passedTime);

        Time.timeScale = timeScale;
    }
}

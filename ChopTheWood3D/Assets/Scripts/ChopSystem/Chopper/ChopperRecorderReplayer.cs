using System;
using System.Collections;
using UnityEngine;

public class ChopperRecorderReplayer : MonoBehaviour
{
    [SerializeField] private ChopperMovementRecorder _recorder;
    [SerializeField] private float _replaySpeed;

    private IEnumerator _replayRoutine;

    #region Events
    public Action OnReplayStarted { get; set; }
    public Action OnReplayFinished { get; set; }
    public Action<Vector3> OnReplayUpdated { get; set; }
    #endregion

    public void TryReplayRecording()
    {
        if (!_recorder.HasRecording)
            return;

        _replayRoutine = ReplayProgress();
        StartCoroutine(_replayRoutine);
    }

    private IEnumerator ReplayProgress()
    {
        transform.position = _recorder.Points[0];

        OnReplayStarted?.Invoke();

        int startPIndex = 0;
        int endPIndex = 1;

        float extraTime = 0;

        while (endPIndex < _recorder.Points.Count)
        {
            float t = extraTime;

            Vector3 p1 = _recorder.Points[startPIndex];
            Vector3 p2 = _recorder.Points[endPIndex];

            while (t <= 1)
            {
                Vector3 newPosition = Vector3.Lerp(p1, p2, t);

                t += Time.deltaTime * _replaySpeed;

                transform.position = newPosition;

                OnReplayUpdated?.Invoke(transform.position);

                yield return null;
            }

            extraTime = t - 1.0f;

            startPIndex++;
            endPIndex++;
        }

        OnReplayFinished?.Invoke();
    }
}

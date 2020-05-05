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

    public Action<RecordingData> OnCurRecordingDataChanged { get; set; }
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
        RecordingData rd = _recorder.RecordingDataCollection[0];

        transform.position = rd.Point;

        OnReplayStarted?.Invoke();

        OnCurRecordingDataChanged?.Invoke(rd);

        int startPIndex = 0;
        int endPIndex = 1;

        float extraTime = 0;

        while (endPIndex < _recorder.RecordingDataCollection.Count)
        {
            float t = extraTime;

            Vector3 p1 = _recorder.RecordingDataCollection[startPIndex].Point;
            Vector3 p2 = _recorder.RecordingDataCollection[endPIndex].Point;

            while (t <= 1)
            {
                Vector3 newPosition = Vector3.Lerp(p1, p2, t);

                t += Time.unscaledDeltaTime * _replaySpeed;

                transform.position = newPosition;

                OnReplayUpdated?.Invoke(transform.position);

                yield return null;
            }

            OnCurRecordingDataChanged?.Invoke(_recorder.RecordingDataCollection[endPIndex]);

            extraTime = t - 1.0f;

            startPIndex++;
            endPIndex++;
        }

        OnReplayFinished?.Invoke();
    }
}

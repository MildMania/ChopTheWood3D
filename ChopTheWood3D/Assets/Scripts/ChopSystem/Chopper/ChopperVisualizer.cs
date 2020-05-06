using UnityEngine;

[RequireComponent(typeof(ChopperMovementRecorder), typeof(LineRenderer))]
public class ChopperVisualizer : MonoBehaviour
{
    public MeshRenderer _cutPointRenderer;

    private ChopperMovementRecorder _recorder;
    private ChopperMovementRecorder _Recorder
    {
        get
        {
            if (_recorder == null)
                _recorder = GetComponent<ChopperMovementRecorder>();

            return _recorder;
        }
    }

    private ChopperRecorderReplayer _replayer;
    private ChopperRecorderReplayer _Replayer
    {
        get
        {
            if (_replayer == null)
                _replayer = GetComponent<ChopperRecorderReplayer>();

            return _replayer;
        }
    }

    private LineRenderer _lineRenderer;
    private LineRenderer _LineRenderer
    {
        get
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();

            return _lineRenderer;
        }
    }

    private void Awake()
    {
        RegisterToRecorder();
        RegisterToReplayer();

        StopVisualizing();
    }

    private void OnDestroy()
    {
        UnregisterFromRecorder();
        UnregisterFromReplayer();
    }

    #region Recorder Related
    private void RegisterToRecorder()
    {
        _Recorder.OnRecordingStarted += OnRecordingStarted;
        _Recorder.OnRecordingEnded += OnRecordingEnded;

        _Recorder.OnRecordingDataAdded += OnPointAdded;
    }

    private void UnregisterFromRecorder()
    {
        _Recorder.OnRecordingStarted -= OnRecordingStarted;
        _Recorder.OnRecordingEnded -= OnRecordingEnded;

        _Recorder.OnRecordingDataAdded -= OnPointAdded;
    }

    private void OnRecordingStarted()
    {
        StartVisualizing();
    }

    private void OnRecordingEnded()
    {
        StopVisualizing();
    }

    private void OnPointAdded(RecordingData rd)
    {
        UpdateLineRenderer(rd.Point);
    }
    #endregion

    #region Replayer Related
    private void RegisterToReplayer()
    {
        _Replayer.OnReplayStarted += OnReplayStarted;
        _Replayer.OnReplayFinished += OnReplayFinished;

        _Replayer.OnReplayUpdated += OnReplayUpdated;
    }

    private void UnregisterFromReplayer()
    {
        _Replayer.OnReplayStarted -= OnReplayStarted;
        _Replayer.OnReplayFinished -= OnReplayFinished;

        _Replayer.OnReplayUpdated -= OnReplayUpdated;
    }

    private void OnReplayStarted()
    {
        StartVisualizing();
    }

    private void OnReplayFinished()
    {
        StopVisualizing();
    }

    private void OnReplayUpdated(Vector3 point)
    {
        UpdateLineRenderer(point);
    }
    #endregion

    private void UpdateLineRenderer(Vector3 point)
    {
        _LineRenderer.positionCount++;
        _LineRenderer.SetPosition(_LineRenderer.positionCount - 1, point);

        if (_LineRenderer.positionCount == 2)
            _LineRenderer.enabled = true;
    }

    private void StartVisualizing()
    {
        _cutPointRenderer.enabled = true;
    }

    private void StopVisualizing()
    {
        _LineRenderer.positionCount = 0;
        _LineRenderer.enabled = false;

        _cutPointRenderer.enabled = false;
    }

}



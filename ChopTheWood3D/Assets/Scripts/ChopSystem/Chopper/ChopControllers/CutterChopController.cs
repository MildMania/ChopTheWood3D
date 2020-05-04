using UnityEngine;

public class CutterChopController : ChopControllerBase
{
    [SerializeField] private ChopperRecorderReplayer _replayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _replayer.TryReplayRecording();
    }

    protected override void AwakeCustomActions()
    {
        RegisterToRecorder();

        base.AwakeCustomActions();
    }

    protected override void OnDestroyCustomActions()
    {
        UnregisterFromRecorder();

        base.OnDestroyCustomActions();
    }

    private void RegisterToRecorder()
    {
        _replayer.OnReplayStarted += OnReplayStarted;
        _replayer.OnReplayFinished += OnReplayFinished;
        _replayer.OnReplayUpdated += OnReplayUpdated;
    }

    private void UnregisterFromRecorder()
    {
        _replayer.OnReplayStarted -= OnReplayStarted;
        _replayer.OnReplayFinished -= OnReplayFinished;
        _replayer.OnReplayUpdated -= OnReplayUpdated;
    }

    private void OnReplayStarted()
    {
        StartChopping();
    }

    private void OnReplayFinished()
    {
        StopChopping();
    }

    private void OnReplayUpdated(Vector3 newPosition)
    {
        OnMoved?.Invoke(newPosition);
    }
}

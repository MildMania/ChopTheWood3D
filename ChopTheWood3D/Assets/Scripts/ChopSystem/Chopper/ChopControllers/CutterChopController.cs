using UnityEngine;

public class CutterChopController : ChopControllerBase
{
    [SerializeField] private ChopperMovementRecorder _recorder;

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
        _recorder.OnReplayStarted += OnReplayStarted;
        _recorder.OnReplayFinished += OnReplayFinished;
        _recorder.OnReplayUpdated += OnReplayUpdated;
    }

    private void UnregisterFromRecorder()
    {
        _recorder.OnReplayStarted -= OnReplayStarted;
        _recorder.OnReplayFinished -= OnReplayFinished;
        _recorder.OnReplayUpdated -= OnReplayUpdated;
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

using UnityEngine;

public class CutterChopController : ChopControllerBase
{
    [SerializeField] private ChopperRecorderReplayer _replayer;

    protected override void AwakeCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseTraverFinished;

        base.AwakeCustomActions();
    }

    private void OnPhaseTraverStarted(PhaseBaseNode phase)
    {
        if (phase is ChopperCutPhase)
        {
            RegisterToRecorder();

            _replayer.TryReplayRecording();
        }
    }

    private void OnPhaseTraverFinished(PhaseBaseNode phase)
    {
        if (phase is ChopperCutPhase)
            UnregisterFromRecorder();
    }

    protected override void OnDestroyCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseTraverFinished;

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

using System;
using UnityEngine;

public class CutterChopController : ChopControllerBase
{
    [SerializeField] private ChopperRecorderReplayer _replayer;

    private CutterChopBehaviour _CutterChopBehaviour
    {
        get
        {
            return (CutterChopBehaviour)_chopBehaviour;
        }
    }

    private ChopperCutPhase _chopperCutPhase;

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
            Debug.Log("Chopped Cut Phase Started");

            _chopperCutPhase = (ChopperCutPhase)phase;

            RegisterToRecorder();

            if(!_replayer.TryReplayRecording())
                _chopperCutPhase.CompleteTraverse();
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
        _replayer.OnCurRecordingDataChanged += OnCurRecordingDataChanged;
    }

    private void UnregisterFromRecorder()
    {
        _replayer.OnReplayStarted -= OnReplayStarted;
        _replayer.OnReplayFinished -= OnReplayFinished;
        _replayer.OnCurRecordingDataChanged -= OnCurRecordingDataChanged;
    }

    private void OnReplayStarted()
    {
        StartChopping();
    }

    private void OnReplayFinished()
    {
        StopChopping();

        _chopperCutPhase.CompleteTraverse();
    }


    private void OnCurRecordingDataChanged(RecordingData recordingData)
    {
        _CutterChopBehaviour.Move(recordingData);
    }

}

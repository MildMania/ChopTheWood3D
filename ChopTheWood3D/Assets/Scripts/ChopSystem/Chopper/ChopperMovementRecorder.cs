using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RecordingData
{
    public Vector3 Point { get; set; }
    public IChopperInteractable InteractedInteractable { get; set; }

    public RecordingData(Vector3 point)
    {
        Point = point;
        InteractedInteractable = null;
    }

    public RecordingData(
        Vector3 point,
        IChopperInteractable interactedInteractable)
    {
        Point = point;
        InteractedInteractable = interactedInteractable;
    }
}

public class ChopperMovementRecorder : MonoBehaviour
{
    [SerializeField] private ChopperMovementController _movementController;
    [SerializeField] private GhostChopController _ghostChopController;

    [SerializeField] private float _pointDistanceTreshold;

    public bool HasRecording { get; private set; }
    public List<RecordingData> RecordingDataCollection { get; } = new List<RecordingData>();

    private IEnumerator _recordRoutine;

    #region Events
    public Action OnRecordingStarted { get; set; }
    public Action OnRecordingEnded { get; set; }
    public Action<RecordingData> OnRecordingDataAdded { get; set; }

    #endregion

    private void Awake()
    {
        RegisterToPhaseBaseNode();
        RegisterToMovementController();
    }

    private void OnDestroy()
    {
        UnregisterFromPhaseNode();
        UnregisterFromMovementController();
    }

    private void RegisterToPhaseBaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverseStarted;
    }

    private void UnregisterFromPhaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverseStarted;
    }

    private void OnPhaseTraverseStarted(PhaseBaseNode phaseNode)
    {
        if (phaseNode is GhostCutPhase)
            ResetRecording();
    }

    private void RegisterToMovementController()
    {
        _movementController.OnMovementStarted += OnMovementStarted;
        _movementController.OnMovementEnded += OnMovementEnded;
    }

    private void UnregisterFromMovementController()
    {
        _movementController.OnMovementStarted -= OnMovementStarted;
        _movementController.OnMovementEnded -= OnMovementEnded;
    }

    private void OnMovementStarted()
    {
        StartRecordingMovement();

        OnRecordingStarted?.Invoke();
    }

    private void OnMovementEnded()
    {
        StopRecordingMovement();

        OnRecordingEnded?.Invoke();
    }

    private void StartRecordingMovement()
    {
        ResetRecording();

        _ghostChopController.ChopBehaviour.OnTouchedChoppable += OnTouchedChoppable;

        _recordRoutine = RecordProgress();
        StartCoroutine(_recordRoutine);
    }

    private void StopRecordingMovement()
    {
        _ghostChopController.ChopBehaviour.OnTouchedChoppable -= OnTouchedChoppable;

        if (_recordRoutine != null)
            StopCoroutine(_recordRoutine);

        HasRecording = true;
    }

    private void OnTouchedChoppable(IChopperInteractable interactable)
    {
        RecordingData rd = RecordingDataCollection[RecordingDataCollection.Count - 1];
        rd.InteractedInteractable = interactable;

        RecordingDataCollection[RecordingDataCollection.Count - 1] = rd;
    }


    private IEnumerator RecordProgress()
    {
        RecordingDataCollection.Clear();

        while (true)
        {
            TryRecordPosition();

            yield return null;
        }
    }

    private bool TryRecordPosition()
    {
        if(RecordingDataCollection.Count > 0)
        {
            Vector3 prevPos = RecordingDataCollection[RecordingDataCollection.Count - 1].Point;

            float distance = Vector3.Distance(transform.position, prevPos);

            if (distance < _pointDistanceTreshold)
                return false;
        }

        RecordingData rd = new RecordingData(transform.position);

        RecordingDataCollection.Add(rd);

        OnRecordingDataAdded?.Invoke(rd);

        return true;
    }


    private void ResetRecording()
    {
        HasRecording = false;

        RecordingDataCollection.Clear();
    }
}

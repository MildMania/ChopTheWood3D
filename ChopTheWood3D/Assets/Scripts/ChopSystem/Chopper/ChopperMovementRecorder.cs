using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopperMovementRecorder : MonoBehaviour
{
    [SerializeField] private ChopperMovementController _movementController;

    [SerializeField] private float _pointDistanceTreshold;

    public bool HasRecording { get; private set; }
    public List<Vector3> Points { get; } = new List<Vector3>();

    private IEnumerator _recordRoutine;

    #region Events
    public Action OnRecordingStarted { get; set; }
    public Action OnRecordingEnded { get; set; }
    public Action<Vector3> OnPointAdded { get; set; }

    #endregion

    private void Awake()
    {
        RegisterToMovementController();
    }

    private void OnDestroy()
    {
        UnregisterFromMovementController();
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
    }

    private void OnMovementEnded()
    {
        StopRecordingMovement();
    }

    private void StartRecordingMovement()
    {
        HasRecording = false;

        _recordRoutine = RecordProgress();
        StartCoroutine(_recordRoutine);
    }

    private void StopRecordingMovement()
    {
        if (_recordRoutine != null)
            StopCoroutine(_recordRoutine);

        HasRecording = true;
    }

    private IEnumerator RecordProgress()
    {
        Points.Clear();

        while (true)
        {
            TryRecordPosition();

            yield return null;
        }
    }

    private bool TryRecordPosition()
    {
        if(Points.Count > 0)
        {
            Vector3 prevPos = Points[Points.Count - 1];

            float distance = Vector3.Distance(transform.position, prevPos);

            if (distance < _pointDistanceTreshold)
                return false;
        }

        Points.Add(transform.position);

        OnPointAdded?.Invoke(transform.position);

        return true;
    }

    
}

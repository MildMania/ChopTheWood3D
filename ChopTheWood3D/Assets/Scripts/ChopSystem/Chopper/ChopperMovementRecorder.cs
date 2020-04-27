using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopperMovementRecorder : MonoBehaviour
{
    [SerializeField] private ChopperMovementController _movementController;

    [SerializeField] private float _pointDistanceTreshold;
    [SerializeField] private float _replaySpeed;

    public List<Vector3> Points { get; } = new List<Vector3>();

    private IEnumerator _recordRoutine;
    private IEnumerator _replayRoutine;

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
        _recordRoutine = RecordProgress();
        StartCoroutine(_recordRoutine);
    }

    private void StopRecordingMovement()
    {
        if (_recordRoutine != null)
            StopCoroutine(_recordRoutine);

        ReplayRecording();
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

    private void ReplayRecording()
    {
        _replayRoutine = ReplayProgress();
        StartCoroutine(_replayRoutine);
    }

    private IEnumerator ReplayProgress()
    {
        int startPIndex = 0;
        int endPIndex = 1;

        float extraTime = 0;

        while (endPIndex < Points.Count)
        {
            float t = extraTime;

            Vector3 p1 = Points[startPIndex];
            Vector3 p2 = Points[endPIndex];

            while(t <= 1)
            {
                Vector3 newPosition = Vector3.Lerp(p1, p2, t);

                t += Time.deltaTime * _replaySpeed;

                transform.position = newPosition;

                yield return null;
            }

            extraTime = t - 1.0f;

            startPIndex++;
            endPIndex++;
        }
    }
}

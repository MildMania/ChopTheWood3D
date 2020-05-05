﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChopperMovementRecorder), typeof(LineRenderer))]
public class ChopperRecorderVisualizer : MonoBehaviour
{

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
    }

    private void OnDestroy()
    {
        UnregisterFromRecorder();
    }

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
        _LineRenderer.positionCount = _Recorder.RecordingDataCollection.Count;
        _LineRenderer.SetPosition(_Recorder.RecordingDataCollection.Count - 1, rd.Point);

        if (_Recorder.RecordingDataCollection.Count == 2)
            _LineRenderer.enabled = true;
    }

    private void StartVisualizing()
    {
    }

    private void StopVisualizing()
    {
        _LineRenderer.positionCount = 0;
        _LineRenderer.enabled = false;
    }

}



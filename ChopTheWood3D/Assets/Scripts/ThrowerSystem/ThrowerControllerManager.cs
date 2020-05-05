using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThrowPhaseInfo
{
    [SerializeField] private bool _canShowInScene;
    public bool CanShowInScene
    {
        get
        {
            return _canShowInScene;
        }
    }

    [SerializeField] private ThrowerController _throwerController;

    public ThrowerController ThrowerController
    {
        get
        {
            return _throwerController;
        }
    }
}

public class ThrowerControllerManager : MonoBehaviour
{
    private static ThrowerControllerManager _instance;
    public static ThrowerControllerManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ThrowerControllerManager>();

            return _instance;
        }
    }

    [SerializeField] private ThrowPhaseInfo[] _throwPhaseInfoArr;

    private int _throwPhaseIndex;

    public bool AnyThrowPhaseLeft { get; private set; }

    private void Awake()
    {
        CheckAnyThrowPhaseLeft();

        RegisterToPhaseBaseNode();
    }

    private void OnDestroy()
    {
        UregisterFromPhaseBaseNode();
    }

    private void RegisterToPhaseBaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseNodeTraverStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseNodeTraverFinished;
    }

    private void UregisterFromPhaseBaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseNodeTraverStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseNodeTraverFinished;
    }

    private void OnPhaseNodeTraverStarted(PhaseBaseNode phaseNode)
    {
        if (!(phaseNode is GhostCutPhase))
            return;

        ActivateThrowerController();
    }

    private void OnPhaseNodeTraverFinished(PhaseBaseNode phaseNode)
    {
        if (!(phaseNode is GhostCutPhase))
            return;

        _throwPhaseIndex++;

        CheckAnyThrowPhaseLeft();
    }

    private void ActivateThrowerController()
    {
        _throwPhaseInfoArr[_throwPhaseIndex].ThrowerController.ActivateThrowers();
    }

    private void CheckAnyThrowPhaseLeft()
    {
        if (_throwPhaseIndex > _throwPhaseInfoArr.Length)
            AnyThrowPhaseLeft = false;
        else
            AnyThrowPhaseLeft = true;

    }
}

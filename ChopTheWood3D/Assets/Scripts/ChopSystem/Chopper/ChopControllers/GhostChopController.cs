using System;
using UnityEngine;

public class GhostChopController : ChopControllerBase
{
    [SerializeField] private ChopperMovementController _movementController;

    private GhostCutPhase _ghostCutPhase;

    private GhostChopBehaviour _GhostChopBehaviour
    {
        get
        {
            return (GhostChopBehaviour)_chopBehaviour;
        }
    }

    protected override void AwakeCustomActions()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseTraverStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseTraverFinished;

        base.AwakeCustomActions();
    }

    private void OnPhaseTraverStarted(PhaseBaseNode phase)
    {
        if (phase is GhostCutPhase)
        {
            _ghostCutPhase = (GhostCutPhase)phase;

            ChoppableController.Instance.OnNoVisibleChoppableLeft += OnNoVisibleChoppableLeft;

            RegisterToMovementController();
        }
    }

    private void OnPhaseTraverFinished(PhaseBaseNode phase)
    {
        if (phase is GhostCutPhase)
        {
            ChoppableController.Instance.OnNoVisibleChoppableLeft -= OnNoVisibleChoppableLeft;

            UnregisterFromMovementController();
        }
    }

    protected override void OnDestroyCustomActions()
    {
        ChoppableController.Instance.OnNoVisibleChoppableLeft -= OnNoVisibleChoppableLeft;

        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseTraverStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseTraverFinished;

        UnregisterFromMovementController();

        base.OnDestroyCustomActions();
    }

    private void RegisterToMovementController()
    {
        _movementController.OnMovementStarted += OnMovementStarted;
        _movementController.OnMovementEnded += OnMovementEnded;
        _movementController.OnMoved += OnChopperMoved;
    }

    private void UnregisterFromMovementController()
    {
        _movementController.OnMovementStarted -= OnMovementStarted;
        _movementController.OnMovementEnded -= OnMovementEnded;
        _movementController.OnMoved -= OnChopperMoved;
    }

    private void OnNoVisibleChoppableLeft()
    {
        _ghostCutPhase.CompleteTraverse();
    }

    private void OnMovementEnded()
    {
        StopChopping();

        _ghostCutPhase.CompleteTraverse();
    }

    private void OnMovementStarted()
    {
        StartChopping();
    }

    private void OnChopperMoved(Vector3 newPosition)
    {
        _GhostChopBehaviour.Move(newPosition);
    }
}

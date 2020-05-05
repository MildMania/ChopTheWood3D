using UnityEngine;

public class GhostChopController : ChopControllerBase
{
    [SerializeField] private ChopperMovementController _movementController;

    private GhostCutPhase _ghostCutPhase;

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
            RegisterToMovementController();
        }
    }

    private void OnPhaseTraverFinished(PhaseBaseNode phase)
    {
        if (phase is GhostCutPhase)
            UnregisterFromMovementController();
    }

    protected override void OnDestroyCustomActions()
    {
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
        OnMoved?.Invoke(newPosition);
    }
}

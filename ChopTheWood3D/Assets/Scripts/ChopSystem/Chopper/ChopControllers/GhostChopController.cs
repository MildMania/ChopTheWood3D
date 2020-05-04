using UnityEngine;

public class GhostChopController : ChopControllerBase
{
    [SerializeField] private ChopperMovementController _movementController;

    protected override void AwakeCustomActions()
    {
        RegisterToMovementController();

        base.AwakeCustomActions();
    }

    protected override void OnDestroyCustomActions()
    {
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

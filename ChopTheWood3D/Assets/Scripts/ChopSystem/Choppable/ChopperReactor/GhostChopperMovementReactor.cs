using UnityEngine;

public class GhostChopperMovementReactor : ChopperReactorBase<GhostChopController>
{
    [SerializeField] private Throwable _throwable;
    [SerializeField] private FreeFallBehaviour _freeFallbehaviour;

    public override void ChopFailed(ChopControllerBase chopController)
    {
        _throwable.StopThrow();
        _freeFallbehaviour.StartFreeFall();
    }

    public override void ChoppedChoppable(ChopControllerBase chopController)
    {
        _throwable.StopThrow();
    }

    public override void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece)
    {
    }

    public override void DecreasedHealth(ChopControllerBase chopController, ChoppablePiece piece)
    {
    }

    public override void ExitedPiece(ChopControllerBase chopController, ChoppablePiece piece)
    {
    }
}

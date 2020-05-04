using UnityEngine;

public class CutterChopperReactor : ChopperReactorBase<CutterChopController>
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _upwardModifier;

    public override void ChopFailed(ChopControllerBase chopController)
    {
    }

    public override void ChoppedChoppable(ChopControllerBase chopController)
    {
        foreach(ChoppablePiece piece in Parent.Pieces)
        {
            foreach(PieceLog log in piece.PieceLogs)
            {
                log.Rigidbody.useGravity = true;
                log.Rigidbody.AddExplosionForce(_explosionForce, log.ExplosionTransform.position, _explosionRadius, _upwardModifier);
            }
        }
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

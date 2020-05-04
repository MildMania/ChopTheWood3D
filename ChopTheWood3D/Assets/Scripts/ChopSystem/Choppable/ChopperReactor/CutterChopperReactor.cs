using UnityEngine;

public class CutterChopperReactor : ChopperReactorBase<CutterChopController>
{
    [SerializeField] private float _cutSpeed;

    public override void ChopFailed(ChopControllerBase chopController)
    {
    }

    public override void ChoppedChoppable(ChopControllerBase chopController)
    {
        foreach(ChoppablePiece piece in Parent.Pieces)
        {
            foreach(PieceLog log in piece.PieceLogs)
            {
                log.Rigidbody.velocity
                    = _cutSpeed * log.Rigidbody.transform.TransformDirection(log.LocalForward);
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

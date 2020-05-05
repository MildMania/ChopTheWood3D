using UnityEngine;

[System.Serializable]
public class PieceLog
{
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
    }

    [SerializeField] private Transform _explosionTransform;
    public Transform ExplosionTransform
    {
        get
        {
            return _explosionTransform;
        }
    }
}

public class CutterChopperReactor : ChopperReactorBase<CutterChopController>
{
    [SerializeField] private float _minExplosionForce;
    [SerializeField] private float _maxExplosionForce;

    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _upwardModifier;

    [SerializeField] private PieceLog[] _pieceLogs;
    public PieceLog[] PieceLogs
    {
        get
        {
            return _pieceLogs;
        }
    }

    public override void ChopFailed(ChopControllerBase chopController)
    {
    }

    public override void ChoppedChoppable(ChopControllerBase chopController)
    {
        foreach (PieceLog log in PieceLogs)
        {
            float targetForce = Utilities.NextFloat(_minExplosionForce, _maxExplosionForce);

            log.Rigidbody.useGravity = true;
            log.Rigidbody.AddExplosionForce(targetForce, log.ExplosionTransform.position, _explosionRadius, _upwardModifier);
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

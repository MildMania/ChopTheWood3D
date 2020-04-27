using System;
using UnityEngine;

public interface IChopperReactor
{
    void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece);
    void ChoppedChoppable(ChopControllerBase chopController);
    void ChopFailed(ChopControllerBase chopController);
    Type GetChopControllerType();
}

public abstract class ChopperReactorBase<TChopController> : MonoBehaviour, IChopperReactor
    where TChopController : ChopControllerBase
{
    public abstract void ChoppedChoppable(ChopControllerBase chopController);

    public abstract void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece);

    public abstract void ChopFailed(ChopControllerBase chopController);

    public abstract Type GetChopControllerType();
}

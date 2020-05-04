using System;
using UnityEngine;

public interface IChopperReactor
{
    void SetParentChoppable(Choppable choppable);
    void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece);
    void ExitedPiece(ChopControllerBase chopController, ChoppablePiece piece);
    void ChoppedChoppable(ChopControllerBase chopController);
    void ChopFailed(ChopControllerBase chopController);
    void DecreasedHealth(ChopControllerBase chopController, ChoppablePiece piece);
    Type GetChopControllerType();

}

public abstract class ChopperReactorBase<TChopController> : MonoBehaviour, IChopperReactor
    where TChopController : ChopControllerBase
{
    public Choppable Parent { get; private set; }

    public void SetParentChoppable(Choppable choppable)
    {
        Parent = choppable;
    }

    public abstract void ChoppedChoppable(ChopControllerBase chopController);

    public abstract void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece);

    public abstract void ExitedPiece(ChopControllerBase chopController, ChoppablePiece piece);

    public abstract void ChopFailed(ChopControllerBase chopController);

    public abstract void DecreasedHealth(ChopControllerBase chopController, ChoppablePiece piece);

    public Type GetChopControllerType()
    {
        return typeof(TChopController);
    }

}

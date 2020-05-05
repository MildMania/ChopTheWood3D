using System;
using UnityEngine;

public class ChopBehaviourBase : MonoBehaviour
{
    protected Vector3 _prevPosition;

    public Action<Choppable, ChoppablePiece> OnPieceChopped { get; set; }
    public Action<Choppable, ChoppablePiece> OnExitedPiece { get; set; }
    public Action<Choppable> OnChoppableChopped { get; set; }
    public Action<Choppable> OnChoppableFailed { get; set; }
    public Action<Choppable, ChoppablePiece> OnChoppableHealthDecreased { get; set; }

    public Action<IChopperInteractable> OnTouchedChoppable { get; set; }

    public void StartChopping(Vector3 startPos)
    {
        _prevPosition = startPos;
    }

    public void StopChopping()
    {

    }

    protected bool TryTouchInteractable(
        IChopperInteractable interactable,
        out ChoppableTouchResult result)
    {
        result = new ChoppableTouchResult();

        if (interactable.ParentChoppable.TryTouchChoppable(interactable, result))
        {
            OnTouchedChoppable?.Invoke(interactable);

            if (result.Result == ETouchResult.ChoppedPiece)
                OnPieceChopped?.Invoke(result.Choppable, result.ChoppedPiece);
            else if (result.Result == ETouchResult.ExitingPiece)
                OnExitedPiece?.Invoke(result.Choppable, result.ChoppedPiece);
            else if (result.Result == ETouchResult.ChoppedAll)
                OnChoppableChopped?.Invoke(result.Choppable);
            else if (result.Result == ETouchResult.Failed)
                OnChoppableFailed?.Invoke(result.Choppable);
            else if (result.Result == ETouchResult.DecreasedHealth)
                OnChoppableHealthDecreased?.Invoke(result.Choppable, result.ChoppedPiece);

            return true;
        }

        return false;
    }
}

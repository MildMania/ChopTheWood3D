using UnityEngine;
using System;

public class ChopBehaviour : MonoBehaviour
{
    [SerializeField] private float _chopDistanceTreshold;

    [SerializeField] private LayerMask _edgeLayerMask;
    [SerializeField] private LayerMask _pieceLayerMask;

    private ChopControllerBase _checkerController;

    private Vector3 _prevPosition;
    public bool _isInsideChoppable;

    public Action<Choppable, ChoppablePiece> OnPieceChopped { get; set; }
    public Action<Choppable> OnChoppableChopped { get; set; }
    public Action<Choppable> OnChoppableFailed { get; set; }

    public void StartChopping(
        ChopControllerBase checkerController,
        Vector3 startPos)
    {
        _checkerController = checkerController;
        _checkerController.OnMoved += OnMoved;

        _prevPosition = startPos;
    }

    public void StopChopping()
    {
        _checkerController.OnMoved -= OnMoved;
        _checkerController = null;
    }

    private void OnMoved(Vector3 newPosition)
    {
        float distance = Vector3.Distance(newPosition, _prevPosition);

        if (distance < _chopDistanceTreshold)
            return;

        Ray ray;

        ray = new Ray(_prevPosition, (newPosition - _prevPosition).normalized);

        RaycastHit edgeHit = GetHitForEdges(ray, distance);

        if (TryTouchInteractable(edgeHit))
        {
            _prevPosition = newPosition;

            return;
        }

        if (_isInsideChoppable)
            ray = new Ray(newPosition, (_prevPosition - newPosition).normalized);

        RaycastHit pieceHit = GetHitForPieces(ray, distance);

        TryTouchInteractable(pieceHit);

        _prevPosition = newPosition;
    }

    private RaycastHit GetHitForEdges(Ray ray, float distance)
    {
        RaycastHit hit;

        Physics.Raycast(
            ray,
            out hit,
            distance,
            _edgeLayerMask);

        return hit;
    }

    private RaycastHit GetHitForPieces(Ray ray, float distance)
    {
        RaycastHit hit;

        Physics.Raycast(
            ray,
            out hit,
            distance,
            _pieceLayerMask);

        return hit;
    }

    private bool TryTouchInteractable(RaycastHit hit)
    {
        if (hit.collider == null)
            return false;

        if (!hit.collider.gameObject.TryGetComponent(out IChopperInteractable interactable))
            return false;

        ChoppableTouchResult result = new ChoppableTouchResult();

        if (interactable.ParentChoppable.TryTouchChoppable(interactable, result))
        {
            Debug.Log("Result: " + result.Result.ToString());

            if (interactable.ParentChoppable.ChopState == EChopState.Chopping)
                _isInsideChoppable = true;
            else
                _isInsideChoppable = false;

            if (result.Result == ETouchResult.ChoppedPiece)
                OnPieceChopped?.Invoke(result.Choppable, result.ChoppedPiece);
            else if (result.Result == ETouchResult.ChoppedAll)
                OnChoppableChopped?.Invoke(result.Choppable);
            else if (result.Result == ETouchResult.Failed)
                OnChoppableFailed?.Invoke(result.Choppable);

            return true;
        }

        Debug.Log("Result: " + result.Result.ToString());

        return false;
    }
}

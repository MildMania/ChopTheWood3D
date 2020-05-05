using UnityEngine;

public class GhostChopBehaviour : ChopBehaviourBase
{
    [SerializeField] private float _chopDistanceTreshold;

    [SerializeField] private LayerMask _edgeLayerMask;
    [SerializeField] private LayerMask _pieceLayerMask;

    public bool _isInsideChoppable;

    public void Move(Vector3 newPosition)
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

        if(TryTouchInteractable(interactable, out ChoppableTouchResult result))
        {
            if (interactable.ParentChoppable.ChopState == EChopState.Chopping)
                _isInsideChoppable = true;
            else
                _isInsideChoppable = false;

            return true;
        }

        return false;
    }
}

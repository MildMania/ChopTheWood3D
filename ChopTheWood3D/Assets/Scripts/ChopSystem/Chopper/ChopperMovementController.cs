using System;
using UnityEngine;

public class ChopperMovementController : MonoBehaviour
{
    [SerializeField] private ChopperInputController _inputController;

    private Camera _camera;
    private Camera _Camera
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main;

            return _camera;
        }
    }

    #region Events
    public Action OnMovementStarted { get; set; }
    public Action OnMovementEnded { get; set; }
    public Action<Vector3> OnMoved { get; set; }
    #endregion

    private void Awake()
    {
        RegisterToInputController();
    }

    private void OnDestroy()
    {
        UnregisterToInputController();
    }

    private void RegisterToInputController()
    {
        _inputController.OnMoveStartInput += OnMoveStartInput;
        _inputController.OnMoveInput += OnMoveInput;
        _inputController.OnMoveEndInput += OnMoveEndInput;
    }

    private void UnregisterToInputController()
    {
        _inputController.OnMoveStartInput -= OnMoveStartInput;
        _inputController.OnMoveInput -= OnMoveInput;
        _inputController.OnMoveEndInput -= OnMoveEndInput;
    }

    private void OnMoveStartInput(Input_WI_OnFingerDown input)
    {
        SetPosition(input.FingerPos);

        OnMovementStarted?.Invoke();
    }

    private void OnMoveEndInput(Input_WI_OnFingerUp input)
    {
        OnMovementEnded?.Invoke();
    }

    private void OnMoveInput(Input_WI_OnDragMove input)
    {
        SetPosition(input.FingerPos);

        OnMoved?.Invoke(transform.position);
    }

    private void SetPosition(Vector2 screenPos)
    {
        Vector3 newPosition = _Camera.GetWorldPositionOnPlane(screenPos, transform.position.z);

        transform.position = newPosition;
    }

}

using System;
using UnityEngine;

public class ChopControllerBase : MonoBehaviour
{
    [SerializeField] private ChopBehaviour _chopBehaviour;

    public Action<Vector3> OnMoved { get; set; }

    private void Awake()
    {
        AwakeCustomActions();
    }

    protected virtual void AwakeCustomActions()
    {

    }

    private void OnDestroy()
    {
        OnDestroyCustomActions();
    }

    protected virtual void OnDestroyCustomActions()
    {

    }

    protected void StartChopping()
    {
        RegisterToChopBehaviour();
        _chopBehaviour.StartChopping(this, transform.position);

    }

    protected void StopChopping()
    {
        _chopBehaviour.StopChopping(this);
        UnregisterFromChopBehaviour();
    }

    private void RegisterToChopBehaviour()
    {
        _chopBehaviour.OnPieceChopped += OnPieceChopped;
        _chopBehaviour.OnExitedPiece += OnExitedPiece;
        _chopBehaviour.OnChoppableChopped += OnChoppableChoped;
        _chopBehaviour.OnChoppableFailed += OnChoppableFailed;
        _chopBehaviour.OnChoppableHealthDecreased += OnChoppableHealthDecreased;
    }

    private void UnregisterFromChopBehaviour()
    {
        _chopBehaviour.OnPieceChopped -= OnPieceChopped;
        _chopBehaviour.OnExitedPiece -= OnExitedPiece;
        _chopBehaviour.OnChoppableChopped -= OnChoppableChoped;
        _chopBehaviour.OnChoppableFailed -= OnChoppableFailed;
        _chopBehaviour.OnChoppableHealthDecreased -= OnChoppableHealthDecreased;
    }

    private void OnPieceChopped(Choppable c, ChoppablePiece cPiece)
    {
        c.ChoppedPiece(this, cPiece);
    }

    private void OnExitedPiece(Choppable c, ChoppablePiece cPiece)
    {
        c.ExitedPiece(this, cPiece);
    }

    private void OnChoppableChoped(Choppable c)
    {
        c.ChoppedChoppable(this);
    }

    private void OnChoppableFailed(Choppable c)
    {
        c.ChopFailed(this);
    }

    private void OnChoppableHealthDecreased(Choppable c, ChoppablePiece cPiece)
    {
        c.DecreasedHealth(this, cPiece);
    }

}

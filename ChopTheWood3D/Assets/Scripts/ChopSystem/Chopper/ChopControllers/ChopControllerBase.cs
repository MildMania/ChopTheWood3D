using System;
using UnityEngine;

public class ChopControllerBase : MonoBehaviour
{
    [SerializeField] protected ChopBehaviourBase _chopBehaviour;
    public ChopBehaviourBase ChopBehaviour
    {
        get
        {
            return _chopBehaviour;
        }
    }

    #region Events

    public Action OnStartedChopping { get; set; }
    public Action OnStoppedChopping { get; set; }

    #endregion

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
        _chopBehaviour.StartChopping(transform.position);

        OnStartedChopping?.Invoke();
    }

    protected void StopChopping()
    {
        _chopBehaviour.StopChopping();
        UnregisterFromChopBehaviour();

        OnStoppedChopping?.Invoke();
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

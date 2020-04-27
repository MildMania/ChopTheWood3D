using System;
using UnityEngine;

public class ChopControllerBase : MonoBehaviour
{
    [SerializeField] protected ChopBehaviour _chopBehaviour;
    public ChopBehaviour ChopBehaviour
    {
        get
        {
            return _chopBehaviour;
        }
    }

    public Action<Vector3> OnMoved { get; set; }

    private void Awake()
    {
        AwakeCustomActions();

        RegisterToChopBehaviour();
    }

    protected virtual void AwakeCustomActions()
    {

    }

    private void OnDestroy()
    {
        OnDestroyCustomActions();

        UnregisterFromChopBehaviour();
    }

    protected virtual void OnDestroyCustomActions()
    {

    }

    private void RegisterToChopBehaviour()
    {
        _chopBehaviour.OnPieceChopped += OnPieceChopped;
        _chopBehaviour.OnChoppableChopped += OnChoppableChoped;
        _chopBehaviour.OnChoppableFailed += OnChoppableFailed;
    }

    private void OnPieceChopped(Choppable c, ChoppablePiece cPiece)
    {
        c.ChoppedPiece(this, cPiece);
    }

    private void OnChoppableChoped(Choppable c)
    {
        c.ChoppedChoppable(this);
    }

    private void OnChoppableFailed(Choppable c)
    {
        c.ChopFailed(this);
    }

    private void UnregisterFromChopBehaviour()
    {

    }

}

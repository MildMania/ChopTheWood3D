using UnityEngine;
using System;
using System.Linq;

public enum EChopState
{
    Idle,
    Chopping,
    Exiting,
    Succeeded,
    Failed,
}

public enum ETouchResult
{
    NoTouch, 
    Entered,
    Exiting,
    ChoppedPiece,
    Failed,
    ChoppedAll
}

public class ChoppableTouchResult
{
    public ETouchResult Result { get; set; }
    public ChoppablePiece ChoppedPiece { get; set; }
    public Choppable Choppable { get; set; }

    public ChoppableTouchResult()
    {
        Result = ETouchResult.NoTouch;
    }
}

public class Choppable : MonoBehaviour
{
    private ChoppablePiece[] _pieces;
    public ChoppablePiece[] Pieces
    {
        get
        {
            if (_pieces == null)
                _pieces = GetComponentsInChildren<ChoppablePiece>();

            return _pieces;
        }
    }

    private IChopperReactor[] _reactors;
    private IChopperReactor[] _Reactors
    {
        get
        {
            if (_reactors == null)
                _reactors = GetComponentsInChildren<IChopperReactor>();

            return _reactors;
        }
    }

    public EChopState ChopState { get; private set; }
    public ChoppablePiece CurChopperPiece { get; private set; }

    public bool IsAvailable { get; private set; }

    public Action<EChopState> OnStateChanged { get; set; }
    public Action<bool> OnAvailabilityUpdated { get; set; }

    private void Awake()
    {
        ChopState = EChopState.Idle;

        InitPieces();
    }

    private void InitPieces()
    {
        foreach (ChoppablePiece p in Pieces)
            p.InitPiece(this);
    }

    public bool TryTouchChoppable(IChopperInteractable interactable, ChoppableTouchResult result)
    {
        result.Result = ETouchResult.NoTouch;
        result.Choppable = this;

        if (ChopState != EChopState.Idle
            && ChopState != EChopState.Chopping)
            return false;

        if (interactable is ChoppablePiece)
            ChopperTouched((ChoppablePiece)interactable, result);
        else if(interactable is ChoppableEdge)
            ChopperTouched((ChoppableEdge)interactable, result);

        return true;
    }

    private void ChopperTouched(ChoppablePiece p, ChoppableTouchResult result)
    {
        if (p.ChopState == EChopState.Exiting)
        {
            p.ChopperExited();
            return;
        }

        if (CurChopperPiece == null
            || CurChopperPiece == p)
            ChopFailed(result);
    }

    private void ChopperTouched(ChoppableEdge e, ChoppableTouchResult result)
    {
        if (CurChopperPiece == null
            || CurChopperPiece == e.ParentPiece)
            e.ParentPiece.TryTouchEdge(e, result);
        else
            ChopFailed(result);
    }

    private void ChopFailed(ChoppableTouchResult result)
    {
        result.Result = ETouchResult.Failed;

        SetState(EChopState.Failed);

        Debug.Log("Chop Failed!");
    }

    public void PieceChopping(ChoppablePiece p)
    {
        CurChopperPiece = p;

        SetState(EChopState.Chopping);
    }

    public void PieceChopped(ChoppablePiece p, ChoppableTouchResult result)
    {
        PieceExiting(p);

        result.Result = ETouchResult.ChoppedPiece;
        result.ChoppedPiece = p;

        CheckIfChopped(result);
    }

    public void PieceExiting(ChoppablePiece p)
    {
        CurChopperPiece = null;

        SetState(EChopState.Idle);
    }

    private void CheckIfChopped(ChoppableTouchResult result)
    {
        foreach (ChoppablePiece p in Pieces)
            if (p.ChopState != EChopState.Succeeded)
                return;

        Debug.Log("Choppable Chopped!");

        result.Result = ETouchResult.ChoppedAll;
        result.Choppable = this;

        SetState(EChopState.Succeeded);
    }

    private void SetState(EChopState state)
    {
        if (ChopState == state)
            return;

        ChopState = state;

        OnStateChanged?.Invoke(state);
    }

    public void ResetChoppable()
    {
        ChopState = EChopState.Idle;

        foreach (ChoppablePiece p in Pieces)
            p.ResetPiece();

        CurChopperPiece = null;
    }

    public void SetChoppableActive(bool isAvailable)
    {
        if (IsAvailable == isAvailable)
            return;

        IsAvailable = isAvailable;

        OnAvailabilityUpdated?.Invoke(isAvailable);
    }

    #region Chopper Reactor Methods
    public void ChoppedChoppable(ChopControllerBase chopController)
    {
        IChopperReactor cr = GetReactor(chopController);

        cr.ChoppedChoppable(chopController);
    }

    public void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece)
    {
        IChopperReactor cr = GetReactor(chopController);

        cr.ChoppedPiece(chopController, piece);
    }

    public void ChopFailed(ChopControllerBase chopController)
    {
        IChopperReactor cr = GetReactor(chopController);

        cr.ChopFailed(chopController);
    }

    private IChopperReactor GetReactor(ChopControllerBase chopController)
    {
        return _Reactors.FirstOrDefault(val => val.GetChopControllerType() == chopController.GetType());
    }
    #endregion
}

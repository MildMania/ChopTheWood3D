﻿using UnityEngine;
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
    EnteredPiece,
    ExitingPiece,
    ChoppedPiece,
    Failed,
    DecreasedHealth,
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
    [SerializeField] private int _health;

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
            {
                _reactors = GetComponentsInChildren<IChopperReactor>();

                foreach (IChopperReactor r in _reactors)
                    r.SetParentChoppable(this);
            }

            return _reactors;
        }
    }

    public EChopState ChopState { get; private set; }
    public ChoppablePiece CurChopperPiece { get; private set; }

    public int CurHealth { get; private set; }

    public bool IsAvailable { get; private set; }

    public Action<EChopState> OnStateChanged { get; set; }
    public Action<int> OnHealthUpdated { get; set; }
    public Action<bool> OnAvailabilityUpdated { get; set; }

    private void Awake()
    {
        CurHealth = _health;

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

        if (CurHealth == 0)
        {
            result.Result = ETouchResult.ChoppedPiece;
            result.ChoppedPiece = p;
        }
        else
        {
            result.Result = ETouchResult.ExitingPiece;
            result.ChoppedPiece = p;
        }

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

        CurHealth--;

        OnHealthUpdated?.Invoke(CurHealth);

        if (CurHealth == 0)
        {
            result.Result = ETouchResult.ChoppedAll;
            result.Choppable = this;
            SetState(EChopState.Succeeded);
        }
        else
        {
            result.Result = ETouchResult.DecreasedHealth;
            result.Choppable = this;

            ChopState = EChopState.Idle;

            foreach (ChoppablePiece p in Pieces)
                p.ResetPiece();

            CurChopperPiece = null;
        }

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
        CurHealth = _health;

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

    public int GetIndexOfPiece(ChoppablePiece piece)
    {
        for (int i = 0; i < Pieces.Length; i++)
            if (Pieces[i] == piece)
                return i;

        return -1;
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

    public void ExitedPiece(ChopControllerBase chopController, ChoppablePiece piece)
    {
        IChopperReactor cr = GetReactor(chopController);

        cr.ExitedPiece(chopController, piece);
    }

    public void ChopFailed(ChopControllerBase chopController)
    {
        IChopperReactor cr = GetReactor(chopController);

        cr.ChopFailed(chopController);
    }

    public void DecreasedHealth(ChopControllerBase chopController, ChoppablePiece piece)
    {
        IChopperReactor cr = GetReactor(chopController);

        cr.DecreasedHealth(chopController, piece);
    }

    private IChopperReactor GetReactor(ChopControllerBase chopController)
    {
        return _Reactors.FirstOrDefault(val => val.GetChopControllerType() == chopController.GetType());
    }
    #endregion
}

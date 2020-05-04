using UnityEngine;
using System;

[System.Serializable]
public class PieceLog
{
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
    }

    [SerializeField] private Vector3 _localForward;
    public Vector3 LocalForward
    {
        get
        {
            return _localForward;
        }
    }
}

public class ChoppablePiece : MonoBehaviour, IChopperInteractable
{
    [SerializeField] private ChoppableConnection _connection;

    [SerializeField] private PieceLog[] _pieceLogs;
    public PieceLog[] PieceLogs
    {
        get
        {
            return _pieceLogs;
        }
    }

    public Choppable ParentChoppable { get; private set; }
    public EChopState ChopState { get; private set; }

    public Action<EChopState> OnStateChanged { get; set; }

    public void InitPiece(Choppable choppable)
    {
        ParentChoppable = choppable;

        SetState(EChopState.Idle);

        _connection.InitConnection(this);
    }

    public void ChopFailed()
    {
        SetState(EChopState.Failed);
    }

    public bool TryTouchEdge(ChoppableEdge edge, ChoppableTouchResult result)
    {
        if (edge.ChopState != EChopState.Idle)
            return false;

        if (ChopState == EChopState.Idle
            || ChopState == EChopState.Chopping)
        {
            if (_connection.Type == ChoppableConnection.EType.TwoWay)
            {
                ChoppableEdge other = _connection.Edge1;

                if (_connection.Edge1 == edge)
                    other = _connection.Edge2;

                edge.Chopped();
                SetState(EChopState.Chopping);
                ParentChoppable.PieceChopping(this);

                if (other.ChopState == EChopState.Succeeded)
                    CheckPieceChopped(result);
                else
                    result.Result = ETouchResult.EnteredPiece;

                return true;
            }
            else if(_connection.Type == ChoppableConnection.EType.OneWay)
            {
                if (_connection.Edge1 == edge)
                {
                    edge.Chopped();
                    SetState(EChopState.Chopping);
                    result.Result = ETouchResult.EnteredPiece;
                    ParentChoppable.PieceChopping(this);

                    return true;
                }
                else if (_connection.Edge2 == edge
                    && _connection.Edge1.ChopState == EChopState.Succeeded)
                {
                    edge.Chopped();
                    ParentChoppable.PieceChopping(this);

                    CheckPieceChopped(result);

                    return true;
                }
                else
                {
                    ParentChoppable.TryTouchChoppable(this, result);

                    return false;
                }
            }
        }

        return false;
    }

    public void ChopperExited()
    {
        SetState(EChopState.Idle);

        Debug.Log("Exited piece");
    }

    private void CheckPieceChopped(ChoppableTouchResult result)
    {
        SetState(EChopState.Succeeded);

        ParentChoppable.PieceChopped(this, result);
    }

    private void SetState(EChopState state)
    {
        if (ChopState == state)
            return;

        ChopState = state;

        OnStateChanged?.Invoke(state);
    }

    public void ResetPiece()
    {
        ChopState = EChopState.Idle;

        _connection.ResetConnection();
    }
}

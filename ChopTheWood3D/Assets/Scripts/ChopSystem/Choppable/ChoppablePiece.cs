using UnityEngine;
using System;

public class ChoppablePiece : MonoBehaviour, IChopperInteractable
{
    [SerializeField] private ChoppableConnection _connection;
    [SerializeField] private int _health;

    public Choppable ParentChoppable { get; private set; }
    public int CurHealth { get; private set; }
    public EChopState ChopState { get; private set; }

    public Action<EChopState> OnStateChanged { get; set; }
    public Action<int> OnHealthUpdated { get; set; }

    public void InitPiece(Choppable choppable)
    {
        CurHealth = _health;

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
                    result.Result = ETouchResult.Entered;

                return true;
            }
            else if(_connection.Type == ChoppableConnection.EType.OneWay)
            {
                if (_connection.Edge1 == edge)
                {
                    edge.Chopped();
                    SetState(EChopState.Chopping);
                    result.Result = ETouchResult.Entered;
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
        CurHealth--;

        OnHealthUpdated?.Invoke(CurHealth);

        if (CurHealth == 0)
        {
            SetState(EChopState.Succeeded);

            ParentChoppable.PieceChopped(this, result);
        }
        else
        {
            _connection.ResetConnection();

            result.Result = ETouchResult.Exiting;

            SetState(EChopState.Exiting);
        }
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

        CurHealth = _health;

        _connection.ResetConnection();
    }
}

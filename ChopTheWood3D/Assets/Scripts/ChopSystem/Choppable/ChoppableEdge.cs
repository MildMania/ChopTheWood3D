using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableEdge : MonoBehaviour, IChopperInteractable
{
    [SerializeField] private Collider _collider;

    public ChoppablePiece ParentPiece { get; private set; }

    public EChopState ChopState { get; private set; }
    public Choppable ParentChoppable 
    {
        get 
        { 
            return ParentPiece.ParentChoppable; 
        }
    }

    public void InitEdge(ChoppablePiece piece)
    {
        ParentPiece = piece;

        ChopState = EChopState.Idle;
    }

    public void Chopped()
    {
        ChopState = EChopState.Succeeded;

        _collider.enabled = false;

        Debug.Log("Edge Chopped: " + gameObject.name);
    }

    public void ResetEdge()
    {
        _collider.enabled = true;

        ChopState = EChopState.Idle;
    }
}

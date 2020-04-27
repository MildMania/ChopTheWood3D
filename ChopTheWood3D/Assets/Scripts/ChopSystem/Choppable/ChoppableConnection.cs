using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChoppableConnection
{
    public enum EType
    {
        TwoWay,
        OneWay,
    }

    [SerializeField] private EType _type;
    public EType Type
    {
        get
        {
            return _type;
        }
    }

    [SerializeField] private ChoppableEdge _edge1;
    public ChoppableEdge Edge1
    {
        get
        {
            return _edge1;
        }
    }

    [SerializeField] private ChoppableEdge _edge2;
    public ChoppableEdge Edge2
    {
        get
        {
            return _edge2;
        }
    }

    public void InitConnection(ChoppablePiece piece)
    {
        Edge1.InitEdge(piece);
        Edge2.InitEdge(piece);
    }

    public void ResetConnection()
    {
        Edge1.ResetEdge();
        Edge2.ResetEdge();
    }
}

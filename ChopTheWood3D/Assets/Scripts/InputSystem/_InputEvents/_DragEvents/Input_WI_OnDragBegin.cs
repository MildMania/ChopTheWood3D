using UnityEngine;
using System.Collections;

public class Input_WI_OnDragBegin : InputEvent
{
    public int FingerIndex{get;private set;}
    public Vector2 FingerPos{get;private set;}
    public Vector2 DeltaMove{get;private set;}

    public Input_WI_OnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 deltaMove)
    {
        FingerIndex = fingerIndex;
        FingerPos = fingerPos;
        DeltaMove = deltaMove;
    }
}

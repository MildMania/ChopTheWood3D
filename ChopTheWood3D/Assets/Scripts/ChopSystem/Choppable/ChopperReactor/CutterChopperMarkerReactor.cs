using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterChopperMarkerReactor : ChopperReactorBase<CutterChopController>
{
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] [Range(0.0f, 1.0f)] private int _targetAlpha;
    [SerializeField] private int _duration;

    private MaterialPropertyBlock[] _mpbArr;

    private const string TINT_COLOR_PROPERTY = "_TintColor";

    private void Awake()
    {
        InitMPBArray();
    }

    private void InitMPBArray()
    {
        _mpbArr = new MaterialPropertyBlock[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _mpbArr[i] = new MaterialPropertyBlock();

            _renderers[i].GetPropertyBlock(_mpbArr[i]);
        }
    }

    public override void ChoppedChoppable(ChopControllerBase chopController)
    {
        FadeOutPieces();
    }

    private void FadeOutPieces()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].GetPropertyBlock(_mpbArr[i]);

            //for (int pIndex = 0; pIndex < Parent.Pieces.Length; pIndex++)
            //    _mpbArr[i].SetFloat(_piecePropertyArr[pIndex], 1);

            //_mpbArr[i].SetColor(MASK_COLOR_PROPERTY, _choppableChopColor);

            _renderers[i].SetPropertyBlock(_mpbArr[i]);
        }
    }

    public override void ChoppedPiece(ChopControllerBase chopController, ChoppablePiece piece)
    {
    }

    public override void ExitedPiece(ChopControllerBase chopController, ChoppablePiece piece)
    {
    }

    public override void ChopFailed(ChopControllerBase chopController)
    {
    }

    public override void DecreasedHealth(ChopControllerBase chopController, ChoppablePiece piece)
    {
    }
}

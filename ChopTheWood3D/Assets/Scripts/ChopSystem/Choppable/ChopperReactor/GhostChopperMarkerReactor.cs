using UnityEngine;

public class GhostChopperMarkerReactor : ChopperReactorBase<GhostChopController>
{
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private ParticleSystem _failEffect;

    [SerializeField] private Color _pieceChopColor;
    [SerializeField] private Color _choppableChopColor;

    private MaterialPropertyBlock[] _mpbArr;

    private const string MASK_COLOR_PROPERTY = "_MaskColor";

    private readonly string[] _piecePropertyArr = new string[4]
    {
        "_RedMaskCoef",
        "_GreenMaskCoef",
        "_BlueMaskCoef",
        "_AlphaMaskCoef"
    };

    private void Awake()
    {
        InitMPBArray();
    }

    private void InitMPBArray()
    {
        _mpbArr = new MaterialPropertyBlock[_renderers.Length];

        for(int i = 0; i < _renderers.Length; i++)
        {
            _mpbArr[i] = new MaterialPropertyBlock();

            _renderers[i].GetPropertyBlock(_mpbArr[i]);
        }
    }

    public override void ChoppedChoppable(ChopControllerBase chopController)
    {
        MarkChoppable();

        Parent.ResetChoppable();
    }

    public override void ChoppedPiece(
        ChopControllerBase chopController,
        ChoppablePiece piece)
    {
        MarkPiece(piece);
    }

    public override void ExitedPiece(
        ChopControllerBase chopController,
        ChoppablePiece piece)
    {
        MarkPiece(piece);
    }

    public override void ChopFailed(ChopControllerBase chopController)
    {
        InstantiateAndPlayParticle(_failEffect);
    }

    private void InstantiateAndPlayParticle(ParticleSystem particle)
    {
        ParticleSystem effectInstance = Instantiate(particle);
        effectInstance.transform.position = transform.position;
        effectInstance.Play();
    }

    public override void DecreasedHealth(ChopControllerBase chopController, ChoppablePiece piece)
    {
        MarkChoppable();
    }

    private void MarkPiece(ChoppablePiece piece)
    {
        int pieceIndex = Parent.GetIndexOfPiece(piece);

        string propertyName = _piecePropertyArr[pieceIndex];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].GetPropertyBlock(_mpbArr[i]);

            _mpbArr[i].SetFloat(propertyName, 1);
            _mpbArr[i].SetColor(MASK_COLOR_PROPERTY, _pieceChopColor);

            _renderers[i].SetPropertyBlock(_mpbArr[i]);
        }
    }

    private void MarkChoppable()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].GetPropertyBlock(_mpbArr[i]);

            for (int pIndex = 0; pIndex < Parent.Pieces.Length; pIndex++)
                _mpbArr[i].SetFloat(_piecePropertyArr[pIndex], 1);

            _mpbArr[i].SetColor(MASK_COLOR_PROPERTY, _choppableChopColor);

            _renderers[i].SetPropertyBlock(_mpbArr[i]);
        }
    }
}

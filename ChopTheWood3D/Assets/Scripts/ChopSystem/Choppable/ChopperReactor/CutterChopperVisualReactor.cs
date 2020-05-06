using System.Collections;
using UnityEngine;

public class CutterChopperVisualReactor : ChopperReactorBase<CutterChopController>
{
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private ParticleSystem _cartoonCutEffect;
    [SerializeField] private ParticleSystem _cutDebrisEffect;
    [SerializeField] private float _zOffset;
    [SerializeField] private float _delay;
    [SerializeField] private float _duration;

    private MaterialPropertyBlock[] _mpbArr;
    private const string TINT_COLOR_PROPERTY = "_TintColor";
    private IEnumerator _fadeoutRoutine;

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
        InstantiateAndPlayParticle(_cartoonCutEffect);
        InstantiateAndPlayParticle(_cutDebrisEffect);

        FadeOutPieces();
    }

    private void InstantiateAndPlayParticle(ParticleSystem particle)
    {
        ParticleSystem effectInstance = Instantiate(particle);
        Vector3 pos = transform.position;
        pos.z += _zOffset;
        effectInstance.transform.position = pos;

        effectInstance.Play();
    }

    private void FadeOutPieces()
    {
        if (_fadeoutRoutine != null)
            StopCoroutine(_fadeoutRoutine);

        _fadeoutRoutine = FadeoutRoutine();
        StartCoroutine(_fadeoutRoutine);
    }

    private IEnumerator FadeoutRoutine()
    {
        yield return new WaitForSecondsRealtime(_delay);

        Color targetColor = Color.white;

        float remDur = _duration;

        while (remDur >= 0.0f)
        {
            targetColor.a = remDur / _duration;

            remDur -= Time.unscaledDeltaTime;

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].GetPropertyBlock(_mpbArr[i]);

                _mpbArr[i].SetColor(TINT_COLOR_PROPERTY, targetColor);
                
                _renderers[i].SetPropertyBlock(_mpbArr[i]);
            }

            yield return null;
        }

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].GetPropertyBlock(_mpbArr[i]);

            _mpbArr[i].SetColor(TINT_COLOR_PROPERTY, new Color(1, 1, 1, 0));

            _renderers[i].SetPropertyBlock(_mpbArr[i]);
        }

        _fadeoutRoutine = null;
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

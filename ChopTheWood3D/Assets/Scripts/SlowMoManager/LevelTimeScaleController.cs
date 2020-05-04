using System.Collections.Generic;
using UnityEngine;

public class LevelTimeScaleController : MonoBehaviour
{
    public enum ETimeScale
    {
        Default,
        Quarter,
        Half,
        x2,
        x3
    }

    private static LevelTimeScaleController _instance;
    public static LevelTimeScaleController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelTimeScaleController>();

            return _instance;
        }
    }


    [SerializeField] private ETimeScale _levelTimeScale;

    public float LevelTimeScaleCoef
    {
        get
        {
            return _timeScaleDict[_levelTimeScale];
        }
    }

    private ETimeScale _curTimeScale;

    private Dictionary<ETimeScale, float> _timeScaleDict = new Dictionary<ETimeScale, float>
    {
        {ETimeScale.Default, 1.0f},
        {ETimeScale.Quarter, 0.25f},
        {ETimeScale.Half, 0.5f},
        {ETimeScale.x2, 2.0f},
        {ETimeScale.x3, 3.0f},
    };

    public float CurTimeScaleCoef
    {
        get
        {
            return _timeScaleDict[_curTimeScale];
        }
    }

    private void Awake()
    {
        _curTimeScale = ETimeScale.Default;

        RegisterToPhaseNode();
    }

    private void OnDestroy()
    {
        UnregisterFromPhaseNode();
    }

    private void RegisterToPhaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseFinished;
    }

    private void UnregisterFromPhaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseFinished;
    }

    private void OnPhaseStarted(PhaseBaseNode phaseNode)
    {
        if (phaseNode is GhostCutPhase)
            InitTimeScale();
        else if (phaseNode is LevelEndPhase)
        {
            _curTimeScale = ETimeScale.Default;
            Time.timeScale = 1;
        }
    }

    private void OnPhaseFinished(PhaseBaseNode phaseNode)
    {

    }

    private void InitTimeScale()
    {
        _curTimeScale = _levelTimeScale;
        Time.timeScale = _timeScaleDict[_curTimeScale];

        Debug.Log("Init Time Scale:" + Time.timeScale);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = _timeScaleDict[_curTimeScale] * timeScale;

        Debug.Log("TimeScale: " + Time.timeScale);
    }
}

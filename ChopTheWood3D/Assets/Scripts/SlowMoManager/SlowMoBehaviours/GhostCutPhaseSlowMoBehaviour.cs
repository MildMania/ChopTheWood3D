using UnityEngine;

public class GhostCutPhaseSlowMoBehaviour : MonoBehaviour
{
    [SerializeField] private AnimationCurve _slowMoStartCurve;
    [SerializeField] private AnimationCurve _slowMoFinishCurve;

    private void Awake()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseFinished;
    }

    private void OnDestroy()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseFinished;
    }

    private void OnPhaseStarted(PhaseBaseNode phase)
    {
        if (phase is GhostCutPhase)
            StartSlowMo();
    }

    private void OnPhaseFinished(PhaseBaseNode phase)
    {
        if (phase is ChopperCutPhase)
            FinishSlowMo();
    }

    private void StartSlowMo()
    {
        SlowMoManager.Instance.StartSlowMo(_slowMoStartCurve);
    }

    private void FinishSlowMo()
    {
        SlowMoManager.Instance.StartSlowMo(_slowMoFinishCurve);
    }
}

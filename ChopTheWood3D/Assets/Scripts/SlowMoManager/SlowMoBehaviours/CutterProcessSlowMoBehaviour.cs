using UnityEngine;

public class CutterProcessSlowMoBehaviour : MonoBehaviour
{
    [SerializeField] private GhostChopController _ghostChopController;
    [SerializeField] private CutterChopController _cutterChopController;

    [SerializeField] private AnimationCurve _slowMoStartCurve;
    [SerializeField] private AnimationCurve _slowMoFinishCurve;

    private void Awake()
    {
        _ghostChopController.OnStartedChopping += OnChopStarted;
        _cutterChopController.OnStoppedChopping += OnChopStopped;
    }

    private void OnDestroy()
    {
        _ghostChopController.OnStartedChopping -= OnChopStarted;
        _cutterChopController.OnStoppedChopping -= OnChopStopped;
    }

    private void OnChopStarted()
    {
        StartSlowMo();
    }

    private void OnChopStopped()
    {
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

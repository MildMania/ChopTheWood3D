using UnityEngine;

public class LogThrowPhase : PhaseActionNode
{
    public LogThrowPhase(int id)
        : base(id)
    {
    }

    protected override void ProcessFlow()
    {
        WorldInputManager.OnFingerDown += OnFingerDown;
    }

    private void OnFingerDown(int arg1, Vector2 arg2)
    {
        TraverseCompleted();
    }

    protected override void TraverseCompletedCustomActions()
    {
        WorldInputManager.OnFingerDown -= OnFingerDown;
    }
}

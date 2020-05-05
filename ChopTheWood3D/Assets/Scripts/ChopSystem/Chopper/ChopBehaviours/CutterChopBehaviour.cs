public class CutterChopBehaviour : ChopBehaviourBase
{

    public void Move(RecordingData recordingData)
    {
        TryTouchInteractable(recordingData);
    }

    private bool TryTouchInteractable(RecordingData recordingData)
    {
        IChopperInteractable interactable = recordingData.InteractedInteractable;

        if (interactable == null)
            return false;

        if (TryTouchInteractable(interactable, out ChoppableTouchResult result))
            return true;

        return false;
    }
}

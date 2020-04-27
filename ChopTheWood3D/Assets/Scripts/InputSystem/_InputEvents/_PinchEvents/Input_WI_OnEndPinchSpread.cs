public class Input_WI_OnEndPinchSpread : InputEvent
{
    public int FingerIndex { get; private set; }

    public Input_WI_OnEndPinchSpread(int fingerIndex)
    {
        FingerIndex = fingerIndex;
    }
}

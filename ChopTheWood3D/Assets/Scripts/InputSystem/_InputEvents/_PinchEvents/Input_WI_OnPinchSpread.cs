public class Input_WI_OnPinchSpread : InputEvent
{
    public WorldInputManager.PinchSpreadData PinchData { get; private set; }

    public Input_WI_OnPinchSpread(WorldInputManager.PinchSpreadData pinchData)
    {
        PinchData = pinchData;
    }
}
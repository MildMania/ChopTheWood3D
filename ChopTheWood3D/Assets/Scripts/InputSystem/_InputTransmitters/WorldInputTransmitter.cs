public class WorldInputTransmitter : WorldInputTransmitterBase
{
    public WorldInputTransmitter()
    {
    }

    protected override InputControllerBase GetInputController()
    {
        return WorldInputController.Instance;
    }
}

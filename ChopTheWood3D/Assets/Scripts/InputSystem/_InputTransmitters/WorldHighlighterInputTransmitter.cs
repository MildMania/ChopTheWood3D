public class WorldHighlighterInputTransmitter : WorldInputTransmitterBase
{
    public WorldHighlighterInputTransmitter()
    {
    }

    protected override InputControllerBase GetInputController()
    {
        return WorldHighlighterInputController.Instance;
    }
}

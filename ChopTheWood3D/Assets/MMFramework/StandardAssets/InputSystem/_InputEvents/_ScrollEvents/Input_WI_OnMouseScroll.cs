public class Input_WI_OnMouseScroll : InputEvent
{
    public float ScrollAmount { get; private set; }

    public Input_WI_OnMouseScroll(float scrollAmount)
    {
        ScrollAmount = scrollAmount;
    }
}

using UnityEngine.EventSystems;

public interface IUIInputDrag : IEventSystemHandler
{
    void OnDrag(Input_UI_OnDrag eventData);
}

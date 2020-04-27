using UnityEngine.EventSystems;

public interface IUIInputPointerDown : IEventSystemHandler
{
    void OnPointerDown(Input_UI_OnPointerDown eventData);
}

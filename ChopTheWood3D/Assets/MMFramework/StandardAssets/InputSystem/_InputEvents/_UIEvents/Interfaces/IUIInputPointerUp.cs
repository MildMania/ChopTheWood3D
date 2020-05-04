using UnityEngine.EventSystems;

public interface IUIInputPointerUp : IEventSystemHandler
{
    void OnPointerUp(Input_UI_OnPointerUp eventData);
}

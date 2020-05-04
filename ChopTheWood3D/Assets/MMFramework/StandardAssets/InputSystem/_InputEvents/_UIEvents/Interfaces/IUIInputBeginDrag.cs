using UnityEngine.EventSystems;

public interface IUIInputBeginDrag : IEventSystemHandler
{
    void OnBeginDrag(Input_UI_OnBeginDrag eventData);
}

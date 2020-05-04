using UnityEngine.EventSystems;

public interface IUIInputEndDrag : IEventSystemHandler
{
    void OnEndDrag(Input_UI_OnEndDrag eventData);
}

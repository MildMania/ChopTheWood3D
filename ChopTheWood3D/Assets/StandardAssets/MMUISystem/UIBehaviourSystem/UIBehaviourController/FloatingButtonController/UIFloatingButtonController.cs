using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class UIFloatingButtonController : UIBehaviourControllerBase<UIFloatingButtonBehaviour>
{
    public int MaxOpenFloatingButton;

    public Queue<UIFloatingButtonBehaviour> FloatingButtonQueue { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        FloatingButtonQueue = new Queue<UIFloatingButtonBehaviour>();

        ResetAllContainers();
    }

    public override void OnSubContainerPressDown(UIFloatingButtonBehaviour interactedContainer, PointerEventData eventData)
    {
        if(interactedContainer.IsActive)
        {
            if(FloatingButtonQueue.Contains(interactedContainer))
            {
                var queueToList = FloatingButtonQueue.ToList();
                queueToList.Remove(interactedContainer);

                FloatingButtonQueue.Clear();

                queueToList.ForEach(b => FloatingButtonQueue.Enqueue(b));
            }

            interactedContainer.Deactivate();
        }
        else
        {
            if (FloatingButtonQueue.Count > 0 && FloatingButtonQueue.Count == MaxOpenFloatingButton)
            {
                var buttonToClose = FloatingButtonQueue.Dequeue();
                buttonToClose.Deactivate();
            }

            FloatingButtonQueue.Enqueue(interactedContainer);
            interactedContainer.Activate();
        }

        base.OnSubContainerPressDown(interactedContainer, eventData);
    }

    public void CloseAllContainers()
    {
        var queueToList = FloatingButtonQueue.ToList();

        foreach (var container in queueToList)
            OnSubContainerPressDown(container, null);
    }

    public void ResetAllContainers()
    {
        BehaviourList.ForEach(b => b.ResetUI(false));
    }
}

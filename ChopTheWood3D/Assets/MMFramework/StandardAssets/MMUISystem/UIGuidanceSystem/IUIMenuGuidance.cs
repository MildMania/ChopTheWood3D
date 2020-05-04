using System;
using System.Collections.Generic;

public interface IUIMenuGuidance<T> : IUIMenuGuidance
{
    void OnGuideAdded(T trackInfo);
    void OnGuideRemoved(T trackInfo);
}

public interface IUIMenuGuidance
{
    List<UIGuidanceBase> GuidableComponentList { get; set; }

    void OnGuidanceFinished(IConvertible itemType);
}

public static class IUIMenuGuidanceExtensions
{
    public static void RegisterToMenuGuidance(this IUIMenuGuidance targetInterface, UIGuidanceBase targetComponent)
    {
        if (targetInterface.GuidableComponentList == null)
            targetInterface.GuidableComponentList = new List<UIGuidanceBase>();

        if (targetInterface.GuidableComponentList.Contains(targetComponent))
            return;

        targetInterface.GuidableComponentList.Add(targetComponent);

        targetComponent.OnGuidanceFinished += targetInterface.OnGuidanceFinished;
    }

    public static void UnregisterToMenuGuidance(this IUIMenuGuidance targetInterface, UIGuidanceBase targetComponent)
    {
        if (targetInterface == null 
            || targetInterface.GuidableComponentList == null)
            return;

        targetInterface.GuidableComponentList.Remove(targetComponent);

        targetComponent.OnGuidanceFinished -= targetInterface.OnGuidanceFinished;
    }
}

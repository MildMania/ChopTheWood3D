using System;

public class UIInvCardGuidance : UIGuidanceBase
{
    public IConvertible GuidedItemType { get; private set; }

    public void SetGuidance(IConvertible itemType)
    {
        GuidedItemType = itemType;
    }

    public override void SetGuidanceSeen()
    {
        FireOnGuidanceFinished(GuidedItemType);
    }
}

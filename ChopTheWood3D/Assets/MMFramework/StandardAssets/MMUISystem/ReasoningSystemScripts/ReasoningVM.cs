using System;

public class ReasoningVM : VMBase
{
    #region Events
    public Action<ReasoningTextPLD> OnReasonReceived;
    public void FireOnReasonReceived(ReasoningTextPLD pld)
    {
        if (OnReasonReceived != null)
            OnReasonReceived(pld);
    }
    #endregion

    public ReasoningVM()
    {
        StartListeningEvents();
    }

    protected override void DisposeCustomActions()
    {
        FinishListeningEvents();
    }

    public override void StartListeningEvents()
    {
        IUIReasonProviderExtensions.OnUIReasonRaised += OnUIReasonFired;
    }

    public override void FinishListeningEvents()
    {
        IUIReasonProviderExtensions.OnUIReasonRaised -= OnUIReasonFired;
    }

    private void OnUIReasonFired(string reasonText)
    {
        ReasoningTextPLD pld = new ReasoningTextPLD()
        {
            Text = reasonText,
        };

        FireOnReasonReceived(pld);
    }
}

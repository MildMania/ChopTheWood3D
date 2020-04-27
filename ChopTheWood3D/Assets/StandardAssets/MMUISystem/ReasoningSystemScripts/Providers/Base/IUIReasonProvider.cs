using System;

public interface IUIReasonProvider<T>
{
    void RaiseReason(T reasonParam);
}

public static class IUIReasonProviderExtensions
{
    public static Action<string> OnUIReasonRaised;

    public static void FireOnUIReasonRaised<T>(this IUIReasonProvider<T> reasonProvider, string text)
    {
        if (OnUIReasonRaised != null)
            OnUIReasonRaised(text);
    }
}

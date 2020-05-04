using System;

public abstract class EventRaiserBase<T>
    where T : EventArgs
{
    public EventHandler<T> OnRaised { get; set; }
    public static EventHandler<T> OnRaisedStatic { get; set; }

    public void Raise(T args)
    {
        if (OnRaised != null)
            OnRaised(this, args);

        if (OnRaisedStatic != null)
            OnRaisedStatic(this, args);
    }
}

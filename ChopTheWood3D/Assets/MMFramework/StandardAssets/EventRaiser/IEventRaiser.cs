using System;

public interface IEventRaiser<T1, T2>
    where T1 : EventRaiserBase<T2>
    where T2 : EventArgs
{
    T1 Raiser { get; set; }
}

using System.Collections.Generic;
using System;
using System.Linq;

public interface IInputReceiver
{
    List<InputTransmitter> AttachedInputTransmitterList { get; set; }
    Dictionary<Type, InputTransmitter.EventDelegate> Delegates { get; set; }
    Dictionary<Delegate, InputTransmitter.EventDelegate> DelegateLookUp { get; set; }
}

public static class IInputReceiverExtensions
{
    public static void AttachInputTransmitter(this IInputReceiver r, InputTransmitter t)
    {
        if (r.AttachedInputTransmitterList == null)
            r.AttachedInputTransmitterList = new List<InputTransmitter>();

        if (r.AttachedInputTransmitterList.Contains(t))
            return;

        r.AttachedInputTransmitterList.Add(t);
    }

    public static void DeattachInputTransmitter(this IInputReceiver r, InputTransmitter t)
    {
        if (r.AttachedInputTransmitterList == null)
            return;

        r.AttachedInputTransmitterList.Remove(t);

        t.Dispose();
    }

    public static void AddInputListener<T>(this IInputReceiver r, InputTransmitter.EventDelegate<T> callback)
        where T : InputEvent
    {
        if (r.Delegates == null)
            r.Delegates = new Dictionary<Type, InputTransmitter.EventDelegate>();

        if (r.DelegateLookUp == null)
            r.DelegateLookUp = new Dictionary<Delegate, InputTransmitter.EventDelegate>();

        if (r.DelegateLookUp.ContainsKey(callback))
            return;

        InputTransmitter.EventDelegate del = (e) => callback((T)e);
        r.DelegateLookUp[callback] = del;

        InputTransmitter.EventDelegate tempDel;

        if (r.Delegates.TryGetValue(typeof(T), out tempDel))
            r.Delegates[typeof(T)] = tempDel += del;
        else
            r.Delegates.Add(typeof(T), del);

        if (r.AttachedInputTransmitterList == null)
            r.AttachedInputTransmitterList = new List<InputTransmitter>();
    }

    public static void RemoveInputListener<T>(this IInputReceiver r, InputTransmitter.EventDelegate<T> callback)
        where T : InputEvent
    {
        if (r.Delegates == null)
            return;

        InputTransmitter.EventDelegate internalDel;

        if (r.DelegateLookUp.TryGetValue(callback, out internalDel))
        {
            InputTransmitter.EventDelegate tempDel;
            if (r.Delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDel;

                if (tempDel == null)
                    r.Delegates.Remove(typeof(T));
                else
                    r.Delegates[typeof(T)] = tempDel;
            }

            r.DelegateLookUp.Remove(callback);
        }
    }

    public static void SetInputReceivingEnabled(this IInputReceiver r, bool isEnabled, bool cacheCurState = true)
    {
        foreach (InputTransmitter t in r.AttachedInputTransmitterList)
            t.SetTransmissionEnabled(isEnabled, cacheCurState);
    }

    public static void SetInputReceivingEnabled<T>(this IInputReceiver r, bool isEnabled, bool cacheCurState = true)
        where T : InputTransmitter
    {
        T t = (T)r.AttachedInputTransmitterList.SingleOrDefault(val => val is T);

        if (t == null)
            return;

        t.SetTransmissionEnabled(isEnabled, cacheCurState);
    }

    public static void SetInputReceivingToPrevState(this IInputReceiver r)
    {
        foreach (InputTransmitter t in r.AttachedInputTransmitterList)
            t.SetTransmissionStateToPrevState();
    }

    public static void SetInputReceivingToPrevState<T>(this IInputReceiver r)
        where T : InputTransmitter
    {
        T t = (T)r.AttachedInputTransmitterList.SingleOrDefault(val => val is T);

        if (t == null)
            return;

        t.SetTransmissionStateToPrevState();
    }

    public static bool HasAnyEnabledTransmitter(this IInputReceiver r)
    {
        return r.AttachedInputTransmitterList.Any(
            val => val.IsTransmissionEnabled
            && val.InputController.IsInputEnabled);
    }
}
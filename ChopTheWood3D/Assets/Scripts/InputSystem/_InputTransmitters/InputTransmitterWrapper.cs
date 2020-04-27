using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputTransmitterWrapper<T> : MonoBehaviour
    where T : InputTransmitter, new()
{
    Dictionary<IInputReceiver, T> _receiverDict;
    public Dictionary<IInputReceiver, T> ReceiverDict
    {
        get
        {
            if (_receiverDict == null)
                _receiverDict = new Dictionary<IInputReceiver, T>();

            return _receiverDict;
        }
    }

    public static TTransmitterWrapper AttachTransmitterWrapper<TTransmitterWrapper>(GameObject go)
        where TTransmitterWrapper : InputTransmitterWrapper<T>
    {
        TTransmitterWrapper wrapper = go.AddComponent<TTransmitterWrapper>();

        return wrapper;
    }

    public static void DeattachTransmitterWrapper<TTransmitterWrapper>(GameObject go)
        where TTransmitterWrapper : InputTransmitterWrapper<T>
    {
        TTransmitterWrapper wrapper = go.GetComponent<TTransmitterWrapper>();

        if (wrapper == null)
            return;

        foreach (var rKVP in wrapper.ReceiverDict)
            rKVP.Key.DeattachInputTransmitter(rKVP.Value);

        Destroy(wrapper);
    }

    private void Awake()
    {
        InitReceiverDict();
    }

    private void OnDestroy()
    {
        foreach (var rKVP in _receiverDict)
            rKVP.Value.Dispose();
    }

    void InitReceiverDict()
    {
        _receiverDict = new Dictionary<IInputReceiver, T>();

        List<IInputReceiver> irList = GetComponents<IInputReceiver>().ToList();

        foreach(IInputReceiver ir in irList)
        {
            T transmitter = InputTransmitter.AttachInputTransmitter<T>(ir);

            if (transmitter == null)
                return;

            transmitter.SetTransmissionEnabled(true, false);

            _receiverDict.Add(ir, transmitter);
        }
    }
}

using System;
using UnityEngine;

public abstract class DrawerBase<T> : MonoBehaviour where T : IPLDBase
{
    public abstract void ParseData(T pld);
    public abstract void ActivateListeners();
    public abstract void DeactivateListeners();
    public abstract void ResetDrawer();
}

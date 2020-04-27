using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PreDefinedDrawerControllerBase : MonoBehaviour
{
    #region Events
    public Action OnDrawerActivated;
    protected void FireOnDrawerActivated()
    {
        OnDrawerActivated?.Invoke();
    }

    public Action OnDrawerDeactivated;
    protected void FireOnDrawerDeactivated()
    {
        OnDrawerDeactivated?.Invoke();
    }
    #endregion

    protected List<PreDefinedDrawerBase> _drawerList;
    public List<PreDefinedDrawerBase> DrawerList
    {
        get
        {
            if (_drawerList == null)
                _drawerList = new List<PreDefinedDrawerBase>(GetComponentsInChildren<PreDefinedDrawerBase>(true));

            return _drawerList;
        }
    }

    protected PreDefinedDrawerBase GetDrawer(Type type)
    {
        return DrawerList.Find(d => d.GetType().IsAssignableFrom(type));
    }

    protected List<PreDefinedDrawerBase> GetDrawers(Type type)
    {
        return DrawerList.FindAll(d => d.GetType().IsAssignableFrom(type));
    }

    private void Awake()
    {
        DeactivateDrawers();
    }

    public void ActivateDrawerListeners()
    {
        DrawerList.ForEach(d => d.ActivateListeners());
    }

    public void ActivateDrawers()
    {
        DrawerList.ForEach(d => d.Activate());
    }

    public void DeactivateDrawerListeners()
    {
        DrawerList.ForEach(d => DeactivateDrawerListeners(d));
    }

    public void DeactivateDrawers()
    {
        DrawerList.ForEach(d => d.Deactivate());
    }

    public void DeactivateDrawerListeners(PreDefinedDrawerBase drawer)
    {
        drawer.DeactivateListeners();
    }

    public virtual void ActivateListeners()
    {
        ActivateDrawerListeners();
    }

    public virtual void DeactivateListeners()
    {
        DeactivateDrawerListeners();
    }

    public abstract void Activate();
    public abstract void Deactivate();
}

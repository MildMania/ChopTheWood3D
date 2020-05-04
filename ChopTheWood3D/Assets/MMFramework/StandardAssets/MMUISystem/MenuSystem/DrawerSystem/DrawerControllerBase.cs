using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DrawerControllerBase<T> : MonoBehaviour 
    where T : IPLDBase
{
    #region Events
    public Action OnDrawerActivated;
    protected void FireOnDrawerActivated()
    {
        if (OnDrawerActivated != null)
            OnDrawerActivated();
    }

    public Action OnDrawerDeactivated;
    protected void FireOnDrawerDeactivated()
    {
        if (OnDrawerDeactivated != null)
            OnDrawerDeactivated();
    }
    #endregion

    protected List<DrawerBase<T>> _drawerList;
    public List<DrawerBase<T>> DrawerList
    {
        get
        {
            if (_drawerList == null)
                _drawerList = new List<DrawerBase<T>>(GetComponentsInChildren<DrawerBase<T>>(true));

            return _drawerList;
        }
    }

    protected DrawerBase<T> GetDrawer(Type type)
    {
        return DrawerList.Find(d => d.GetType().IsAssignableFrom(type));
    }

    protected List<DrawerBase<T>> GetDrawers(Type type)
    {
        return DrawerList.FindAll(d => d.GetType().IsAssignableFrom(type));
    }

    public virtual void ResetDrawers()
    {
        DrawerList.ForEach(d => d.ResetDrawer());
    }

    public void ActivateDrawerListeners()
    {
        DrawerList.ForEach(d => d.ActivateListeners());
    }

    public void DeactivateDrawerListeners()
    {
        DrawerList.ForEach(d => DeactivateDrawerListeners(d));
    }

    public void DeactivateDrawerListeners(DrawerBase<T> drawer)
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

    public abstract void DistributeData(List<IPLDBase> pldList);
}

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIGuidanceBase : MonoBehaviour
{
    #region Events
    public Action<IConvertible> OnGuidanceFinished;
    protected void FireOnGuidanceFinished(IConvertible itemType)
    {
        if (OnGuidanceFinished != null)
            OnGuidanceFinished(itemType);
    }
    #endregion

    public UIGuidanceBase ParentComponent;
    public bool RegisterToUIMenu;

    protected List<UIGuidanceBase> _ChildComponentList;

    public bool IsActive { get; private set; }

    public virtual void Init()
    {
        if (RegisterToUIMenu)
            RegisterGuidance();

        if (ParentComponent != null)
            ParentComponent.RegisterAsChild(this);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (RegisterToUIMenu)
            UnregisterGuidance();
    }

    private void RegisterAsChild(UIGuidanceBase childComp)
    {
        if (_ChildComponentList == null)
            _ChildComponentList = new List<UIGuidanceBase>();

        _ChildComponentList.Add(childComp);
    }

    public void RegisterGuidance()
    {
        GetComponentInParent<IUIMenuGuidance>().RegisterToMenuGuidance(this);
    }

    public void UnregisterGuidance()
    {
        IUIMenuGuidance mainGuidance = GetComponentInParent<IUIMenuGuidance>();

        mainGuidance.UnregisterToMenuGuidance(this);
    }

    public virtual void ActivateGuidance()
    {
        if (IsActive)
            return;

        IsActive = true;

        gameObject.SetActive(true);

        CheckParentComponentForActivation();
    }

    private void CheckParentComponentForActivation()
    {
        if (ParentComponent == null)
            return;

        ParentComponent.ActivateGuidance();
    }

    public virtual void DeactivateGuidance()
    {
        if (!IsActive || IsAnyChildActive())
            return;

        IsActive = false;

        gameObject.SetActive(false);

        CheckParentComponentForDeactivation();
    }

    private bool IsAnyChildActive()
    {
        if (_ChildComponentList == null || _ChildComponentList.Count == 0)
            return false;

        foreach(UIGuidanceBase childGuidance in _ChildComponentList)
        {
            if(childGuidance.IsActive)
                return true;
        }

        return false;
    }

    private void CheckParentComponentForDeactivation()
    {
        if (ParentComponent == null)
            return;

        ParentComponent.DeactivateGuidance();
    }

    public abstract void SetGuidanceSeen();
}

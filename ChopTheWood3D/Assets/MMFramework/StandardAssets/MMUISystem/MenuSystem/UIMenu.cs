using MMUISystem.UIButton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract partial class UIMenu : MonoBehaviour
{
    #region Events
    public static Action<UIMenu> OnUIMenuInitCompleted;
    public static Action<UIMenu> OnUIMenuDestroyed;
    #endregion

    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath;
    public bool ActivateInAwake;
    public UIAnimSequence UIMenuAnimation;
    public List<UnityUIButton> CloseButtonCollection;

    public bool IsPreActivationFinished { get; protected set; }
    public bool IsPostActivationFinished { get; protected set; }
    public bool IsPreDeactivationFinished { get; protected set; }
    public bool IsPostDeactivationFinished { get; protected set; }

    private Canvas _canvas;
    public Canvas Canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();

            return _canvas;
        }
    }


    public static Action<UIMenu> OnPreActivation, OnPostActivation, OnPreDeactivation, OnPostDeactivation;

    protected ParameterEncapsulation Param { get; private set; }
    protected List<UnityUIButton> UIButtons { get { return new List<UnityUIButton>(GetComponentsInChildren<UnityUIButton>(true)); } }
    protected IEnumerator _deactivateRoutine, _activateRoutine;

    protected virtual IEnumerator PreDeactivateAdditional()
    {
        FinishListeningEvents();

        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostDeactivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PreActivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostActivateAdditional()
    {
        StartListeningEvents();

        yield return new WaitForEndOfFrame();
    }

    protected virtual void Awake()
    {
        Init();

        if (ActivateInAwake)
            Activate(null);
    }

    protected virtual void OnDestroy()
    {
        OnUIMenuDestroyed?.Invoke(this);
    }

    protected void Init()
    {
        gameObject.SetActive(false);

        IsPreDeactivationFinished = true;
        IsPostDeactivationFinished = true;

        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        OnUIMenuInitCompleted?.Invoke(this);
    }

    #region Activation / Deactivation
    public virtual void Activate(ParameterEncapsulation param)
    {
        gameObject.SetActive(true);

        Param = param;

        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        _activateRoutine = ActivateRoutine();
        StartCoroutine(_activateRoutine);
    }

    IEnumerator ActivateRoutine()
    {
        IsPreDeactivationFinished = false;
        IsPostDeactivationFinished = false;

        yield return StartCoroutine(PreActivateAdditional());
        IsPreActivationFinished = true;

        OnPreActivation?.Invoke(this);

        if (UIMenuAnimation != null)
        {
            UIMenuAnimation.ResetSequence();

            UIMenuAnimation.PlayIntroSequence(OnMenuIntroAnimFinished);
        }
        else
            OnMenuIntroAnimFinished();
    }

    private void OnMenuIntroAnimFinished()
    {
        StartCoroutine(PostActivateAdditional());

        IsPostActivationFinished = true;

        OnPostActivation?.Invoke(this);
    }

    public virtual void Deactivate()
    {
        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        _deactivateRoutine = DeactivateRoutine();
        StartCoroutine(_deactivateRoutine);
    }

    IEnumerator DeactivateRoutine()
    {
        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        yield return StartCoroutine(PreDeactivateAdditional());
        IsPreDeactivationFinished = true;

        OnPreDeactivation?.Invoke(this);

        if (UIMenuAnimation != null)
            UIMenuAnimation.PlayOutroSequence(OnMenuOutroAnimFinished);
        else
            OnMenuOutroAnimFinished();
    }

    private void OnMenuOutroAnimFinished()
    {
        if (UIMenuAnimation != null)
            UIMenuAnimation.ResetSequence();

        StartCoroutine(PostDeactivateAdditional());
        IsPostDeactivationFinished = true;

        gameObject.SetActive(false);

        OnPostDeactivation?.Invoke(this);
    }
    #endregion

    public abstract void OnBackPressed(PointerEventData eventData);
    protected abstract void StartListeningEvents();
    protected abstract void FinishListeningEvents();
}

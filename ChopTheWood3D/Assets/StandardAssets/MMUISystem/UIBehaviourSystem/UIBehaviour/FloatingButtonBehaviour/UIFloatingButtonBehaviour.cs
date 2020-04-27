using System.Collections.Generic;

public class UIFloatingButtonBehaviour : UIBehaviourBase<UIFloatingButtonBehaviour>
{
    public List<MMUITweener> TweenerList;

    protected override void Awake()
    {
        base.Awake();

        TweenerList.ForEach(t => t.AddOnFinish(OnFloatingButtonTweenFinished));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        TweenerList.ForEach(t => t.ResetEventDelegates());
    }

    private void OnFloatingButtonTweenFinished()
    {
        FireOnTweenFinished(IsActive);
    }

    public override void Activate(params object[] parameters)
    {
        StartTransition(true, parameters);
    }

    public override void Deactivate(params object[] parameters)
    {
        StartTransition(false, parameters);
    }

    public override void ResetUI(bool isActivate, params object[] parameters)
    {
        IsActive = isActivate;

        TweenerList.ForEach(t => t.KillTween());

        if(IsActive)
            TweenerList.ForEach(t => t.InitValueToTO());
        else
            TweenerList.ForEach(t => t.InitValueToFROM());
    }

    protected override void HideContent()
    {
    }

    protected override void ShowContent()
    {
    }

    //parameters: float duration, MMTweeningEaseEnum ease
    protected override void StartTransition(bool isActivate, params object[] parameters)
    {
        IsActive = isActivate;

        if(IsActive)
            TweenerList.ForEach(t => t.PlayForward());
        else
            TweenerList.ForEach(t => t.PlayReverse());
    }
}

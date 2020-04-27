using System.Collections.Generic;
using TMPro;

public class ReasoningTextDrawer : DrawerBase<ReasoningTextPLD>
{
    public TextMeshProUGUI Text;
    public MMTweenAlpha IntroTween;
    public MMTweenAlpha OutroTween;

    public bool IsAvail { get; private set; }

    public override void ActivateListeners()
    {
        IntroTween.AddOnFinish(OnIntroTweenFinished);
        OutroTween.AddOnFinish(OnOutroTweenFinished);
    }

    public override void DeactivateListeners()
    {
        IntroTween.ResetEventDelegates();
        OutroTween.ResetEventDelegates();
    }

    private void OnIntroTweenFinished()
    {
        OutroTween.InitValueToFROM();

        OutroTween.PlayForward();
    }

    private void OnOutroTweenFinished()
    {
        CloseDrawer();
    }

    private void CloseDrawer()
    {
        gameObject.SetActive(false);

        Text.SetText("");

        IsAvail = true;

        IntroTween.KillTween();
        OutroTween.KillTween();

        IntroTween.InitValueToFROM();
    }

    public override void ParseData(ReasoningTextPLD pld)
    {
        IsAvail = false;

        Text.SetText(pld.Text);

        gameObject.SetActive(true);

        IntroTween.PlayForward();
    }

    public override void ResetDrawer()
    {
        CloseDrawer();
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextUnlockDrawerPLD : IPLDBase
{
    public int Percentage { get; private set; }
    public bool IsInstant { get; private set; }
    public Color NextColor { get; private set; }

    public NextUnlockDrawerPLD(Color nextColor, int perc, bool isInstant)
    {
        NextColor = nextColor;

        Percentage = perc;

        IsInstant = isInstant;
    }
}

public class NextUnlockDrawer : DrawerBase<NextUnlockDrawerPLD>
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private Image _nextUnlockImage;
    [SerializeField] private MMTweenAlpha _alphaTween;
    [SerializeField] private MMTweenScale _scaleTween;
    [SerializeField] private AdvancedFillBarScript _fillbar;

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public override void ParseData(NextUnlockDrawerPLD pld)
    {
        if(_header != null)
            _header.SetText("Next Unlock");

        _nextUnlockImage.color = pld.NextColor;
        _nextUnlockImage.gameObject.SetActive(false);

        if (pld.Percentage == 100)
            _fillbar.OnBarUpdateComplete += RevealUnlocked;

        _fillbar.UpdateBar(pld.Percentage, pld.IsInstant);
    }
    private void RevealUnlocked()
    {
        _fillbar.OnBarUpdateComplete -= RevealUnlocked;

        _header.SetText("Unlocked!");

        _nextUnlockImage.gameObject.SetActive(true);

        PlayUnlockAnim();
    }

    private void ReverseScaleTween()
    {
        _scaleTween.PlayReverse();
    }

    public override void ResetDrawer()
    {
    }

    public void PlayUnlockAnim()
    {
        _alphaTween.PlayForward();
        _scaleTween.AddOnFinish(ReverseScaleTween, false).PlayForward();
    }
}
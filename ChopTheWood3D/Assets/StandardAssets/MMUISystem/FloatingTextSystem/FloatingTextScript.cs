using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public UIMenuTweenBehaviour TweenBehaviour;

    private Action<FloatingTextScript> _onFloatingFinished;
    private void FireOnFloatingFinished()
    {
        _onFloatingFinished?.Invoke(this);
    }

    public virtual void ResetText()
    {
        Text.SetText("");

        TweenBehaviour.ResetAnim();
    }

    public virtual void ActivateText(FloatingTextArgs args, Action<FloatingTextScript> onFloatingFinished)
    {
        ResetText();

        _onFloatingFinished = onFloatingFinished;

        string textToWrite = args.Text;
        if (args.IsRichText)
            textToWrite = Regex.Unescape(textToWrite);

        Text.SetText(textToWrite);

        gameObject.SetActive(true);

        TweenBehaviour.PlayIntro(OnFloatingFinished);
    }

    public virtual void DeactivateText()
    {
        ResetText();

        gameObject.SetActive(false);
    }

    private void OnFloatingFinished()
    {
        FireOnFloatingFinished();
    }
}

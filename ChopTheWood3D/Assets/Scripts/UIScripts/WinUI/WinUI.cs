using MMUISystem.UIButton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinUI : UIMenu<WinUI>
{
    [SerializeField] private NextUnlockDrawerController _nextUnlockDrawerController;
    [SerializeField] private SessionCoinDrawerController _sessionCoinDrawerController;

    [SerializeField] private UnityUIButton _tapToContinueButton;
    [SerializeField] private MMTweenAlpha _tapToContinueButton_AlphaTween;
    [SerializeField] private MMTweenPosition _tapToContinueButton_PosTween;

    [SerializeField] private UnityUIButton _unlockButton;
    [SerializeField] private MMTweenAlpha _unlockButton_AlphaTween;
    [SerializeField] private MMTweenPosition _unlockButton_PosTween;

    private WinVM _viewModel;

    protected override void Awake()
    {
        _viewModel = new WinVM();

        _viewModel.InitViewModel();

        base.Awake();
    }

    protected override void OnDestroy()
    {
        _viewModel.Dispose();

        _viewModel = null;

        base.OnDestroy();
    }

    public override void OnBackPressed(PointerEventData eventData)
    {
    }

    protected override void FinishListeningEvents()
    {
        _viewModel.FinishListeningEvents();

        _nextUnlockDrawerController.DeactivateListeners();
        _sessionCoinDrawerController.DeactivateListeners();

        UIButtons.ForEach(b => b.StopListening());

        _tapToContinueButton.OnButtonPressedUp -= OnContinuePressed;
        _tapToContinueButton.OnButtonTapped -= OnContinuePressed;

        _unlockButton.OnButtonPressedUp -= OnUnlockPressed;
        _unlockButton.OnButtonTapped -= OnUnlockPressed;
    }

    protected override void StartListeningEvents()
    {
        _viewModel.StartListeningEvents();

        _nextUnlockDrawerController.ActivateListeners();
        _sessionCoinDrawerController.ActivateListeners();

        UIButtons.ForEach(b => b.StartListening());

        _tapToContinueButton.OnButtonPressedUp += OnContinuePressed;
        _tapToContinueButton.OnButtonTapped += OnContinuePressed;

        _unlockButton.OnButtonPressedUp += OnUnlockPressed;
        _unlockButton.OnButtonTapped += OnUnlockPressed;
    }

    protected override IEnumerator PreActivateAdditional()
    {
        ToggleUnlockButton(false);
        ToggleContinueButton(false);

        _nextUnlockDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetPrevNextUnlockPLD() });

        return base.PreActivateAdditional();
    }

    protected override IEnumerator PostActivateAdditional()
    {
        NextUnlockDrawerPLD pld = (NextUnlockDrawerPLD)_viewModel.GetNextUnlockPLD();
        _nextUnlockDrawerController.DistributeData(new List<IPLDBase>() { pld });

        if (pld.Percentage == 100)
            ToggleUnlockButton(true);
        else
            ToggleContinueButton(true);

        _sessionCoinDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetSessionCoinPLD() });

        return base.PostActivateAdditional();
    }

    private void ToggleUnlockButton(bool isActive)
    {
        _unlockButton.ToggleButton(isActive);

        if (isActive)
        {
            _unlockButton_PosTween.PlayForward();
            _unlockButton_AlphaTween.PlayForward();
        }
        else
        {
            _unlockButton_PosTween.PlayReverse();
            _unlockButton_AlphaTween.PlayReverse();
        }
    }

    private void ToggleContinueButton(bool isActive)
    {
        _tapToContinueButton.ToggleButton(isActive);

        if (isActive)
        {
            _tapToContinueButton_PosTween.PlayForward();
            _tapToContinueButton_AlphaTween.PlayForward();
        }
        else
        {
            _tapToContinueButton_PosTween.PlayReverse();
            _tapToContinueButton_AlphaTween.PlayReverse();
        }
    }

    private void OnContinuePressed(PointerEventData eventData)
    {
        ToggleContinueButton(false);

        _viewModel.ContinuePressed();
    }

    private void OnUnlockPressed(PointerEventData eventData)
    {
        _nextUnlockDrawerController.PlayUnlockAnim();

        ToggleUnlockButton(false);

        ToggleContinueButton(true);
    }
}

using MMUISystem.UIButton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoseUI : UIMenu<LoseUI>
{
    //[SerializeField] private NextUnlockDrawerController _nextUnlockDrawerController;
    [SerializeField] private SessionCoinDrawerController _sessionCoinDrawerController;
    [SerializeField] private UnityUIButton _retryButton;

    private LoseVM _viewModel;

    protected override void Awake()
    {
        _viewModel = new LoseVM();

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

        //_nextUnlockDrawerController.DeactivateListeners();
        _sessionCoinDrawerController.DeactivateListeners();

        UIButtons.ForEach(b => b.StopListening());

        _retryButton.OnButtonPressedUp -= OnRetryPressed;
        _retryButton.OnButtonTapped -= OnRetryPressed;
    }

    protected override void StartListeningEvents()
    {
        _viewModel.StartListeningEvents();

        //_nextUnlockDrawerController.ActivateListeners();
        _sessionCoinDrawerController.ActivateListeners();

        UIButtons.ForEach(b => b.StartListening());

        _retryButton.OnButtonPressedUp += OnRetryPressed;
        _retryButton.OnButtonTapped += OnRetryPressed;
    }

    protected override IEnumerator PreActivateAdditional()
    {
        //_nextUnlockDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetPrevNextUnlockPLD() });

        return base.PreActivateAdditional();
    }

    protected override IEnumerator PostActivateAdditional()
    {
        _sessionCoinDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetSessionCoinPLD() });

        return base.PostActivateAdditional();
    }

    private void OnRetryPressed(PointerEventData eventData)
    {
        _viewModel.RetryPressed();
    }
}

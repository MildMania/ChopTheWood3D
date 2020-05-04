using MMUISystem.UIButton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDUI : UIMenu<HUDUI>, IInputBlocker
{
    [SerializeField] private LevelIDDrawerController _levelIDDrawerController;
    [SerializeField] private LevelProgressDrawerController _levelProgressDrawerController;
    [SerializeField] private SpeedIndicatorDawerController _speedIndicatorDawerController;

    [SerializeField] private UnityUIButton _retryButton;
    [SerializeField] private UnityUIButton _pauseButton;

    private HUDVM _viewModel;

    protected override void Awake()
    {
        _viewModel = new HUDVM();

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

        _viewModel.OnProgressUpdating -= OnProgressUpdated;

        _levelIDDrawerController.DeactivateListeners();
        _levelProgressDrawerController.DeactivateListeners();
        _speedIndicatorDawerController.DeactivateListeners();

        UIButtons.ForEach(b => b.StopListening());

        _retryButton.OnButtonPressedUp -= OnRetryPressed;
        _retryButton.OnButtonTapped -= OnRetryPressed;

        _pauseButton.OnButtonPressedUp -= OnPausePressed;
        _pauseButton.OnButtonTapped -= OnPausePressed;
    }

    protected override void StartListeningEvents()
    {
        _viewModel.StartListeningEvents();

        _viewModel.OnProgressUpdating += OnProgressUpdated;

        _levelIDDrawerController.ActivateListeners();
        _levelProgressDrawerController.ActivateListeners();
        _speedIndicatorDawerController.ActivateListeners();

        UIButtons.ForEach(b => b.StartListening());

        _retryButton.OnButtonPressedUp += OnRetryPressed;
        _retryButton.OnButtonTapped += OnRetryPressed;

        _pauseButton.OnButtonPressedUp += OnPausePressed;
        _pauseButton.OnButtonTapped += OnPausePressed;
    }

    private void OnProgressUpdated(IPLDBase pld)
    {
        _levelProgressDrawerController.DistributeData(new List<IPLDBase>() { pld });
    }

    protected override IEnumerator PreActivateAdditional()
    {
        _levelIDDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetLevelIDPLD() });
        _speedIndicatorDawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetSpeedPLD() });

        _levelProgressDrawerController.ResetDrawers();

        return base.PreActivateAdditional();
    }

    private void OnRetryPressed(PointerEventData eventData)
    {
        _viewModel.RetryPressed();
    }

    private void OnPausePressed(PointerEventData eventData)
    {
        _viewModel.PausePressed();
    }
}

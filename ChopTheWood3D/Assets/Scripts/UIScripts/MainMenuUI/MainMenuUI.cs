using MMUISystem.UIButton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUI : UIMenu<MainMenuUI>, IInputBlocker
{
    [SerializeField] private LevelIDDrawerController _levelIDDrawerController;
    [SerializeField] private TotalCoinDrawerController _totalCoinDrawerController;
    [SerializeField] private UnityUIButton _playButton;

    private MainMenuVM _viewModel;

    protected override void Awake()
    {
        _viewModel = new MainMenuVM();

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

        _levelIDDrawerController.DeactivateListeners();
        _totalCoinDrawerController.DeactivateListeners();

        UIButtons.ForEach(b => b.StopListening());

        _playButton.OnButtonPressedUp -= OnPlayPressed;
        _playButton.OnButtonTapped -= OnPlayPressed;
    }

    protected override void StartListeningEvents()
    {
        _viewModel.StartListeningEvents();

        _levelIDDrawerController.ActivateListeners();
        _totalCoinDrawerController.ActivateListeners();

        UIButtons.ForEach(b => b.StartListening());

        _playButton.OnButtonPressedUp += OnPlayPressed;
        _playButton.OnButtonTapped += OnPlayPressed;
    }

    protected override IEnumerator PreActivateAdditional()
    {
        _levelIDDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetLevelIDPLD() });
        _totalCoinDrawerController.DistributeData(new List<IPLDBase>() { _viewModel.GetTotalCoinPLD() });

        return base.PreActivateAdditional();
    }

    private void OnPlayPressed(PointerEventData eventData)
    {
        _viewModel.PlayPressed();
    }
}

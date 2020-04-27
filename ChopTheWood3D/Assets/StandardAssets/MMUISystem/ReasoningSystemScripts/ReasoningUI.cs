using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ReasoningUI : UIMenu<ReasoningUI>
{
    public ReasoningUI_TextSpawner TextSpawner;

    private ReasoningVM _viewModel;

    protected override void Awake()
    {
        _viewModel = new ReasoningVM();

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

    protected override IEnumerator PreActivateAdditional()
    {
        TextSpawner.GenerateDrawers(20, false);

        return base.PreActivateAdditional();
    }

    protected override void FinishListeningEvents()
    {
        _viewModel.FinishListeningEvents();

        TextSpawner.DeactivateListeners();
        
        _viewModel.OnReasonReceived -= OnReasonReceived;
    }

    protected override void StartListeningEvents()
    {
        _viewModel.StartListeningEvents();

        TextSpawner.ActivateListeners();

        _viewModel.OnReasonReceived += OnReasonReceived;
    }

    private void OnReasonReceived(ReasoningTextPLD pld)
    {
        TextSpawner.DistributeData(new List<IPLDBase>() { pld });
    }
}

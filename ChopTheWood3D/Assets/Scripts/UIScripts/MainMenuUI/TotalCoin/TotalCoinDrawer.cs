using TMPro;
using UnityEngine;

public class TotalCoinDrawerPLD : IPLDBase
{
    public string TotalCoinText { get; private set; }

    public TotalCoinDrawerPLD(string totalCoinText)
    {
        TotalCoinText = totalCoinText;
    }
}

public class TotalCoinDrawer : DrawerBase<TotalCoinDrawerPLD>
{
    [SerializeField] private TextMeshProUGUI _coinText;

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public override void ParseData(TotalCoinDrawerPLD pld)
    {
        _coinText.SetText(pld.TotalCoinText);
    }

    public override void ResetDrawer()
    {
    }
}

using UnityEngine;

public class SessionCoinDrawerPLD : IPLDBase
{
    public int SessionCoinText { get; private set; }

    public SessionCoinDrawerPLD(int sessionCoinText)
    {
        SessionCoinText = sessionCoinText;
    }
}

public class SessionCoinDrawer : DrawerBase<SessionCoinDrawerPLD>
{
    [SerializeField] private NumericalCounterScript _numericalCounter;

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public override void ParseData(SessionCoinDrawerPLD pld)
    {
        _numericalCounter.TextMesh.SetText("+0");

        _numericalCounter.SetCounter(pld.SessionCoinText, false, true, null);
    }

    public override void ResetDrawer()
    {
    }
}

using TMPro;
using UnityEngine;

public class SpeedIndicatorDrawerPLD : IPLDBase
{
    public string Speed { get; private set; }

    public SpeedIndicatorDrawerPLD(string speed)
    {
        Speed = speed;
    }
}

public class SpeedIndicatorDrawer : DrawerBase<SpeedIndicatorDrawerPLD>
{
    [SerializeField] private TextMeshProUGUI _speedText;

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public override void ParseData(SpeedIndicatorDrawerPLD pld)
    {
        _speedText.SetText("<size=50>x</size>" + pld.Speed);
    }

    public override void ResetDrawer()
    {
    }
}
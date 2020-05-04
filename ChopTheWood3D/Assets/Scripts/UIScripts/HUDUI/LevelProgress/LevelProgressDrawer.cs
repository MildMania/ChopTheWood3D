using UnityEngine;

public class LevelProgressDrawerPLD : IPLDBase
{
    public float Percentage { get; private set; }

    public LevelProgressDrawerPLD(float perc)
    {
        Percentage = perc;
    }
}

public class LevelProgressDrawer : DrawerBase<LevelProgressDrawerPLD>
{
    [SerializeField] private AdvancedFillBarScript _fillbar;

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public override void ParseData(LevelProgressDrawerPLD pld)
    {
        _fillbar.UpdateBar(Mathf.CeilToInt(pld.Percentage));
    }

    public override void ResetDrawer()
    {
        _fillbar.UpdateBar(0, true);
    }
}
using TMPro;
using UnityEngine;

public class LevelIDDrawerPLD : IPLDBase
{
    public string LevelID { get; private set; }

    public LevelIDDrawerPLD(string levelID)
    {
        LevelID = levelID;
    }
}

public class LevelIDDrawer : DrawerBase<LevelIDDrawerPLD>
{
    [SerializeField] private TextMeshProUGUI _levelIDText;

    public override void ActivateListeners()
    {
    }

    public override void DeactivateListeners()
    {
    }

    public override void ParseData(LevelIDDrawerPLD pld)
    {
        _levelIDText.SetText("level " + pld.LevelID);
    }

    public override void ResetDrawer()
    {
    }
}

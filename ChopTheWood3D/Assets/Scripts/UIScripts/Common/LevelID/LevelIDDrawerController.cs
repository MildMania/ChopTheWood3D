using System.Collections.Generic;

public class LevelIDDrawerController : DrawerControllerBase<LevelIDDrawerPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        DrawerList[0].ParseData((LevelIDDrawerPLD)pldList[0]);
    }
}

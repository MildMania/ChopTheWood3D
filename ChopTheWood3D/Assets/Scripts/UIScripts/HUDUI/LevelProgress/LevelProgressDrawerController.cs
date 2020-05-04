using System.Collections.Generic;

public class LevelProgressDrawerController : DrawerControllerBase<LevelProgressDrawerPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        DrawerList[0].ParseData((LevelProgressDrawerPLD)pldList[0]);
    }
}

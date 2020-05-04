using System.Collections.Generic;

public class SpeedIndicatorDawerController : DrawerControllerBase<SpeedIndicatorDrawerPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        DrawerList[0].ParseData((SpeedIndicatorDrawerPLD)pldList[0]);
    }
}

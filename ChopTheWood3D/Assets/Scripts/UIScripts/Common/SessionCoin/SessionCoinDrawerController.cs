using System.Collections.Generic;

public class SessionCoinDrawerController : DrawerControllerBase<SessionCoinDrawerPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        DrawerList[0].ParseData((SessionCoinDrawerPLD)pldList[0]);
    }
}

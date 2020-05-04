using System.Collections.Generic;

public class TotalCoinDrawerController : DrawerControllerBase<TotalCoinDrawerPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        DrawerList[0].ParseData((TotalCoinDrawerPLD)pldList[0]);
    }
}

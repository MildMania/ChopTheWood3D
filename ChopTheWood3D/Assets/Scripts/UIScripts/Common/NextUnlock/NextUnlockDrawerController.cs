using System;
using System.Collections.Generic;

public class NextUnlockDrawerController : DrawerControllerBase<NextUnlockDrawerPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        DrawerList[0].ParseData((NextUnlockDrawerPLD)pldList[0]);
    }

    public void PlayUnlockAnim()
    {
        ((NextUnlockDrawer)DrawerList[0]).PlayUnlockAnim();
    }
}

using System.Collections.Generic;

public class ReasoningTextPLD : IPLDBase
{
    public string Text;
}

public class ReasoningUI_TextSpawner : GenericDrawerControllerBase<ReasoningTextPLD>
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void DistributeData(List<IPLDBase> pldList)
    {
        List<DrawerBase<ReasoningTextPLD>> drawerList = GetAvailDrawers();

        if(drawerList.Count == 0)
        {
            DrawerBase<ReasoningTextPLD> targetDrawer = DrawerList.Find(d => !((ReasoningTextDrawer)d).IsAvail);

            targetDrawer.ResetDrawer();

            drawerList = new List<DrawerBase<ReasoningTextPLD>>() { targetDrawer };
        }

        for(int i = 0; i < pldList.Count; i++)
            drawerList[i].ParseData((ReasoningTextPLD)pldList[i]);
    }

    private List<DrawerBase<ReasoningTextPLD>> GetAvailDrawers()
    {
        return DrawerList.FindAll(d => ((ReasoningTextDrawer)d).IsAvail);
    }
}

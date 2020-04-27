using System;
using System.Collections.Generic;

public static class DescriptionUIDataController
{
    private static List<DescriptionUIData> _subDrawerList = new List<DescriptionUIData>()
    {
    };

    public static IPLDBase ParseDescriptionData(IConvertible itemType)
    {
        return GetSubDrawer(itemType).GenerateDrawerPLD(itemType);
    }

    private static DescriptionUIData GetSubDrawer(IConvertible objType)
    {
        return _subDrawerList.Find(s => s.CheckIfAppropriateToParse(objType));
    }
}


using System;

public abstract class DescriptionUIData
{
    public abstract IPLDBase GenerateDrawerPLD(IConvertible itemType);
    public abstract bool CheckIfAppropriateToParse(IConvertible itemType);
}

public abstract class DescriptionUIData<T> : DescriptionUIData
    where T : IConvertible
{
    public abstract IPLDBase GenerateDrawerPLD(T itemType);

    public override IPLDBase GenerateDrawerPLD(IConvertible itemType)
    {
        return GenerateDrawerPLD((T)itemType);
    }
}

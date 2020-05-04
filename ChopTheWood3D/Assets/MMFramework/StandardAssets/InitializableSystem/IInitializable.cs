using System;

public interface IInitializable
{
    void InitComponent();
    bool IsInitPrerequisitesSatisfied();
    InitializableBase Initializable { get; }
}

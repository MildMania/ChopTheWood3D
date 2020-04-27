public class ParameterEncapsulation
{
    public IUIMenuPreActivationParameter PreActivationParameter { get; set; }
    public IUIMenuPostActivationParameter PostActivationParameter { get; set; }

    public IUIMenuPreDeactivationParameter PreDeactivationParameter { get; set; }
    public IUIMenuPostDeactivationParameter PostDeactivationParameter { get; set; }
}
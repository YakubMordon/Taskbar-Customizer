namespace Taskbar_Customizer.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}

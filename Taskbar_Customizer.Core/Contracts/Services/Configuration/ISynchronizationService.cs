namespace Taskbar_Customizer.Core.Contracts.Services.Configuration;

/// <summary>
/// Contract for handling actions with synchronization.
/// </summary>
public interface ISynchronizationService
{
    /// <summary>
    /// Method for calling async service synchronization.
    /// </summary>
    /// <param name="key">Key for synchronization.</param>
    /// <param name="value">Value for synchronization.</param>
    public void CallSyncService(string? key, string? value);
}
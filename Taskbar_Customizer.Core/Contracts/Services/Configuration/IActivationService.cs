// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Core.Contracts.Services.Configuration;

/// <summary>
/// Contract for a service for activating an application asynchronously.
/// </summary>
public interface IActivationService
{
    /// <summary>
    /// Method for Activation of the application asynchronously with the specified activation arguments.
    /// </summary>
    /// <param name="activationArgs">The activation arguments passed to the application.</param>
    /// <returns>A task representing the asynchronous activation operation.</returns>
    Task ActivateAsync(object activationArgs);
}

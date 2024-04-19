// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Activation;

/// <summary>
/// Contract for activation handlers.
/// </summary>
public interface IActivationHandler
{
    /// <summary>
    /// Method for determining whether this activation handler can handle the specified arguments.
    /// </summary>
    /// <param name="args">The arguments to evaluate.</param>
    /// <returns>True if this handler can handle the arguments; otherwise, false.</returns>
    bool CanHandle(object args);

    /// <summary>
    /// Asynchronous Event Handler for the activation based on the specified arguments.
    /// </summary>
    /// <param name="args">The arguments for the activation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(object args);
}

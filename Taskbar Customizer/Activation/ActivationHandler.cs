// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Activation;

/// <summary>
/// Base class for implementing custom activation handlers.
/// </summary>
/// <typeparam name="T">The type of activation arguments this handler can process.</typeparam>
/// <remarks>
/// Extend this class to implement new activation handlers. See <see cref="DefaultActivationHandler"/> for an example.
/// For more information about activation handlers in WinUI, visit:
/// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
/// </remarks>
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    /// <inheritdoc />
    public bool CanHandle(object args) => args is T && this.CanHandleInternal((args as T) !);

    /// <inheritdoc />
    public async Task HandleAsync(object args) => await this.HandleInternalAsync((args as T) !);

    /// <summary>
    /// Method, which determines whether this activation handler can handle the specified activation arguments.
    /// </summary>
    /// <param name="args">The activation arguments.</param>
    /// <returns>True if this handler can handle the activation; otherwise, false.</returns>
    protected virtual bool CanHandleInternal(T args) => true;

    /// <summary>
    /// Method, which handles the activation asynchronously based on the specified arguments.
    /// </summary>
    /// <param name="args">The activation arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task HandleInternalAsync(T args);
}

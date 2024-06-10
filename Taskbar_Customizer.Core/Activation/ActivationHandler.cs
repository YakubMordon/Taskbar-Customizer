// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Core.Activation;

using Taskbar_Customizer.Core.Activation;

/// <summary>
/// Base class for implementing custom activation handlers.
/// </summary>
/// <typeparam name="T">The type of activation arguments this handler can process.</typeparam>
/// <remarks>
/// Extend this class to implement new activation handlers. See <see cref="DefaultActivationHandler"/> for an example.
/// For more information about activation handlers in WinUI, visit:
/// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md.
/// </remarks>
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    /// <inheritdoc />
    public bool CanHandle(object args) => args is T && this.CanHandleInternal((args as T) !);

    /// <inheritdoc />
    public async Task HandleAsync(object args) => await this.HandleInternalAsync((args as T) !);

    protected virtual bool CanHandleInternal(T args) => true;

    protected abstract Task HandleInternalAsync(T args);
}

// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Event_Handlers;

/// <summary>
/// Class for handling event of changing language.
/// </summary>
public class LanguageChangeEventHandler
{
    /// <summary>
    /// Delegate for intersection event.
    /// </summary>
    /// <param name="sender">Object, which send event.</param>
    /// <param name="e">Passed event arguments.</param>
    public delegate void LanguageEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Event handler for changing language.
    /// </summary>
    public event LanguageEventHandler? EventHandler;

    /// <summary>
    /// Method for invoking language change event.
    /// </summary>
    public void OnLanguageChanged()
    {
        this.EventHandler?.Invoke(this, EventArgs.Empty);
    }
}
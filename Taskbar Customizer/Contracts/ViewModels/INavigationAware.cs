// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Contracts.ViewModels;

/// <summary>
/// Contract for objects that need to be notified when navigation occurs.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Method called when the implementing object is navigated to with a parameter.
    /// </summary>
    /// <param name="parameter">The navigation parameter.</param>
    void OnNavigatedTo(object parameter);

    /// <summary>
    /// Method called when the implementing object is navigated away from.
    /// </summary>
    void OnNavigatedFrom();
}

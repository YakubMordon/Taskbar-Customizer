// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using Microsoft.UI;
using Microsoft.UI.Xaml;

using Taskbar_Customizer.Helpers.Helpers.Native;

using Windows.UI;

/// <summary>
/// Helper class to manage custom title bar appearance and behavior.
/// DISCLAIMER: The resource key names and color values used below are subject to change. Do not depend on them.
/// Issues related to custom title bars: https://github.com/microsoft/TemplateStudio/issues/4516.
/// </summary>
public static class TitleBarHelper
{
    private const int WAINACTIVE = 0x00;

    private const int WAACTIVE = 0x01;

    private const int WMACTIVATE = 0x0006;

    /// <summary>
    /// Method for updating the appearance of the title bar based on the specified theme.
    /// </summary>
    /// <param name="theme">The desired theme for the title bar.</param>
    public static void UpdateTitleBar(ElementTheme theme)
    {
        if (App.MainWindow.ExtendsContentIntoTitleBar)
        {
            if (theme is ElementTheme.Default)
            {
                theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
            }

            var colors = theme switch
            {
                ElementTheme.Dark => (
                    ButtonForegroundColor: Colors.White,
                    ButtonHoverForegroundColor: Colors.White,
                    ButtonHoverBackgroundColor: Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF),
                    ButtonPressedBackgroundColor: Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                ElementTheme.Light => (
                    ButtonForegroundColor: Colors.Black,
                    ButtonHoverForegroundColor: Colors.Black,
                    ButtonHoverBackgroundColor: Color.FromArgb(0x33, 0x00, 0x00, 0x00),
                    ButtonPressedBackgroundColor: Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                _ => (
                    ButtonForegroundColor: Colors.Transparent,
                    ButtonHoverForegroundColor: Colors.Transparent,
                    ButtonHoverBackgroundColor: Colors.Transparent,
                    ButtonPressedBackgroundColor: Colors.Transparent)
            };

            // Configure button colors
            App.MainWindow.AppWindow.TitleBar.ButtonForegroundColor = colors.ButtonForegroundColor;
            App.MainWindow.AppWindow.TitleBar.ButtonHoverForegroundColor = colors.ButtonHoverForegroundColor;
            App.MainWindow.AppWindow.TitleBar.ButtonHoverBackgroundColor = colors.ButtonHoverBackgroundColor;
            App.MainWindow.AppWindow.TitleBar.ButtonPressedBackgroundColor = colors.ButtonPressedBackgroundColor;

            // Set title bar background color
            App.MainWindow.AppWindow.TitleBar.BackgroundColor = Colors.Transparent;

            // Activate the window based on its active state
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            if (hwnd == User32Interop.GetActiveWindow())
            {
                User32Interop.SendMessage(hwnd, WMACTIVATE, WAINACTIVE, nint.Zero);
                User32Interop.SendMessage(hwnd, WMACTIVATE, WAACTIVE, nint.Zero);
            }
            else
            {
                User32Interop.SendMessage(hwnd, WMACTIVATE, WAACTIVE, nint.Zero);
                User32Interop.SendMessage(hwnd, WMACTIVATE, WAINACTIVE, nint.Zero);
            }
        }
    }

    /// <summary>
    /// Method for applying the system theme to the caption buttons of the window.
    /// </summary>
    public static void ApplySystemThemeToCaptionButtons()
    {
        var frame = App.AppTitlebar as FrameworkElement;
        if (frame is not null)
        {
            UpdateTitleBar(frame.ActualTheme);
        }
    }
}
// Copyright (c) Digital Cloud Technologies. All rights reserved.

namespace Taskbar_Customizer.Helpers.Helpers.Taskbar;

using System.Runtime.InteropServices;

using Microsoft.UI;
using Microsoft.UI.Xaml;

using Windows.UI;
using Windows.UI.ViewManagement;

/// <summary>
/// Helper class to manage custom title bar appearance and behavior.
/// DISCLAIMER: The resource key names and color values used below are subject to change. Do not depend on them.
/// Issues related to custom title bars: https://github.com/microsoft/TemplateStudio/issues/4516.
/// </summary>
internal class TitleBarHelper
{
    /// <summary>
    /// The wParam value for the WM_ACTIVATE message when the window is not active.
    /// </summary>
    private const int WAINACTIVE = 0x00;

    /// <summary>
    /// The wParam value for the WM_ACTIVATE message when the window is active.
    /// </summary>
    private const int WAACTIVE = 0x01;

    /// <summary>
    /// The message code for the WM_ACTIVATE message.
    /// </summary>
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
                var uiSettings = new UISettings();
                var background = uiSettings.GetColorValue(UIColorType.Background);

                theme = background == Colors.White ? ElementTheme.Light : ElementTheme.Dark;
            }

            if (theme is ElementTheme.Default)
            {
                theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
            }

            // Configure button foreground color
            App.MainWindow.AppWindow.TitleBar.ButtonForegroundColor = theme switch
            {
                ElementTheme.Dark => Colors.White,
                ElementTheme.Light => Colors.Black,
                _ => Colors.Transparent
            };

            // Configure button hover foreground color
            App.MainWindow.AppWindow.TitleBar.ButtonHoverForegroundColor = theme switch
            {
                ElementTheme.Dark => Colors.White,
                ElementTheme.Light => Colors.Black,
                _ => Colors.Transparent
            };

            // Configure button hover background color
            App.MainWindow.AppWindow.TitleBar.ButtonHoverBackgroundColor = theme switch
            {
                ElementTheme.Dark => Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF),
                ElementTheme.Light => Color.FromArgb(0x33, 0x00, 0x00, 0x00),
                _ => Colors.Transparent
            };

            // Configure button pressed background color
            App.MainWindow.AppWindow.TitleBar.ButtonPressedBackgroundColor = theme switch
            {
                ElementTheme.Dark => Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF),
                ElementTheme.Light => Color.FromArgb(0x66, 0x00, 0x00, 0x00),
                _ => Colors.Transparent
            };

            // Set title bar background color
            App.MainWindow.AppWindow.TitleBar.BackgroundColor = Colors.Transparent;

            // Activate the window based on its active state
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            if (hwnd == GetActiveWindow())
            {
                SendMessage(hwnd, WMACTIVATE, WAINACTIVE, nint.Zero);
                SendMessage(hwnd, WMACTIVATE, WAACTIVE, nint.Zero);
            }
            else
            {
                SendMessage(hwnd, WMACTIVATE, WAACTIVE, nint.Zero);
                SendMessage(hwnd, WMACTIVATE, WAINACTIVE, nint.Zero);
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

    /// <summary>
    /// Method for retrieving the handle to the active window.
    /// </summary>
    /// <returns>The handle to the active window.</returns>
    [DllImport("user32.dll")]
    private static extern nint GetActiveWindow();

    /// <summary>
    /// Method for sending the specified message to a window or windows.
    /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern nint SendMessage(nint hWnd, int msg, int wParam, nint lParam);
}
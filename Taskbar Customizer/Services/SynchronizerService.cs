// Copyright (c) Digital Cloud Technologies. All rights reserved.

using Microsoft.Identity.Client;

namespace Taskbar_Customizer.Services;

/// <summary>
/// Service for synchronization of data using Project ROME.
/// </summary>
public static class SynchronizerService
{
    /// <summary>
    /// Application (Client) Id;
    /// </summary>
    private const string ClientId = "97c98de9-e0f7-4794-8d28-7179332a230e";

    /// <summary>
    /// Public Client App (Acccessor to API).
    /// </summary>
    public static IPublicClientApplication PublicClientApp;

    /// <summary>
    /// Method for initialization of the notification manager and registers for notification events.
    /// </summary>
    public static void Initialize()
    {
        PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                                            .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/> nativeclient")
                                                .Build();
    }
}

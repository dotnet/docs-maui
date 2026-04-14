---
title: "Authenticate users with MSAL.NET"
description: "Learn how to use MSAL.NET (Microsoft.Identity.Client) to authenticate users against Microsoft Entra ID in a .NET MAUI app, including platform-specific setup, silent-first token acquisition, broker support, and bearer token propagation."
ms.date: 03/17/2026
---

# Authenticate users with MSAL.NET

Microsoft Authentication Library for .NET (MSAL.NET) is the recommended library for authenticating users against Microsoft Entra ID (formerly Azure AD) in .NET MAUI apps. It handles token acquisition, caching, refresh, and platform-specific broker integration.

This article covers:

- Registering your app and configuring platform redirect URIs
- Wrapping MSAL in an injectable service
- Silent-first token acquisition
- Broker support for SSO and Conditional Access
- Platform-specific setup for Android, iOS, and Windows
- Attaching bearer tokens to HTTP calls with a `DelegatingHandler`
- Blazor Hybrid integration with `AuthenticationStateProvider`

## Prerequisites

Add the `Microsoft.Identity.Client` NuGet package to your .NET MAUI project:

```xml
<PackageReference Include="Microsoft.Identity.Client" Version="4.*" />
<PackageReference Include="Microsoft.Identity.Client.Broker" Version="4.*" />
```

> [!NOTE]
> `Microsoft.Identity.Client.Broker` is required for broker support (Microsoft Authenticator, Company Portal, and Windows WAM). Replace `4.*` with the latest stable version from [NuGet](https://www.nuget.org/packages/Microsoft.Identity.Client).

## Register your app

Register your app in the [Microsoft Entra admin center](https://entra.microsoft.com) and configure a platform redirect URI for each target platform.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Add a redirect URI of the form:

```
msal{ClientId}://auth
```

For example, if your client ID is `00000000-0000-0000-0000-000000000000`:

```
msal00000000-0000-0000-0000-000000000000://auth
```

# [iOS/Mac Catalyst](#tab/macios)

Add a redirect URI of the form:

```
msauth.{BundleId}://auth
```

For example:

```
msauth.com.mycompany.myapp://auth
```

# [Windows](#tab/windows)

If you're using the default system browser (no broker), add:

```
http://localhost
```

If you're using broker support (WAM — Windows Web Account Manager), use the packaged app redirect URI instead:

```
ms-appx-web://microsoft.aad.brokerplugin/{ClientId}
```

For example:

```
ms-appx-web://microsoft.aad.brokerplugin/00000000-0000-0000-0000-000000000000
```

-----
<!-- markdownlint-enable MD025 -->

## Create an authentication service

Wrap `IPublicClientApplication` in an injectable `IAuthService` interface. This keeps MSAL out of your ViewModels and makes the service testable.

```csharp
public interface IAuthService
{
    Task<AuthenticationResult?> AcquireTokenAsync(CancellationToken ct = default);
    Task SignOutAsync(CancellationToken ct = default);
}
```

Implement `IAuthService` using MSAL's `PublicClientApplicationBuilder`:

```csharp
public class MsalAuthService : IAuthService
{
    const string ClientId = "00000000-0000-0000-0000-000000000000";
    const string TenantId = "your-tenant-id-or-common";

    static readonly string[] DefaultScopes = ["User.Read"];

    readonly IPublicClientApplication _pca;
    IAccount? _cachedAccount;

    public MsalAuthService()
    {
        var builder = PublicClientApplicationBuilder
            .Create(ClientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
            .WithRedirectUri(GetRedirectUri());

#if IOS || MACCATALYST
        // WithIosKeychainSecurityGroup enables broker token cache sharing on iOS/Mac Catalyst.
        // The value must match the keychain-access-groups entry in Entitlements.plist.
        builder.WithIosKeychainSecurityGroup("com.microsoft.adalcache");
#endif

#if ANDROID || IOS || MACCATALYST
        // On Android, iOS, and Mac Catalyst, WithBroker() enables broker support via
        // Microsoft Authenticator or Company Portal.
        builder.WithBroker();
#elif WINDOWS
        // On Windows, broker support uses WAM (Web Account Manager).
        // Requires the Microsoft.Identity.Client.Broker NuGet package.
        // Also update the Windows redirect URI to ms-appx-web://microsoft.aad.brokerplugin/{ClientId}.
        builder.WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows));
#endif

        _pca = builder.Build();
    }

    static string GetRedirectUri()
    {
#if ANDROID
        return $"msal{ClientId}://auth";
#elif IOS || MACCATALYST
        return $"msauth.{AppInfo.PackageName}://auth";
#else
        return "http://localhost";
#endif
    }
}
```

## Acquire tokens silently first

Always attempt a silent token acquisition before falling back to an interactive prompt. Silent acquisition succeeds when a valid (or refreshable) token is already cached, avoiding unnecessary interactive sign-in prompts.

```csharp
public async Task<AuthenticationResult?> AcquireTokenAsync(CancellationToken ct = default)
{
    // 1. Try to find a previously signed-in account
    var accounts = await _pca.GetAccountsAsync();
    _cachedAccount = accounts.FirstOrDefault();

    // 2. Attempt silent acquisition
    if (_cachedAccount != null)
    {
        try
        {
            return await _pca
                .AcquireTokenSilent(DefaultScopes, _cachedAccount)
                .ExecuteAsync(ct);
        }
        catch (MsalUiRequiredException)
        {
            // Token expired or consent required — fall through to interactive
        }
    }

    // 3. Fall back to interactive sign-in
    var interactiveBuilder = _pca
        .AcquireTokenInteractive(DefaultScopes);

    // Only set login hint when a username is available — WithLoginHint does not accept null
    if (!string.IsNullOrWhiteSpace(_cachedAccount?.Username))
        interactiveBuilder = interactiveBuilder.WithLoginHint(_cachedAccount.Username);

#if ANDROID
    interactiveBuilder = interactiveBuilder
        .WithParentActivityOrWindow(Platform.CurrentActivity
            ?? throw new InvalidOperationException("No current Activity. Ensure Platform.Init() is called in MainActivity.OnCreate."));
#elif IOS || MACCATALYST
    interactiveBuilder = interactiveBuilder
        .WithParentActivityOrWindow(Platform.GetCurrentUIViewController()
            ?? throw new InvalidOperationException("No current UIViewController."));
#endif

    return await interactiveBuilder.ExecuteAsync(ct);
}

public async Task SignOutAsync(CancellationToken ct = default)
{
    var accounts = await _pca.GetAccountsAsync();
    foreach (var account in accounts)
        await _pca.RemoveAsync(account);

    _cachedAccount = null;
}
```

> [!IMPORTANT]
> On Android, always pass `.WithParentActivityOrWindow(Platform.CurrentActivity)` to `AcquireTokenInteractive`. Omitting this causes a crash at runtime because MSAL cannot find a parent window to host the sign-in UI. Ensure `Platform.Init(this, savedInstanceState)` is called in `MainActivity.OnCreate` so that `Platform.CurrentActivity` is set correctly.

## Enable broker support

Broker support routes authentication through the Microsoft Authenticator app (Android/iOS) or Windows Web Account Manager. It enables Single Sign-On (SSO) across apps, Conditional Access policy enforcement, and device compliance checks.

Call `WithBroker` on the builder as shown in the service above. Additionally, configure each platform:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Add package-visibility entries to `Platforms/Android/AndroidManifest.xml` so Android 11+ allows querying broker apps:

```xml
<queries>
  <package android:name="com.azure.authenticator" />
  <package android:name="com.microsoft.windowsintune.companyportal" />
  <package android:name="com.microsoft.workaccount" />
</queries>
```

Add the MSAL auth continuation activity to handle the redirect after broker sign-in. Create `Platforms/Android/MsalActivity.cs`:

```csharp
[Activity(Exported = true,
          LaunchMode = LaunchMode.SingleTask,
          NoHistory = true,
          ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
    DataHost = "auth",
    DataScheme = "msal00000000-0000-0000-0000-000000000000")]
public class MsalActivity : BrowserTabActivity { }
```

Replace `msal00000000-0000-0000-0000-000000000000` with your actual `msal{ClientId}` scheme.

Also override `OnActivityResult` in `Platforms/Android/MainActivity.cs`:

```csharp
protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
{
    base.OnActivityResult(requestCode, resultCode, data);
    AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
}
```

# [iOS/Mac Catalyst](#tab/macios)

Add the `msauth.*` URL scheme to `Platforms/iOS/Info.plist` so iOS routes the broker callback back to your app:

```xml
<key>CFBundleURLTypes</key>
<array>
  <dict>
    <key>CFBundleURLSchemes</key>
    <array>
      <string>msauth.com.mycompany.myapp</string>
    </array>
  </dict>
</array>
```

Add the broker URL schemes MSAL queries for when detecting broker availability:

```xml
<key>LSApplicationQueriesSchemes</key>
<array>
  <string>msauthv2</string>
  <string>msauthv3</string>
</array>
```

Add the keychain access group to `Platforms/iOS/Entitlements.plist` (not `Info.plist`) to allow MSAL to share the token cache with broker apps. This must match the value passed to `WithIosKeychainSecurityGroup`:

```xml
<key>keychain-access-groups</key>
<array>
  <string>$(AppIdentifierPrefix)com.microsoft.adalcache</string>
</array>
```

Override `OpenUrl` in `Platforms/iOS/AppDelegate.cs` to handle the redirect:

```csharp
public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
{
    if (AuthenticationContinuationHelper.IsBrokerResponse(null))
    {
        // Process broker response on a background thread as required by MSAL
        _ = Task.Factory.StartNew(() =>
            AuthenticationContinuationHelper.SetBrokerContinuationEventArgs(url));
        return true;
    }
    else if (!AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url))
    {
        return false;
    }

    return true;
}
```

# [Windows](#tab/windows)

No broker setup is required. MSAL uses the `http://localhost` redirect URI and the system browser.

-----
<!-- markdownlint-enable MD025 -->

## Register the service

Register `MsalAuthService` in `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<IAuthService, MsalAuthService>();
```

## Attach bearer tokens to HTTP calls

Use a `DelegatingHandler` to automatically attach the access token to all outgoing API requests, so individual services don't need to handle token acquisition:

```csharp
public class AuthHandler : DelegatingHandler
{
    readonly IAuthService _authService;

    public AuthHandler(IAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        var result = await _authService.AcquireTokenAsync(ct);

        if (result != null)
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", result.AccessToken);

        return await base.SendAsync(request, ct);
    }
}
```

Register the handler and your typed `HttpClient` in `MauiProgram.cs`:

```csharp
builder.Services.AddTransient<AuthHandler>();

builder.Services.AddHttpClient<IMyApiClient, MyApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com/");
})
.AddHttpMessageHandler<AuthHandler>();
```

## Blazor Hybrid integration

In a .NET MAUI Blazor Hybrid app, surface the MSAL authentication state to Blazor components by implementing a custom `AuthenticationStateProvider`:

```csharp
public class MsalAuthenticationStateProvider : AuthenticationStateProvider
{
    readonly IAuthService _authService;

    public MsalAuthenticationStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var result = await _authService.AcquireTokenAsync();

            if (result == null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = result.ClaimsPrincipal?.Claims
                ?? (string.IsNullOrEmpty(result.IdToken)
                    ? Enumerable.Empty<Claim>()
                    : ParseClaims(result.IdToken));

            var identity = new ClaimsIdentity(claims, "msal");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyAuthenticationStateChanged() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    static IEnumerable<Claim> ParseClaims(string idToken)
    {
        // Decode the JWT id_token payload and extract claims
        var payload = idToken.Split('.')[1];
        var json = Encoding.UTF8.GetString(
            Convert.FromBase64String(PadBase64(payload)));
        var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)
            ?? new();

        return claims.Select(kvp =>
            new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
    }

    static string PadBase64(string base64Url)
    {
        // JWT uses base64url encoding: replace URL-safe chars before standard base64 decode
        var base64 = base64Url.Replace('-', '+').Replace('_', '/');
        return (base64.Length % 4) switch
        {
            2 => base64 + "==",
            3 => base64 + "=",
            _ => base64
        };
    }
}
```

Register the provider in `MauiProgram.cs`:

```csharp
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, MsalAuthenticationStateProvider>();
```

Use the standard `<AuthorizeView>` component in Razor pages to conditionally render authenticated content:

```razor
<AuthorizeView>
    <Authorized>
        <p>Hello, @context.User.Identity?.Name!</p>
    </Authorized>
    <NotAuthorized>
        <p>Please sign in.</p>
    </NotAuthorized>
</AuthorizeView>
```

## Related links

- [MSAL.NET documentation](/entra/msal/dotnet/)
- [Microsoft Entra External ID — sign users into a .NET MAUI app](/entra/external-id/customers/tutorial-mobile-app-maui-sign-in-prepare-tenant)
- [Microsoft Graph SDK tutorial for .NET MAUI](/windows/apps/windows-dotnet-maui/tutorial-graph-api)
- [.NET MAUI authentication and authorization architecture](/dotnet/architecture/maui/authentication-and-authorization)

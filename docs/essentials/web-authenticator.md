---
title: "Xamarin.Essentials: Web Authenticator"
description: "This document describes the WebAuthenticator class in Xamarin.Essentials, which lets you start browser based authentication flows which listen for a callback to the app."
author: redth
ms.author: jodick
ms.date: 03/26/2020
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Web Authenticator

The **WebAuthenticator** class lets you initiate browser based flows which listen for a callback to a specific URL registered to the app.

## Overview

Many apps require adding user authentication, and this often means enabling your users to sign in their existing Microsoft, Facebook, Google, and now Apple Sign In accounts.

[Microsoft Authentication Library (MSAL)](/azure/active-directory/develop/msal-overview) provides an excellent turn-key solution to adding authentication to your app. There's even support for Xamarin apps in their client NuGet package.

If you're interested in using your own web service for authentication, it's possible to use **WebAuthenticator** to implement the client side functionality.

## Why use a server back end?

Many authentication providers have moved to only offering explicit or two-legged authentication flows to ensure better security. This means you'll need a _'client secret'_ from the provider to complete the authentication flow. Unfortunately, mobile apps are not a great place to store secrets and anything stored in a mobile app's code, binaries, or otherwise is generally considered to be insecure.

The best practice here is to use a web backend as a middle layer between your mobile app and the authentication provider.

> [!IMPORTANT]
> We strongly recommend against using older mobile-only authentication libraries and patterns which do not leverage a web backend in the authentication flow due to their inherent lack of security for storing client secrets.

## Get started

[!include[](~/essentials/includes/get-started.md)]

To access the **WebAuthenticator** functionality the following platform specific setup is required.

# [Android](#tab/android)

Android requires an Intent Filter setup to handle your callback URI. This is easily accomplished by subclassing the `WebAuthenticatorCallbackActivity` class:

```csharp
const string CALLBACK_SCHEME = "myapp";

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
[IntentFilter(new[] { Android.Content.Intent.ActionView },
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
    DataScheme = CALLBACK_SCHEME)]
public class WebAuthenticationCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
{
}
```
If your project's Target Android version is set to **Android 11 (R API 30)** you must update your Android Manifest with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the **AndroidManifest.xml** file under the Properties folder and add the following inside of the manifest node:
```XML
<queries>
    <intent>
        <action android:name="android.support.customtabs.action.CustomTabsService" />
    </intent>
</queries>
```

# [iOS](#tab/ios)

On iOS you'll need to add your app's callback URI pattern to your Info.plist such as:

```xml
<key>CFBundleURLTypes</key>
<array>
    <dict>
        <key>CFBundleURLName</key>
        <string>xamarinessentials</string>
        <key>CFBundleURLSchemes</key>
        <array>
            <string>xamarinessentials</string>
        </array>
        <key>CFBundleTypeRole</key>
        <string>Editor</string>
    </dict>
</array>
```

You will also need to override your `AppDelegate`'s `OpenUrl` and `ContinueUserActivity` methods to call into Essentials:

```csharp
public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
{
    if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
        return true;

    return base.OpenUrl(app, url, options);
}

public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
{
    if (Xamarin.Essentials.Platform.ContinueUserActivity(application, userActivity, completionHandler))
        return true;
    return base.ContinueUserActivity(application, userActivity, completionHandler);
}
```

# [UWP](#tab/uwp)

For UWP, you'll need to declare your callback URI in your `Package.appxmanifest` file:

```xml
<Applications>
    <Application Id="App" Executable="$targetnametoken$.exe" EntryPoint="MyApp.App">
        <Extensions>
            <uap:Extension Category="windows.protocol">
            <uap:Protocol Name="myapp">
                <uap:DisplayName>My App</uap:DisplayName>
            </uap:Protocol>
            </uap:Extension>
        </Extensions>
    </Application>
</Applications>
```

-----

## Using WebAuthenticator

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The API consists mainly of a single method `AuthenticateAsync` which takes two parameters: The url which should be used to start the web browser flow, and the Uri which you expect the flow to ultimately call back to and which your app is registered to be able to handle.

The result is a `WebAuthenticatorResult` which includes any query parameters parsed from the callback URI:

```csharp
var authResult = await WebAuthenticator.AuthenticateAsync(
        new Uri("https://mysite.com/mobileauth/Microsoft"),
        new Uri("myapp://"));

var accessToken = authResult?.AccessToken;
```

The `WebAuthenticator` API takes care of launching the url in the browser and waiting until the callback is received:

![Typical Web Authentication Flow.](images/web-authenticator.png)

If the user cancels the flow at any point, a `TaskCanceledException` is thrown.

### Private authentication session

iOS 13 introduced an ephemeral web browser API for developers to launch the authentication session as private. This enables developers to request that no shared cookies or browsing data is available between authentication sessions and will be a fresh login session each time. This is available through the new `WebAuthenticatorOptions` that was introduced in Xamarin.Essentials 1.7 for iOS.

```csharp
var url = new Uri("https://mysite.com/mobileauth/Microsoft");
var callbackUrl = new Uri("myapp://")
var authResult = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
    {
        Url = url,
        CallbackUrl = callbackUrl,
        PrefersEphemeralWebBrowserSession = true
    });
```

## Platform differences

# [Android](#tab/android)

Custom Tabs are used whenever available, otherwise an Intent is started for the URL.

# [iOS](#tab/ios)

On iOS 12 or higher, `ASWebAuthenticationSession` is used.  On iOS 11, `SFAuthenticationSession` is used.  On older iOS versions, `SFSafariViewController` is used if available, otherwise Safari is used.

# [UWP](#tab/uwp)

On UWP, the `WebAuthenticationBroker` is used if supported, otherwise the system browser is used.

-----

## Apple Sign In

According to [Apple's review guidelines](https://developer.apple.com/app-store/review/guidelines/#sign-in-with-apple), if your app uses any social login service to authenticate, it must also offer Apple Sign In as an option.

To add Apple Sign In to your apps, first you'll need to [configure your app to use Apple Sign In](../ios/platform/ios13/sign-in.md).

For iOS 13 and higher you'll want to call the `AppleSignInAuthenticator.AuthenticateAsync()` method. This will use the native Apple Sign in API's under the hood so your users get the best experience possible on these devices. You can write your shared code to use the right API at runtime like this:

```csharp
var scheme = "..."; // Apple, Microsoft, Google, Facebook, etc.
WebAuthenticatorResult r = null;

if (scheme.Equals("Apple")
    && DeviceInfo.Platform == DevicePlatform.iOS
    && DeviceInfo.Version.Major >= 13)
{
    // Use Native Apple Sign In API's
    r = await AppleSignInAuthenticator.AuthenticateAsync();
}
else
{
    // Web Authentication flow
    var authUrl = new Uri(authenticationUrl + scheme);
    var callbackUrl = new Uri("xamarinessentials://");

    r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
}

var authToken = string.Empty;
if (r.Properties.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
    authToken += $"Name: {name}{Environment.NewLine}";
if (r.Properties.TryGetValue("email", out var email) && !string.IsNullOrEmpty(email))
    authToken += $"Email: {email}{Environment.NewLine}";

// Note that Apple Sign In has an IdToken and not an AccessToken
authToken += r?.AccessToken ?? r?.IdToken;
```

> [!TIP]
> For non-iOS 13 devices this will start the web authentication flow, which can also be used to enable Apple Sign In on your Android and UWP devices.
> You can sign into your iCloud account on your iOS simulator to test Apple Sign In.

-----

## ASP.NET core server back end

It's possible to use the `WebAuthenticator` API with any web back end service.  To use it with an ASP.NET core app, first you need to configure the web app with the following steps:

1. Setup your desired [external social authentication providers](/aspnet/core/security/authentication/social/?tabs=visual-studio) in an ASP.NET Core web app.
2. Set the Default Authentication Scheme to `CookieAuthenticationDefaults.AuthenticationScheme` in your `.AddAuthentication()` call.
3. Use `.AddCookie()` in your Startup.cs `.AddAuthentication()` call.
4. All providers must be configured with `.SaveTokens = true;`.


```csharp
services.AddAuthentication(o =>
    {
        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddFacebook(fb =>
    {
        fb.AppId = Configuration["FacebookAppId"];
        fb.AppSecret = Configuration["FacebookAppSecret"];
        fb.SaveTokens = true;
    });
```

> [!TIP]
> If you'd like to include Apple Sign In, you can use the `AspNet.Security.OAuth.Apple` NuGet package.  You can view the full [Startup.cs sample](https://github.com/xamarin/Essentials/blob/develop/Samples/Sample.Server.WebAuthenticator/Startup.cs#L32-L60) in the Essentials GitHub repository.

### Add a custom mobile auth controller

With a mobile authentication flow it is usually desirable to initiate the flow directly to a provider that the user has chosen (e.g. by clicking a "Microsoft" button on the sign in screen of the app).  It is also important to be able to return relevant information to your app at a specific callback URI to end the authentication flow.

To achieve this, use a custom API Controller:

```csharp
[Route("mobileauth")]
[ApiController]
public class AuthController : ControllerBase
{
    const string callbackScheme = "myapp";

    [HttpGet("{scheme}")] // eg: Microsoft, Facebook, Apple, etc
    public async Task Get([FromRoute]string scheme)
    {
        // 1. Initiate authentication flow with the scheme (provider)
        // 2. When the provider calls back to this URL
        //    a. Parse out the result
        //    b. Build the app callback URL
        //    c. Redirect back to the app
    }
}
```

The purpose of this controller is to infer the scheme (provider) that the app is requesting, and initiate the authentication flow with the social provider. When the provider calls back to the web backend, the controller parses out the result and redirects to the app's callback URI with parameters.

Sometimes you may want to return data such as the provider's `access_token` back to the app which you can do via the callback URI's query parameters. Or, you may want to instead create your own identity on your server and pass back your own token to the app. What and how you do this part is up to you!

Check out the [full controller sample](https://github.com/xamarin/Essentials/blob/develop/Samples/Sample.Server.WebAuthenticator/Controllers/MobileAuthController.cs) in the Essentials repository.

> [!NOTE]
> The above sample demonstrates how to return the Access Token from the 3rd party authentication (ie: OAuth) provider. To obtain a token you can use to authorize web requests to the web backend itself, you should create your own token in your web app, and return that instead.  The [Overview of ASP.NET Core authentication](/aspnet/core/security/authentication) has more information about advanced authentication scenarios in ASP.NET Core.

-----
## API

- [WebAuthenticator source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/WebAuthenticator)
- [WebAuthenticator API documentation](xref:Xamarin.Essentials.WebAuthenticator)
- [ASP.NET Core Server Sample](https://github.com/xamarin/Essentials/blob/develop/Samples/Sample.Server.WebAuthenticator/)

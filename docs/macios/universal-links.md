---
title: "Apple universal links"
description: "Learn how to use deep linking functionality in a .NET MAUI iOS app."
ms.date: 02/20/2024
---

# Apple universal links

It's often desirable to connect a website and a mobile app so that links on a website launch the mobile app and display the relevant content in the mobile app. *App linking*, which is also known as *deep linking*, is a technique that enables a mobile device to respond to a URI and launch a mobile app that corresponds to the URI.

On Apple platforms, deep links are known as *universal links*. When a user taps on a universal link, the system redirects the link directly to your app without routing Safari or your website. These links can be based on a custom scheme, such as `myappname://`, or can use the `http` or `https` scheme. For example, clicking on a link on a recipe website would open a mobile app that's associated with that website and display a specific recipe to the user. Users who don't have your app installed are taken to content on your website. This article focuses on universal links that use the HTTPS scheme.

.NET MAUI iOS apps support universal links. This requires hosting a digital assets links JSON file on the domain, which describes the relationship with your app. This enables Apple to verify that the app trying to handle a URI has ownership of the URIs domain to prevent malicious apps from intercepting your app links.

The process for handling Apple universal links in a .NET MAUI iOS app is as follows:

- Create and host an associated domains file. For more information, see [Create and host an associated domains file](#create-and-host-an-associated-domains-file).
- Add the associated domains entitlement to your app. For more information, see [Add the associated domains entitlement to your app](#add-the-associated-domains-entitlement-to-your-app).
- Update your app delegate to respond to the user activity object the system provides when a universal link routes to your app. For more information, see [Update your app delegate](#update-your-app-delegate).

For more information, see [Allowing apps and websites to link to your content](https://developer.apple.com/documentation/xcode/allowing-apps-and-websites-to-link-to-your-content) on developer.apple.com. For information about defining a custom URL scheme for your app, see [Defining a custom URL scheme for your app](https://developer.apple.com/documentation/xcode/defining-a-custom-url-scheme-for-your-app) on developer.apple.com.

## Create and host an associated domains file

To associate a website with your app, you'll need to have an associated domain file on your website along with the appropriate entitlement in your app. When a user installs your app, iOS attempts to download the associated domain file and verify the domains in your entitlement.

The associated domain file is a JON file that must be hosted by the relevant web domain at the following location: `https://domain.name/.well-known/apple-app-site-association`.

The following JSON shows the contents of an associated domains file:

```json
{
    "activitycontinuation": {
        "apps": [ "85HMA3YHJX.com.companyname.myrecipeapp" ]
    },
    "applinks": {
        "apps": [],
        "details": [
            {
                "appID": "85HMA3YHJX.com.companyname.myrecipeapp",
                "paths": [ "*", "/*" ]
            }
        ]
    }
}
```

The `apps` and `appID` keys specify the app identifiers for the apps that are available for use on the website. The values for these keys are based on the app identifier prefix and the bundle identifier.

> [!IMPORTANT]
> The associated domain file must be hosted using `https` with a valid certificate and no redirects.

For more information, see [Supporting associated domains](https://developer.apple.com/documentation/xcode/supporting-associated-domains) on developer.apple.com.

## Add the associated domains entitlement to your app

You'll need to add the associated domains entitlement to your app, which specifies a list of domains that the app is associated with. This can be accomplished by adding an *Entitlements.plist* file to your app. For more information about adding an entitlement on iOS, see [Entitlements](~/ios/entitlements.md). For more information about adding an entitlement on Mac Catalyst, see [Entitlements](~/mac-catalyst/entitlements.md).

The entitlement is defined using the `com.apple.developer.associated-domains` key, of type `Array` of `String`:

```xml
<key>com.apple.developer.associated-domains</key>
<array>
  <string>applinks:recipe-app.com</string>
</array>
```

For more information about this entitlement, see [Associated domains entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_associated-domains) on developer.apple.com.

Alternatively, you can modify your project file (*.csproj) to add the entitlement in an `<ItemGroup>` element:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios' Or $([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">

    <!-- For debugging, use '?mode=developer' for debug to bypass apple's CDN cache -->
    <CustomEntitlements
        Condition="$(Configuration) == 'Debug'"
        Include="com.apple.developer.associated-domains"
        Type="StringArray"
        Value="applinks:recipe-app.com?mode=developer" />

    <!-- Non-debugging, use normal applinks:url value -->
    <CustomEntitlements
        Condition="$(Configuration) != 'Debug'"
        Include="com.apple.developer.associated-domains"
        Type="StringArray"
        Value="applinks:recipe-app.com" />

</ItemGroup>
```

Replace the `applinks:recipe-app.com` with the correct value for your own domain. Ensure you only include the desired subdomain and the top-level domain. Don't include path and query components or a trailing slash (`/`).

> [!NOTE]
> In iOS 14+ and macOS 11+, apps no longer send requests for `apple-app-site-association` files directly to your web server. Instead, they send requests to an Apple-managed content delivery network (CDN) dedicated to associated domains.

## Update your app delegate

When a user activates a universal link, iOS launches your app and sends it an <xref:Foundation.NSUserActivity> object. This object can be queried to determine how your app launched, and to determine what action to take. This should be performed in an early lifecycle callback such as `fsdfsd`.

To respond to an iOS lifecycle delegate being invoked, call the `ConfigureLifecycleEvents` method on the `MauiAppBuilder` object in the `CreateMauiapp` method of your `MauiProgram` class. Then, on the `ILifecycleBuilder` object, call the `AddiOS` method and specify the `Action` that registers a handler for the required callback:

```csharp
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.Logging;

namespace MyNamespace;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if IOS || MACCATALYST
                lifecycle.AddiOS(ios =>
                {
                    // Universal link delivered to FinishedLaunching after app launch.
                    ios.FinishedLaunching((app, data) => HandleAppLink(app.UserActivity));

                    // Universal link delivered to ContinueUserActivity when the app is running or suspended.
                    ios.ContinueUserActivity((app, userActivity, handler) => HandleAppLink(userActivity));

                    // Only required if using Scenes for multi-window support.
                    if (OperatingSystem.IsIOSVersionAtLeast(13) || OperatingSystem.IsMacCatalystVersionAtLeast(13))
                    {
                        // Universal link delivered to SceneWillConnect after app launch
                        ios.SceneWillConnect((scene, sceneSession, sceneConnectionOptions)
                            => HandleAppLink(sceneConnectionOptions.UserActivities.ToArray()
                                .FirstOrDefault(a => a.ActivityType == Foundation.NSUserActivityType.BrowsingWeb)));

                        // Universal link delivered to SceneContinueUserActivity when the app is running or suspended
                        ios.SceneContinueUserActivity((scene, userActivity) => HandleAppLink(userActivity));
                    }
                });
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

#if IOS || MACCATALYST
    static bool HandleAppLink(Foundation.NSUserActivity? userActivity)
    {
        if (userActivity is not null && userActivity.ActivityType == Foundation.NSUserActivityType.BrowsingWeb && userActivity.WebPageUrl is not null)
        {
            HandleAppLink(userActivity.WebPageUrl.ToString());
            return true;
        }
        return false;
    }
#endif

    static void HandleAppLink(string url)
    {
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            App.Current?.SendOnAppLinkRequestReceived(uri);
    }
}
```

When iOS opens your app as a result of a universal link, the <xref:Foundation.NSUserActivity> object will have an <xref:Foundation.NSUserActivity.ActivityType> property with a value of <xref:Foundation.NSUserActivityType.BrowsingWeb>. The activity object's <xref:Foundation.NSUserActivity.WebPageUrl> property will contain the URL that the user wants to access. The URL can be passed to your `App` class with the <xref:Microsoft.Maui.Controls.Application.SendOnAppLinkRequestReceived%2A> method.

> [!NOTE]
> If you aren't using Scenes in your app for multi-window support, you can omit the lifecycle handlers for the Scene methods.

In your `App` class, override the <xref:Microsoft.Maui.Controls.Application.OnAppLinkRequestReceived%2A> method to receive and process the intent data:

```csharp
namespace MyNamespace;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override async void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);

        // Show an alert to test that the app link was received.
        await Dispatcher.DispatchAsync(async () =>
        {
            await Windows[0].Page!.DisplayAlert("App link received", uri.ToString(), "OK");
        });

        Console.WriteLine("App link: " + uri.ToString());
    }
}
```

In the example above, the <xref:Microsoft.Maui.Controls.Application.OnAppLinkRequestReceived%2A> override displays the intent data. In practice, the app link should take users directly to the content represented by the URI, without any prompts, logins, or other interruptions. Therefore, the <xref:Microsoft.Maui.Controls.Application.OnAppLinkRequestReceived%2A> override is the location from which to invoke navigation to the content represented by the URI.

> [!WARNING]
> Universal links offer a potential attack vector into your app, so ensure you validate all URL parameters and discard any malformed URLs.

For more information, see [Supporting Universal Links in your app](https://developer.apple.com/documentation/xcode/supporting-universal-links-in-your-app) on developer.apple.com.

## Test a universal link

> [!IMPORTANT]
> Universal links should be tested on a device, rather than on a Simulator.

To test a universal link, paste a link into your Notes app and long-press it (on iOS) or control-click it (on macOS) to discover your choices for following the link. Provided that universal links have been correctly configured, the choice to open in app and in Safari will appear. Your choice will set the default behavior on your device when following universal links from this domain. To change this default choice, repeat the steps and make a different choice.

> [!NOTE]
> Entering the URL into Safari will never open the app. Instead, Safari will accept this action as direct navigation. Provided that a user is on your domain after navigating there directly, your site will show a banner to open your app.

On iOS, you can test your universal links with the associated domains diagnostic tests in developer settings:

1. Enable developer mode in Settings. For more information, see [Enabling Developer Mode on a device](https://developer.apple.com/documentation/xcode/enabling-developer-mode-on-a-device) on developer.apple.com.
1. In **Settings > Developer**, scroll to the **Universal Links** and enable **Associated Domains Development**.
1. Open **Diagnostics** and type in your URL. You'll then receive feedback on whether the link is valid for an installed app.

Often, invalid universal links are a result of your `applinks` being incorrectly configured.

For troubleshooting advice, see [Debugging universal links](https://developer.apple.com/documentation/technotes/tn3155-debugging-universal-links) on developer.apple.com.

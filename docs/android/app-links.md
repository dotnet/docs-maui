---
title: "Android app links"
description: "Learn how to use deep linking functionality in a .NET MAUI Android app."
ms.date: 02/20/2024
---

# Android app links

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platforms-deeplinking)

It's often desirable to connect a website and a mobile app so that links on a website launch the mobile app and display the relevant content in the mobile app. *App linking*, which is also known as *deep linking*, is a technique that enables a mobile device to respond to a URI and launch a mobile app that corresponds to the URI.

Android handles app links through the intent system. When a user taps on a link in a mobile browser, the browser will dispatch an intent that Android will delegate to a registered app. These links can be based on a custom scheme, such as `myappname://`, or can use the `http` or `https` scheme. For example, clicking on a link on a recipe website would open a mobile app that's associated with that website and display a specific recipe to the user. If there's more than one app registered to handle the intent, Android will display a disambiguation dialog that asks the user which app to select to handle the intent. Users who don't have your app installed are taken to content on your website.

Android classifies app links into three categories:

- *Deep links* are URIs of any scheme that take users to specific content in your app. When a deep link is clicked, a disambiguation dialog may appear that asks the user to select an app to handle the deep link.
- *Web links* are deep links that use the HTTP or HTTPS scheme. On Android 12 and higher, a web link always shows content in a web browser. On previous versions of Android, if an app can handle the web link then a disambiguation dialog will appear that asks the user to select an app to handle the web link.
- *Android app links*, which are available on API 23+, are web links that use the HTTP or HTTPS scheme and contain the `autoVerify` attribute. This attribute enables your app to become the default handler for an app link. Therefore, when a user clicks on an app link your app opens without displaying a disambiguation dialog.

.NET MAUI Android apps can support all three categories of app links. However, this article focuses on Android app links. This requires proving ownership of a domain as well as hosting a digital assets links file JSON file on the domain, which describes the relationship with your app. This enables Android to verify that the app trying to handle a URI has ownership of the URIs domain to prevent malicious apps from intercepting your app links.

The process for handling Android app links in a .NET MAUI Android app is as follows:

1. Verify domain ownership. For more information, see [Verify domain ownership](#verify-domain-ownership).
1. Create and host a digital assets links file. For more information, see [Create and host a digital assets links file](#create-and-host-a-digital-assets-links-file).
1. Configure the intent filter for your website URIs. For more information, see [Configure the intent filter](#configure-the-intent-filter).
1. Read the data from the incoming intent. For more information, see [Read the data from the incoming intent](#read-the-data-from-the-incoming-intent).

> [!IMPORTANT]
> To use Android app links:
> - A version of your app must be live on Google Play.
> - A companion website must be registered against the app in Google's Developer Console. Once the app is associated with a website, URIs can be indexed that work for both the website and the app, which can then be served in search results. For more information, see [App Indexing on Google Search](https://support.google.com/googleplay/android-developer/answer/6041489) on support.google.com.

For more information about Android app links, see [Handling Android App Links](https://developer.android.com/training/app-links).

## Verify domain ownership

You'll be required to verify your ownership of the domain you're serving app links from in the [Google Search Console](https://search.google.com/search-console). Ownership verification means proving that you own a specific website. Google Search Console supports multiple verification approaches. For more information, see [Verify your site ownership](https://support.google.com/webmasters/answer/9008080) on support.google.com.

## Create and host a digital assets links file

Android app links require that Android verify the association between the app and the website before setting the app as the default handler for the URI. This verification will occur when the app is first installed. The digital assets links file is a JSON file that must be hosted by the relevant web domain at the following location: `https://domain.name/.well-known/assetlinks.json`.

> [!IMPORTANT]
> The `AutoVerify` property must be set to `true` in the <xref:Android.App.IntentFilterAttribute> otherwise Android won't perform verification.

The digital asset file contains the metadata necessary for Android to verify the association. A *assetlinks.json* file requires the following key-value pairs:

- `namespace` - the namespace of the Android app.
- `package_name` - the package name of the Android app.
- `sha256_cert_fingerprints` - the SHA256 fingerprints of the signed app, obtained from your `.keystore` file. For information about finding your keystore's signature, see [Find your keystore's signature](~/android/deployment/publish-cli.md#find-your-keystores-signature).

The following example *assetlinks.json* file grants link-opening rights to a `com.companyname.myrecipeapp` Android app:

```json
[
   {
      "relation": [
         "delegate_permission/common.handle_all_urls"
      ],
      "target": {
         "namespace": "android_app",
         "package_name": "com.companyname.myrecipeapp",
         "sha256_cert_fingerprints": [
            "14:6D:E9:83:C5:73:06:50:D8:EE:B9:95:2F:34:FC:64:16:A0:83:42:E6:1D:BE:A8:8A:04:96:B2:3F:CF:44:E5"
         ]
      }
   }
]
```

It's possible to register more than one SHA256 fingerprint to support different versions or builds of your app. The following *assetlinks.json* file grants link-opening rights to both the `com.companyname.myrecipeapp` and `com.companyname.mycookingapp` Android apps:

```json
[
   {
      "relation": [
         "delegate_permission/common.handle_all_urls"
      ],
      "target": {
         "namespace": "android_app",
         "package_name": "com.companyname.myrecipeapp",
         "sha256_cert_fingerprints": [
            "14:6D:E9:83:C5:73:06:50:D8:EE:B9:95:2F:34:FC:64:16:A0:83:42:E6:1D:BE:A8:8A:04:96:B2:3F:CF:44:E5"
         ]
      }
   },
   {
      "relation": [
         "delegate_permission/common.handle_all_urls"
      ],
      "target": {
         "namespace": "android_app",
         "package_name": "com.companyname.mycookingapp",
         "sha256_cert_fingerprints": [
            "14:6D:E9:83:C5:73:06:50:D8:EE:B9:95:2F:34:FC:64:16:A0:83:42:E6:1D:BE:A8:8A:04:96:B2:3F:CF:44:E5"
         ]
      }
   }
]
```

> [!TIP]
> Use the [Statement List Generator and Tester](https://developers.google.com/digital-asset-links/tools/generator) tool to help generate the correct JSON, and to validate it.

When publishing your JSON verification file to `https://domain.name/.well-known/assetlinks.json`, you must ensure that:

- The file is served with content-type `application/json`.
- The file must be accessible over HTTPS, regardless of whether your app uses HTTPS as the scheme.
- The file must be accessible without redirects.
- If your app links support multiple domains, then you must publish the `assetlinks.json` file on each domain.

For more information, see [Declare website associations](https://developer.android.com/training/app-links/verify-android-applinks#web-assoc) on developer.android.com.

You can confirm that the digital assets file is properly formatted and hosted by using Google's digital asset links API:

```html
https://digitalassetlinks.googleapis.com/v1/statements:list?source.web.site=
  https://<WEB SITE ADDRESS>:&relation=delegate_permission/common.handle_all_urls
```

## Configure the intent filter

An intent filter that maps a URI, or set of URIs, from a website to an activity in the Android app must be configured. In .NET MAUI, this relationship is established by adding the <xref:Android.App.IntentFilterAttribute> to your activity. The intent filter must declare the following information:

- `Intent.ActionView` - this will register the intent filter to respond to requests to view information.
- `Categories` - the intent filter should register both <xref:Android.Content.Intent.CategoryDefault> and <xref:Android.Content.Intent.CategoryBrowsable> to be able to correctly handle the web URI.
- `DataScheme` - the intent filter must declare a custom scheme, and/or `http` and/or `https`.
- `DataHost` - this is the domain from which the URIs will originate.
- `DataPathPrefix` - this is an optional path to resources on the website, which must begin with a `/`.
- `AutoVerify` - this attribute tells Android to verify the relationship between the app and the website.

The following example shows how to use the <xref:Android.App.IntentFilterAttribute> to handle links from `https://www.recipe-app.com/recipes`:

```csharp
using Android.App;
using Android.Content;
using Android.Content.PM;

namespace MyNamespace;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.KeyboardHidden |
        ConfigChanges.Density)]
[IntentFilter(
    new string[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "https",
    DataHost = "recipe-app.com",
    DataPath = "/recipe",
    AutoVerify = true,)]    
public class MainActivity : MauiAppCompatActivity
{
}
```

Android will verify every host that's identified in the intent filters against the digital assets file on the website, before registering the app as the default handler for a URI. All  the intent filters must pass verification before Android can establish the app as the default handler.

> [!NOTE]
> Multiple schemes and hosts can be specified in your intent filter. For more information, see [Create Deep Links to App Content](https://developer.android.com/training/app-links/deep-linking) on developer.android.com.

It may also be necessary to mark your activity as exportable, so that your activity can be launched by other apps. This can be achieved by adding `Exported = true` to the existing `Activity` attribute. For more information, see [Activity element](https://developer.android.com/guide/topics/manifest/activity-element) on developer.android.com.

Once you've added an intent filter with a URI for activity content, Android is able to route any intent that has matching URIs to your app at runtime.

When a web URI intent is invoked Android tries the following actions until the request succeeds:

1. Opens the preferred app to handle the URI.
1. Opens the only available app to handle the URI.
1. Allows the user to select an app to handle the URI.

For more information about intents and intent filters, see [Intents and intent filters](https://developer.android.com/guide/components/intents-filters) on developer.android.com.

## Read the data from the incoming intent

When Android starts your activity through an intent filter, you can use the data provided by the intent to determine what to do. This should be performed in an early lifecycle callback such as `OnCreate`.

To respond to an Android lifecycle delegate being invoked, call the `ConfigureLifecycleEvents` method on the `MauiAppBuilder` object in the `CreateMauiapp` method of your `MauiProgram` class. Then, on the `ILifecycleBuilder` object, call the `AddAndroid` method and specify the `Action` that registers a handler for the required callback:

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
#if ANDROID
                lifecycle.AddAndroid(android =>
                {
                    android.OnCreate((activity, bundle) =>
                    {
                        var action = activity.Intent?.Action;
                        var data = activity.Intent?.Data?.ToString();

                        if (action == Android.Content.Intent.ActionView && data is not null)
                        {
                            activity.Finish();
                            Task.Run(() => HandleAppLink(data));
                        }
                    });
                });
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    static void HandleAppLink(string url)
    {
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            App.Current?.SendOnAppLinkRequestReceived(uri);
    }
}
```

The `Intent.Action` property retrieves the action associated with the incoming intent, and the `Intent.Data` property retrieves the data associated with the incoming intent. Provided that the intent action is set to `ActionView`, the intent data can be passed to your `App` class with the <xref:Microsoft.Maui.Controls.Application.SendOnAppLinkRequestReceived%2A> method.

> [!WARNING]
> App links offer a potential attack vector into your app, so ensure you validate all URL parameters and discard any malformed URLs.

In your `App` class, override the <xref:Microsoft.Maui.Controls.Application.OnAppLinkRequestReceived%2A> method to process the intent data:

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

In the example above the <xref:Microsoft.Maui.Controls.Application.OnAppLinkRequestReceived%2A> override displays the intent data. In practice, the app link should take users directly to the content represented by the URI, without any prompts, logins, or other interruptions. Therefore, the <xref:Microsoft.Maui.Controls.Application.OnAppLinkRequestReceived%2A> override is the location from which to perform navigation to the content represented by the URI.

## Test a URI

Provided that the digital asset file is correctly hosted, you can use the Android Debug Bridge, `adb`, with the activity manager tool, `am`, to simulate opening a URI to ensure that your app links work correctly. For example, the following command tries to view a target app activity that's associated with a URI:

```shell
adb shell am start -W -a android.intent.action.VIEW -c android.intent.category.BROWSABLE -d YOUR_URI_HERE
```

This command will dispatch an intent that Android should direct to your mobile app, which should launch and display the activity registered for the URI.

> [!NOTE]
> You can run `adb` against an emulator or a device.

In addition, you can display the existing link handling policies for the apps installed on a device:

```shell
adb shell dumpsys package domain-preferred-apps
```

This command will display the following information:

- Package - the package name of the app.
- Domain - the domains, separated by spaces, whose web links will be handled by the app.
- Status - the current link handling status for the app. A value of `always` means that the app has set `AutoVerify` to `true` and has passed system verification. It's followed by a hexadecimal number representing the Android system's record of the preference.

For more information about the `adb` command, see [Android Debug Bridge](https://developer.android.com/tools/adb).

In addition, you can manage and verify Android app links through the [Play Console](https://play.google.com/console). For more information, see [Manage and verify Android App Links](https://developer.android.com/training/app-links) on developer.android.com.

For troubleshooting advice, see [Fix common implementation errors](https://developer.android.com/training/app-links/verify-android-applinks#fix-errors) on developer.android.com.

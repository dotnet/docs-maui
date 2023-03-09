---
title: "Migrate Xamarin.Essentials code in .NET for Android and .NET for iOS apps"
description: "Learn how to migrate your Xamarin.Essentials code in .NET for iOS and .NET for Android apps."
ms.date: 03/09/2023
no-loc: [ "Xamarin.Essentials" ]
---

# Migrate Xamarin.Essentials code in .NET for Android and .NET for iOS apps

Xamarin.Essentials is a fundamental library for nearly every Xamarin app, and it's features are now part of .NET Multi-platform App UI (.NET MAUI).

To make use of .NET MAUIs cross-platform APIs for native device features, that were formerly known as Xamarin.Essentials, use the following process:

1. Remove the Xamarin.Essentials NuGet package.
1. Modify your project file to Add `<UseMauiEssentials>true</UseMauiEssentials>` to your .csproj.
1. Call the `Init` method to initialize the functionality.
1. Call additional methods as required.
1. Adjust namespaces.

## Modify your project file

To use .NET MAUIs "essentials" features in your .NET for iOS or .NET for Android app, modify your project file and set the `$(UseMauiEssentials)` build property to `true`.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)
<!-- markdownlint-enable MD025 -->

Add `<UseMauiEssentials>true</UseMauiEssentials>` to the first `<PropertyGroup>` in your project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net7.0-android</TargetFramework>
    <SupportedOSPlatformVersion>21</SupportedOSPlatformVersion>
    <OutputType>Exe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <ApplicationId>com.companyname.MyAndroidApp</ApplicationId>
    <ApplicationVersion>1</ApplicationVersion>
    <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
    <UseMauiEssentials>true</UseMauiEssentials>
  </PropertyGroup>
</Project>
```

<!-- markdownlint-disable MD025 -->
# [iOS](#tab/ios)
<!-- markdownlint-enable MD025 -->

On iOS, add `<UseMauiEssentials>true</UseMauiEssentials>` to the first `<PropertyGroup>` in your project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net7.0-ios</TargetFramework>
    <OutputType>Exe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>true</ImplicitUsings>
    <SupportedOSPlatformVersion>13.0</SupportedOSPlatformVersion>
    <UseMauiEssentials>true</UseMauiEssentials>
  </PropertyGroup>
</Project>
```

## Initialize

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)
<!-- markdownlint-enable MD025 -->

In any <xref:Android.App.Activity> that's launched, you must call the `Platform.Init` method, which is in the <xref:Microsoft.Maui.ApplicationModel> namespace, from the `OnCreate` method:

```csharp
using Android.Content.PM;
using Android.Runtime;
using Microsoft.Maui.ApplicationModel;

namespace MyAndroidApp;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    protected override async void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);
        // ...  
    }
}
```

The `Platform.Init` method requires an <xref:Android.App.Application> argument, or a <xref:Android.App.Activity> argument and a <Android.OS.Bundle> argument.

<!-- markdownlint-disable MD025 -->
# [iOS](#tab/ios)
<!-- markdownlint-enable MD025 -->

In your `AppDelegate` class, call the `Platform.Init` method from the `FinishedLaunching` method:

```csharp
using Microsoft.Maui.ApplicationModel;

namespace MyiOSApp;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        // create a UIViewController with a single UILabel
        var vc = new UIViewController();
        vc.View!.AddSubview(new UILabel(Window!.Frame)
        {
            BackgroundColor = UIColor.SystemBackground,
            TextAlignment = UITextAlignment.Center,
            Text = "Hello, iOS!",
            AutoresizingMask = UIViewAutoresizing.All,
        });
        Window.RootViewController = vc;

        Platform.Init(() => vc);

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;
    }
}
```

The `Platform.Init` method requires a `Func<UIKit.UIViewController` argument.

> [!NOTE]
> If required, you can retrieve the current `UIViewController` object by calling the `Platform.GetCurrentUIViewController` method.

## Other methods

The static `Platform` class contains additional members on each platform that may be required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)
<!-- markdownlint-enable MD025 -->

| Member | Purpose |
| ------ | ------- |
| `ActivityStateChanged` | An event that's raised when any Activity's state changes. |
| `AppContext` | A property that gets the <xref:Android.Content.Context> object that represents the current app context. |
| `CurrentActivity` | A property that gets the current <xref:Android.App.Activity> object that represents the current activity. |
| `Intent` | A static class that contains the `ActionAppAction` string, which is the identifier for the <xref:Android.Content.Intent> used by app actions. |
| `OnNewIntent` | A method to be called to pass an <xref:Android.Content.Intent> from an activity's overridden method, when invoking an app action. |
| `OnResume` | A method to be called to pass an <xref:Android.App.Activity> from an activity's overridden method, when an <xref:Android.App.Activity> is resumed as part of invoking an app action. |
| `OnRequestPermissionsResult` | A method to be called to pass permission request results from an activity's overridden method, for handling internal permission requests. |
| `WaitForActivityAsync` | Wait for an Activity to be created or active.  |

For example, to handle runtime permission requests, override the `OnRequestPermissionsResult` method in every <xref:Android.App.Activity> and call the `Platform.OnRequestPermissionsResult` method from it:

```csharp
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
{
    Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}
```

<!-- markdownlint-disable MD025 -->
# [iOS](#tab/ios)
<!-- markdownlint-enable MD025 -->

| Method | Purpose |
| ------ | ------- |
| `ContinueUserActivity` | Informs the app that there's data associated with continuing a task specified as a <xref:Foundation.NSUserActivity"> object, and then returns whether the app continued the activity. |
| `GetCurrentUIViewController` | Gets the current view controller. |
| `OpenUrl` | Opens the specified URI to start an authentication flow. |
| `PerformActionForShortcutItem` | Invokes the action that corresponds to the chosen <xref:UIKit.AppAction> by the user. |

For example, to handle app actions, override the `PerformActionForShortcutItem` method in your `AppDelegate` class and call the `Platform.PerformActionForShortcutItem` method from it:

```csharp
public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
{
    Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
}
```

## Add using directives

.NET MAUI projects make use of implicit `global using` directives that enable you to remove `using` directives for the `Xamarin.Essentials` namespace, without having to replace them with the equivalent .NET MAUI namespaces. However, the implicit `global using` directives for .NET for iOS and .NET for Android don't include the namespaces for the Xamarin.Essentials functionality. Therefore, `using` directives for the `Xamarin.Essentials` namespace should be replaced with `using` directives for the namespace the functionality is now in:

| Namespace | Purpose |
| --------- | ------- |
| <xref:Microsoft.Maui.ApplicationModel> | Application model functionality, including app actions, permissions, and version tracking. |
| <xref:Microsoft.Maui.ApplicationModel.Communication> | Communication functionality, including contacts, email, and networking.  |
| <xref:Microsoft.Maui.Devices> | Device functionality, including battery, sensors, flashlight, and haptic feedback. |
| <xref:Microsoft.Maui.Media> | Media functionality, including media picking, and text-to-speech. |
| <xref:Microsoft.Maui.ApplicationModel.DataTransfer> | Sharing functionality, including the clipboard, and file sharing. |
| <xref:Microsoft.Maui.Storage> | Storage functionality, including file picking, and secure storage.  |

For more information about the functionality in each namespace, see [Platform integration](~/platform-integration/index.md).

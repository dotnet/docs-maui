---
title: "Migrate Xamarin.Essentials code in .NET Android and .NET iOS apps"
description: "Learn how to migrate your Xamarin.Essentials code in .NET iOS and .NET Android apps."
ms.date: 03/09/2023
no-loc: [ "Xamarin.Essentials", ".NET MAUI" ]
---

# Migrate Xamarin.Essentials code in .NET Android and .NET iOS apps

Xamarin.Essentials is a fundamental library for nearly every Xamarin app, and its functionality is now part of .NET Multi-platform App UI (.NET MAUI).

The following steps outline the process to use .NET MAUI's native device functionality, formerly known as Xamarin.Essentials, in a .NET Android or .NET iOS app:

1. Remove the Xamarin.Essentials NuGet package from your .NET Android or .NET iOS app.
1. Set the `$(UseMauiEssentials)` build property to `true` in your project file. For more information, see [Modify your project file](#modify-your-project-file).
1. Initialize the "essentials" functionality by calling the `Platform.Init` method. For more information, see [Initialize the platform](#initialize-the-platform).
1. Perform additional setup, if necessary. For more information, see [Perform additional setup](#perform-additional-setup).
1. Add using directives for the required functionality. For more information, see [Add using directives](#add-using-directives).

> [!IMPORTANT]
> No action is required to use Xamarin.Essentials in a .NET MAUI app, other than removing references to the `Xamarin.Essentials` namespace, because .NET MAUI already includes the functionality from Xamarin.Essentials.

## Modify your project file

To use .NET MAUIs native device functionality in a .NET Android or .NET iOS app, modify your project file and set the `$(UseMauiEssentials)` build property to `true`.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)
<!-- markdownlint-enable MD025 -->

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-android</TargetFramework>
    ...
    <UseMauiEssentials>true</UseMauiEssentials>
  </PropertyGroup>
</Project>
```

<!-- markdownlint-disable MD025 -->
# [iOS](#tab/ios)
<!-- markdownlint-enable MD025 -->

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-ios</TargetFramework>
    ...
    <UseMauiEssentials>true</UseMauiEssentials>
  </PropertyGroup>
</Project>
```

---

## Initialize the platform

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

The `Platform.Init` method requires an <xref:Android.App.Application> argument, or a <xref:Android.App.Activity> argument and a <xref:Android.OS.Bundle> argument.

<!-- markdownlint-disable MD025 -->
# [iOS](#tab/ios)
<!-- markdownlint-enable MD025 -->

In your `AppDelegate` class, you must call the `Platform.Init` method, which is in the <xref:Microsoft.Maui.ApplicationModel> namespace, from the `FinishedLaunching` method:

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
        // ...
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
> If required, you can retrieve the current <xref:UIKit.UIViewController> object by calling the `Platform.GetCurrentUIViewController` method.

---

## Perform additional setup

The static `Platform` class contains platform-specific helpers.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)
<!-- markdownlint-enable MD025 -->

| Member | Purpose |
| ------ | ------- |
| `ActivityStateChanged` | An event that's raised when any Activity's state changes. |
| `AppContext` | A property that gets the <xref:Android.Content.Context> object that represents the current app context. |
| `CurrentActivity` | A property that gets the current <xref:Android.App.Activity> object that represents the current activity. |
| `Intent` | A static class that contains the `ActionAppAction` string, which is the identifier for the <xref:Android.Content.Intent> used by app actions. |
| `OnNewIntent` | Pass an <xref:Android.Content.Intent> from an activity's overridden method, when invoking an app action. |
| `OnResume` | Pass an <xref:Android.App.Activity> from an activity's overridden method, when an <xref:Android.App.Activity> is resumed as part of invoking an app action. |
| `OnRequestPermissionsResult` | Pass permission request results from an activity's overridden method, for handling internal permission requests. |
| `WaitForActivityAsync` | Wait for an <xref:Android.App.Activity> to be created or become active. |

To access the current `Context` or `Activity` for the running app:

```csharp
var context = Platform.AppContext;

// Current Activity or null if not initialized or not started.
var activity = Platform.CurrentActivity;
```

If there's a situation where the <xref:Android.App.Activity> is needed, but the app hasn't fully started, call the `WaitForActivityAsync` method:

```csharp
var activity = await Platform.WaitForActivityAsync();
```

To handle runtime permission requests, override the `OnRequestPermissionsResult` method in every <xref:Android.App.Activity> and call the `Platform.OnRequestPermissionsResult` method from it:

```csharp
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
{
    Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}
```

In addition to getting the current <xref:Android.App.Activity>, you can also register for lifecycle events:

```csharp
protected override void OnCreate(Bundle bundle)
{
    base.OnCreate(bundle);
    Platform.Init(this, bundle);
    Platform.ActivityStateChanged += Platform_ActivityStateChanged;
}

protected override void OnDestroy()
{
    base.OnDestroy();
    Platform.ActivityStateChanged -= Platform_ActivityStateChanged;
}

void Platform_ActivityStateChanged(object sender, ActivityStateChangedEventArgs e) =>
    Toast.MakeText(this, e.State.ToString(), ToastLength.Short).Show();
```

Activity states are:

- Created
- Resumed
- Paused
- Destroyed
- SaveInstanceState
- Started
- Stopped

<!-- markdownlint-disable MD025 -->
# [iOS](#tab/ios)
<!-- markdownlint-enable MD025 -->

| Method | Purpose |
| ------ | ------- |
| `ContinueUserActivity` | Informs the app that there's data associated with continuing a task specified as a <xref:Foundation.NSUserActivity> object, and then returns whether the app continued the activity. |
| `GetCurrentUIViewController` | Gets the current view controller. This method returns `null` if unable to detect a <xref:UIKit.UIViewController>. |
| `OpenUrl` | Opens the specified URI to start an authentication flow. |
| `PerformActionForShortcutItem` | Invokes the action that corresponds to the chosen `AppAction` by the user. |

For example, to handle app actions, override the `PerformActionForShortcutItem` method in your `AppDelegate` class and call the `Platform.PerformActionForShortcutItem` method from it:

```csharp
public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
{
    Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
}
```

---

## Add using directives

The implicit `global using` directives for .NET iOS and .NET Android don't include the namespaces for .NET MAUIs native device functionality. Therefore, `using` directives for the `Xamarin.Essentials` namespace should be replaced with `using` directives for the namespace that contains the required functionality:

| Namespace | Purpose |
| --------- | ------- |
| <xref:Microsoft.Maui.ApplicationModel> | Application model functionality, including app actions, permissions, and version tracking. |
| <xref:Microsoft.Maui.ApplicationModel.Communication> | Communication functionality, including contacts, email, and networking.  |
| <xref:Microsoft.Maui.Devices> | Device functionality, including battery, sensors, flashlight, and haptic feedback. |
| <xref:Microsoft.Maui.Media> | Media functionality, including media picking, and text-to-speech. |
| <xref:Microsoft.Maui.ApplicationModel.DataTransfer> | Sharing functionality, including the clipboard, and file sharing. |
| <xref:Microsoft.Maui.Storage> | Storage functionality, including file picking, and secure storage.  |

For more information about the functionality in each namespace, see [Platform integration](~/platform-integration/index.md).

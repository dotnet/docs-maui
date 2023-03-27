---
title: "Platform helpers"
description: "DASDSA"
ms.date: 03/28/2023
zone_pivot_groups: devices-deployment
---

# Platform helpers

The static `Platform` class, in the <xref:Microsoft.Maui.ApplicationModel> namespace, contains platform-specific helpers.

:::zone pivot="devices-android"

The `Platform` class contains the following members on Android:

| Member | Purpose |
| ------ | ------- |
| `ActivityStateChanged` | An event that's raised when any Activity's state changes. |
| `AppContext` | A property that gets the <xref:Android.Content.Context> object that represents the current app context. |
| `CurrentActivity` | A property that gets the current <xref:Android.App.Activity> object that represents the current activity. |
| `Intent` | A static class that contains the `ActionAppAction` string, which is the identifier for the <xref:Android.Content.Intent> used by app actions. |
| `OnNewIntent` | Pass an <xref:Android.Content.Intent> from an activity's overridden method, when invoking an app action. |
| `OnResume` | Pass an <xref:Android.App.Activity> from an activity's overridden method, when an <xref:Android.App.Activity> is resumed as part of invoking an app action. |
| `OnRequestPermissionsResult` | Pass permission request results from an activity's overridden method, for handling internal permission requests. |
| `WaitForActivityAsync` | Wait for an Activity to be created or become active. |

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

:::zone-end

:::zone pivot="devices-ios"

The `Platform` class contains the following members on iOS and Mac Catalyst:

| Method | Purpose |
| ------ | ------- |
| `ContinueUserActivity` | Informs the app that there's data associated with continuing a task specified as a <xref:Foundation.NSUserActivity> object, and then returns whether the app continued the activity. |
| `GetCurrentUIViewController` | Gets the current view controller. This method will return `null` if unable to detect a <xref:UIKit.UIViewController>. |
| `OpenUrl` | Opens the specified URI to start an authentication flow. |
| `PerformActionForShortcutItem` | Invokes the action that corresponds to the chosen `AppAction` by the user. |

For example, to handle app actions, override the `PerformActionForShortcutItem` method in your `AppDelegate` class and call the `Platform.PerformActionForShortcutItem` method from it:

```csharp
public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
{
    Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
}
```

:::zone-end

:::zone pivot="devices-windows"

The `Platform` class contains the following members on Windows:

| Method | Purpose |
| ------ | ------- |
| `MapServiceToken` | Gets or sets the map service API key. |
| `OnLaunched` | The lifecycle method that's called when the app is launched. |
| `OnActivated` | Sets the app's new active window. |

:::zone-end

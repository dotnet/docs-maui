---
title: "App actions (shortcuts)"
description: "Describes the IAppActions interface in the Microsoft.Maui.ApplicationModel namespace, which lets you create and respond to app shortcuts from the app icon."
ms.date: 10/08/2026
ai-usage: ai-assisted
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel", "AppDelegate.cs", "AppActions", "Platforms/Android/MainActivity.cs", "Platforms/iOS/AppDelegate.cs", "Platforms/Windows/App.xaml.cs", "Id", "Title", "Subtitle", "Icon"]
---

# App actions

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.IAppActions> interface, which lets you create and respond to app shortcuts. App shortcuts are helpful to users because they allow you, as the app developer, to present them with extra ways of starting your app. For example, if you were developing an email and calendar app, you could present two different app actions, one to open the app directly to the current day of the calendar, and another to open to the email inbox folder.

The default implementation of the `IAppActions` interface is available through the <xref:Microsoft.Maui.ApplicationModel.AppActions.Current?displayProperty=nameWithType> property. Both the `IAppActions` interface and `AppActions` class are contained in the `Microsoft.Maui.ApplicationModel` namespace.

## Get started

To access the `AppActions` functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

In the _Platforms/Android/MainActivity.cs_ file, add the `OnResume` and `OnNewIntent` overrides to the `MainActivity` class, and the following `IntentFilter` attribute:

:::code language="csharp" source="../snippets/shared_2/Platforms/Android/MainActivity.cs" id="intent_filter_1":::

# [iOS/Mac Catalyst](#tab/macios)

::: moniker range="<=net-maui-10.0"

No setup is required.

::: moniker-end

::: moniker range=">=net-maui-11.0"

No AppActions-specific setup is required when you use the .NET MAUI app templates. When you upgrade an existing app for the Xcode 27 SDKs, add the scene manifest and registered scene delegate to both Apple platform folders. See [Upgrade to the scene lifecycle](../../fundamentals/app-lifecycle.md#upgrade-to-the-scene-lifecycle).

::: moniker-end

# [Windows](#tab/windows)

No setup is required.

-----

<!-- markdownlint-enable MD025 -->

## Create actions

App actions can be created at any time, but are often created when an app starts. To configure app actions, invoke the <xref:Microsoft.Maui.Hosting.EssentialsExtensions.ConfigureEssentials(Microsoft.Maui.Hosting.MauiAppBuilder,System.Action{Microsoft.Maui.Hosting.IEssentialsBuilder})> method on the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object in the _MauiProgram.cs_ file. There are two methods you must call on the <xref:Microsoft.Maui.Hosting.IEssentialsBuilder> object to enable an app action:

01. <xref:Microsoft.Maui.Hosting.IEssentialsBuilder.AddAppAction%2A>

    This method creates an action. It takes an `id` string to uniquely identify the action, and a `title` string that's displayed to the user. You can optionally provide a subtitle and an icon.

01. <xref:Microsoft.Maui.Hosting.IEssentialsBuilder.OnAppAction%2A>

    The delegate passed to this method is called when the user invokes an app action, provided the app action instance. Check the `Id` property of the action to determine which app action was started by the user.

The following code demonstrates how to configure the app actions at app startup:

:::code language="csharp" source="../snippets/shared_1/MauiProgram.cs" id="bootstrap_appaction" highlight="12-18":::

## Responding to actions

After app actions [have been configured](#create-actions), the `OnAppAction` method is called for all app actions invoked by the user. Use the `Id` property to differentiate them. The following code demonstrates handling an app action:

:::code language="csharp" source="../snippets/shared_1/App.xaml.cs" id="appaction_handle":::

::: moniker range=">=net-maui-11.0"

### iOS and Mac Catalyst scene dispatch

.NET MAUI dispatches app actions through the existing `PerformActionForShortcutItem` lifecycle registrations for both application and scene lifecycles. With the default MAUI builder, the framework forwards these actions to AppActions. Continue to use `OnAppAction` to respond to them. Do not add manual `Platform.PerformActionForShortcutItem` forwarding to `AppDelegate`, `SceneDelegate`, or another lifecycle registration.

A warm shortcut selection reaches an existing scene through its native shortcut callback. A shortcut in `UISceneConnectionOptions.ShortcutItem` is dispatched after that window's scene activation callbacks, at most once per connection. This cold scene delivery requires no manual forwarding.

If you register a custom `PerformActionForShortcutItem` handler, invoke its completion callback exactly once. Each registration receives its own callback. Pass `true` if your handler handles the action, or `false` otherwise. A handler that only observes or logs the action must also call its callback with `false`, as in this registration in `MauiProgram.cs`:

```csharp
using Microsoft.Maui.LifecycleEvents;

builder.ConfigureLifecycleEvents(events =>
{
#if IOS || MACCATALYST
    events.AddiOS(ios => ios
        .PerformActionForShortcutItem((application, shortcutItem, completionHandler) =>
        {
            System.Diagnostics.Debug.WriteLine($"Shortcut: {shortcutItem.Type}");
            completionHandler(false);
        }));
#endif
});
```

A handler can defer its acknowledgement until asynchronous work finishes. Returning from the handler does not acknowledge the action. When dispatch does not throw, native completion waits until all handler invocations have returned and either one registration reports `true` or every registration reports `false`. If none reports `true`, a handler that never acknowledges can leave native warm-action completion pending. There is no automatic timeout.

A cold scene connection has no native completion callback, but each lifecycle registration must still acknowledge its own callback.

::: moniker-end

### Check if app actions are supported

When you create an app action, either at app startup or while the app is being used, check to see if app actions are supported by reading the [`AppActions.Current.IsSupported`](xref:Microsoft.Maui.ApplicationModel.AppActions.IsSupported) property.

### Create an app action outside of the startup bootstrap

To create app actions, call the <xref:Microsoft.Maui.ApplicationModel.AppActions.SetAsync%2A> method:

:::code language="csharp" source="../snippets/shared_2/MainPage.xaml.cs" id="app_actions":::

### More information about app actions

If app actions aren't supported on the specific version of the operating system, a <xref:Microsoft.Maui.ApplicationModel.FeatureNotSupportedException> will be thrown.

Use the <xref:Microsoft.Maui.ApplicationModel.AppAction.%23ctor(System.String,System.String,System.String,System.String)> constructor to set the following aspects of an app action:

- **Id**: A unique identifier used to respond to the action tap.
- **Title**: the visible title to display.
- **Subtitle**: If supported a subtitle to display under the title.
- **Icon**: Must match icons in the corresponding resources directory on each platform.

<!-- TODO icon in image needs update -->
:::image type="content" source="media/app-actions/appactions.png" alt-text="App actions on home screen.":::

## Get actions

You can get the current list of app actions by calling [`AppActions.Current.GetAsync`](xref:Microsoft.Maui.ApplicationModel.AppActions.GetAsync).

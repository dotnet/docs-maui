---
title: "App Actions"
description: "The AppActions class in Microsoft.Maui.Essentials lets you create and respond to app shortcuts from the app icon."
ms.date: 08/02/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials", "AppDelegate.cs", "AppActions"]
---

# App Actions

The `Microsoft.Maui.Essentials.AppActions` class lets you create and respond to app shortcuts from the app icon. App shortcuts are helpful to users because they allow you, as the app developer, to present them with additional ways of starting your app. For example, if you were developing an email and calendar app, you could present two different app actions, one to open the app directly to the current day of the calendar, and another to open to the email inbox folder.

## Get started

[!INCLUDE [get-started](../essentials/includes/get-started.md)]

To access the `AppActions` functionality the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

Add the intent filter to your `MainActivity` class:

```csharp
[IntentFilter(new[] { Microsoft.Maui.Essentials.Platform.Intent.ActionAppAction },
              Categories = new[] { Android.Content.Intent.CategoryDefault })]
public class MainActivity : MauiAppCompatActivity
{
    ...
}
```

Then add the following logic to handle actions:

```csharp
protected override void OnResume()
{
    base.OnResume();

    Microsoft.Maui.Essentials.Platform.OnResume(this);
}

protected override void OnNewIntent(Android.Content.Intent intent)
{
    base.OnNewIntent(intent);

    Microsoft.Maui.Essentials.Platform.OnNewIntent(intent);
}
```

# [iOS](#tab/ios)

In the _AppDelegate.cs_ add the following logic to handle actions:

```csharp
public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
    => Microsoft.Maui.Essentials.Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
```

# [Windows](#tab/windows)

In the `App.xaml.cs` file in the `OnLaunched` method add the following logic at the bottom of the method:

```csharp
protected override void OnLaunched(LaunchActivatedEventArgs args)
{
    // ... other code

    Microsoft.Maui.Essentials.Platform.OnLaunched(args);
}
```

-----

<!-- markdownlint-enable MD025 -->

## Create actions

[!INCLUDE [essentials-namespace](../essentials/includes/essentials-namespace.md)]

App Actions can be created at any time, but are often created when an application starts. Call the `SetAsync` method to create the list of actions for your app.

```csharp
try
{
    await AppActions.SetAsync(
        new AppAction("app_info", "App Info", icon: "app_info_action_icon"),
        new AppAction("battery_info", "Battery Info"));
}
catch (FeatureNotSupportedException ex)
{
    Debug.WriteLine("App Actions not supported");
}
```

If App Actions aren't supported on the specific version of the operating system a `FeatureNotSupportedException` will be thrown.

The following properties can be set on an `AppAction`:

- **Id**: A unique identifier used to respond to the action tap.
- **Title**: the visible title to display.
- **Subtitle**: If supported a sub-title to display under the title.
- **Icon**: Must match icons in the corresponding resources directory on each platform.

:::image type="content" source="images/appactions.png" alt-text="App actions on home screen.":::

## Responding to actions

When your application starts register for the `OnAppAction` event. When an app action is selected the event will be sent with information as to which action was selected.

```csharp
public App()
{
    //...
    AppActions.OnAppAction += AppActions_OnAppAction;
}

void AppActions_OnAppAction(object sender, AppActionEventArgs e)
{
    // Don't handle events fired for old application instances
    // and cleanup the old instance's event handler
    if (Application.Current != this && Application.Current is App app)
    {
        AppActions.OnAppAction -= app.AppActions_OnAppAction;
        return;
    }
    MainThread.BeginInvokeOnMainThread(async () =>
    {
        await Shell.Current.GoToAsync($"//{e.AppAction.Id}");
    });
}
```

## GetActions

You can get the current list of App Actions by calling `AppActions.GetAsync()`.

## API

- [AppActions source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/AppActions)
<!-- - [AppActions API documentation](xref:Microsft.Maui.Essentials.AppActions) -->

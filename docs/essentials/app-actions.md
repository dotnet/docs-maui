---
title: "Xamarin.Essentials: App Actions"
description: "The Accelerometer class in Xamarin.Essentials lets you create and respond to app shortcuts from the app icon."
author: jamesmontemagno
ms.author: jamont
ms.date: 01/04/2021
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: App Actions

The **AppActions** class lets you create and respond to app shortcuts from the app icon.

## Get started

[!include[](~/essentials/includes/get-started.md)]

To access the **AppActions** functionality the following platform specific setup is required.

# [Android](#tab/android)

Add the intent filter to your `MainActivity` class:

```csharp
[IntentFilter(
        new[] { Xamarin.Essentials.Platform.Intent.ActionAppAction },
        Categories = new[] { Android.Content.Intent.CategoryDefault })]
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    ...
```

Then add the following logic to handle actions:

```csharp
protected override void OnResume()
{
    base.OnResume();

    Xamarin.Essentials.Platform.OnResume(this);
}

protected override void OnNewIntent(Android.Content.Intent intent)
{
    base.OnNewIntent(intent);

    Xamarin.Essentials.Platform.OnNewIntent(intent);
}
```

# [iOS](#tab/ios)

In the `AppDelegate.cs` add the following logic to handle actions:

```csharp
public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
    => Xamarin.Essentials.Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
```

# [UWP](#tab/uwp)

In the `App.xaml.cs` file in the `OnLaunched` method add the following logic at the bottom of the method:

```csharp
Xamarin.Essentials.Platform.OnLaunched(e);
```

-----

## Create Actions

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```
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

If App Actions are not supported on the specific version of the operating system a `FeatureNotSupportedException` will be thrown. 

The following properties can be set on an `AppAction`:

* Id: A unique identifier used to respond to the action tap.
* Title: the visible title to display.
* Subtitle: If supported a sub-title to display under the title.
* Icon: Must match icons in the corresponding resources directory on each platform.

![App Actions on Homescreen.](images/appactions.png)

## Responding To Actions

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

- [AppActions source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/AppActions)
- [AppActions API documentation](xref:Xamarin.Essentials.AppActions)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/App-Actions-XamarinEssentials-API-of-the-Week/player]
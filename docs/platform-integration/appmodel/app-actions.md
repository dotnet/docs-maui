---
title: "App Actions"
description: "The AppActions class in the Microsoft.Maui.ApplicationModel namespace lets you create and respond to app shortcuts from the app icon."
ms.date: 05/05/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel", "AppDelegate.cs", "AppActions", "Platforms/Android/MainActivity.cs", "Platforms/iOS/AppDelegate.cs", "Platforms/Windows/App.xaml.cs", "Id", "Title", "Subtitle", "Icon"]
---

# App Actions

The `Microsoft.Maui.ApplicationModel.AppActions` class lets you create and respond to app shortcuts from the app icon. App shortcuts are helpful to users because they allow you, as the app developer, to present them with additional ways of starting your app. For example, if you were developing an email and calendar app, you could present two different app actions, one to open the app directly to the current day of the calendar, and another to open to the email inbox folder.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `AppActions` class is available in the `Microsoft.Maui.ApplicationModel` namespace.

## Get started

To access the `AppActions` functionality the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

In the _Platforms/Android/MainActivity.cs_ file, add the following `IntentFilter` attribute to the `MainActivity` class:

:::code language="csharp" source="../snippets/shared_2/Platforms/Android/MainActivity.cs" id="intent_filter_1":::

Then add the following logic to handle actions:

:::code language="csharp" source="../snippets/shared_2/Platforms/Android/MainActivity.cs" id="intent_implementation":::

# [iOS](#tab/ios)

In the _Platforms/iOS/AppDelegate.cs_ file, add the following logic to the `AppDelegate` class to handle actions:

:::code language="csharp" source="../snippets/shared_2/Platforms/iOS/AppDelegate.cs" id="perform_action":::

# [Windows](#tab/windows)

In the _Platforms/Windows/App.xaml.cs_ file, override the `OnLaunched` method in the `App` class. Add the following logic at the bottom of the method:

:::code language="csharp" source="../snippets/shared_2/Platforms/Windows/App.xaml.cs" id="launched":::

-----

<!-- markdownlint-enable MD025 -->

## Create actions

App Actions can be created at any time, but are often created when an application starts. App Actions are accessed through the default implementation of the `IAppActions` interface, available from the `Microsoft.Maui.ApplicationModel.AppActions.Current` property. To create App Actions, call the `SetAsync` method:

:::code language="csharp" source="../snippets/shared_2/MainPage.xaml.cs" id="app_actions":::

If App Actions aren't supported on the specific version of the operating system, a `FeatureNotSupportedException` will be thrown.

The following properties can be set on an `AppAction`:

- **Id**: A unique identifier used to respond to the action tap.
- **Title**: the visible title to display.
- **Subtitle**: If supported a sub-title to display under the title.
- **Icon**: Must match icons in the corresponding resources directory on each platform.

:::image type="content" source="media/app-actions/appactions.png" alt-text="App actions on home screen.":::

## Responding to actions

When your application starts, add handle the `AppActions.Current.AppActionActivated` event. When a user invokes an App Action, the event is raised with information about the action.

:::code language="csharp" source="../snippets/shared_2/App.xaml.cs" id="app_action_handler" highlight="7":::

## GetActions

You can get the current list of App Actions by calling `AppActions.Current.GetAsync()`.

## API

- [AppActions source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/AppActions)
<!-- - [AppActions API documentation](xref:Microsft.Maui.Essentials.AppActions) -->

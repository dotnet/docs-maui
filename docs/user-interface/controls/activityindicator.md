---
title: "ActivityIndicator"
description: "The .NET MAUI ActivityIndicator indicates to users that the app is engaged in a lengthy activity, without giving any indication of progress."
ms.date: 02/08/2022
---

# ActivityIndicator

The .NET Multi-platform App UI (.NET MAUI) `ActivityIndicator` displays an animation to show that the application is engaged in a lengthy activity. Unlike the `ProgressBar`, the `ActivityIndicator` gives no indication of progress.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The appearance of an `ActivityIndicator` is platform-dependent, and the following screenshot shows an `ActivityIndicator` on iOS and Android:

:::image type="content" source="media/activityindicator/activityindicators-default.png" alt-text="Screenshot of ActivityIndicator on iOS and Android.":::

`ActivityIndicator` defines the following properties:

- `Color` is a `Color` value that defines the color of the `ActivityIndicator`.
- `IsRunning` is a `bool` value that indicates whether the `ActivityIndicator` should be visible and animating, or hidden. The default value of this property is `false`, which indicates that the `ActivityIndicator` isn't visible.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

## Create an ActivityIndicator

To indicate a lengthy activity, create an `ActivityIndicator` object and sets its properties to define its appearance.

The following XAML example shows how to display an `ActivityIndicator`:

```xaml
<ActivityIndicator IsRunning="true" />
```

The equivalent C# code is:

```csharp
ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
```

The following XAML example shows how to change the color of an `ActivityIndicator`:

```xaml
<ActivityIndicator IsRunning="true"
                   Color="Orange" />
```

The equivalent C# code is:

```csharp
ActivityIndicator activityIndicator = new ActivityIndicator
{
    IsRunning = true,
    Color = Colors.Orange
};
```

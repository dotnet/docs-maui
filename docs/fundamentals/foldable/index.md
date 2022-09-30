---
title: "Supporting foldable devices with adaptive layouts"
description: "Learn how to use build apps that adapt to foldable devices (for example in the Android ecosystem)."
monikerRange: ">= net-maui-7.0"
ms.date: 11/01/2022
---
# Overview

Foldable devices include the Microsoft Surface Duo and Android devices from other manufacturers. They bridge the gap between phones and larger screens like tablets and desktops because apps might need to adjust to a variety of screen sizes and orientations on the same device, including adapting to a hinge or fold in the screen.

.NET MAUI provides the **Microsoft.Maui.Controls.Foldable** NuGet to help developers build adaptive user interfaces that can present content on a small screen, on a large screen, or in a split view that is aligned to the fold.

Visit the [dual-screen developer docs](/dual-screen/) for more information about building apps that target foldable devices, including design patterns and user experiences.

## Get started


1. Open the **NuGet Package Manager** dialog for your solution.
2. Under the **Browse** tab, search for `Xamarin.Forms.DualScreen`.
3. Install the `Xamarin.Forms.DualScreen` package to your solution.
4. Add the following initialization method call to the project's `MauiApp` class, in the `CreateMauiApp` method:

    ```csharp
    using Microsoft.Maui.Foldable;
    ...
    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        ...
        builder.UseFoldable();
        return builder.Build();
	}
    ```

    The `UseFoldable()` initialization is required for the app to be able to detect changes in the app's state, such as being spanned across a fold.

5. Update the `[Activity()]` attribute on the `MainActivity` class in **Platforms/Android**, so that it includes _all_ these `ConfigurationChanges` options:

    ```@csharp
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode
    ```

    These values are required so that configuration changes and span state can be more reliably reported. By default only two are added to Xamarin.Forms projects, so remember to add the rest for reliable dual-screen support.

## Troubleshooting

If the `TwoPaneView` layout isn't working as expected, double-check the set-up instructions on this page. Omitting or misconfiguring the `UseFoldable()` method or the `ConfigurationChanges` attribute values are common causes of errors.
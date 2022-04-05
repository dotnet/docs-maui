---
title: "NavigationPage bar text color mode on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls whether the status bar text color on a NavigationPage matches the luminosity of the navigation bar."
ms.date: 04/05/2022
---

# NavigationPage bar text color mode on iOS

This .NET Multi-platform App UI (.NET MAUI) platform-specific controls whether the status bar text color on a `NavigationPage` is adjusted to match the luminosity of the navigation bar. It's consumed in XAML by setting the `NavigationPage.StatusBarTextColorMode` attached property to a value of the `StatusBarTextColorMode` enumeration:

```xaml
<FlyoutPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
            x:Class="PlatformSpecifics.iOSStatusBarTextColorModePage">
    <FlyoutPage.Flyout>
        <ContentPage Title="Flyout Page Title" />
    </FlyoutPage.Flyout>
    <FlyoutPage.Detail>
        <NavigationPage BarBackgroundColor="Blue"
                        BarTextColor="White"
                        ios:NavigationPage.StatusBarTextColorMode="MatchNavigationBarTextLuminosity">
            <x:Arguments>
                <ContentPage>
                    <Label Text="Slide the flyout page to see the status bar text color mode change." />
                </ContentPage>
            </x:Arguments>
        </NavigationPage>
    </FlyoutPage.Detail>
</FlyoutPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

IsPresentedChanged += (sender, e) =>
{
    var flyoutPage = sender as FlyoutPage;
    if (flyoutPage.IsPresented)
        ((Microsoft.Maui.Controls.NavigationPage)flyoutPage.Detail)
          .On<iOS>()
          .SetStatusBarTextColorMode(StatusBarTextColorMode.DoNotAdjust);
    else
        ((Microsoft.Maui.Controls.NavigationPage)flyoutPage.Detail)
          .On<iOS>()
          .SetStatusBarTextColorMode(StatusBarTextColorMode.MatchNavigationBarTextLuminosity);
};
```

The `NavigationPage.On<iOS>` method specifies that this platform-specific will only run on iOS. The `NavigationPage.SetStatusBarTextColorMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls whether the status bar text color on the `NavigationPage` is adjusted to match the luminosity of the navigation bar, with the `StatusBarTextColorMode` enumeration providing two possible values:

- `DoNotAdjust` – indicates that the status bar text color should not be adjusted.
- `MatchNavigationBarTextLuminosity` – indicates that the status bar text color should match the luminosity of the navigation bar.

In addition, the `GetStatusBarTextColorMode` method can be used to retrieve the current value of the `StatusBarTextColorMode` enumeration that's applied to the `NavigationPage`.

The result is that the status bar text color on a `NavigationPage` can be adjusted to match the luminosity of the navigation bar. In this example, the status bar text color changes as the user switches between the `Flyout` and `Detail` pages of the `FlyoutPage`.

---
title: "ScrollView content touches on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls whether a ScrollView handles a touch gesture or passes it to its content."
ms.date: 04/05/2022
---

# ScrollView content touches on iOS

An implicit timer is triggered when a touch gesture begins in a <xref:Microsoft.Maui.Controls.ScrollView> on iOS and the <xref:Microsoft.Maui.Controls.ScrollView> decides, based on the user action within the timer span, whether it should handle the gesture or pass it to its content. By default, the iOS <xref:Microsoft.Maui.Controls.ScrollView> delays content touches, but this can cause problems in some circumstances with the <xref:Microsoft.Maui.Controls.ScrollView> content not winning the gesture when it should. Therefore, this .NET Multi-platform App UI (.NET MAUI) platform-specific controls whether a <xref:Microsoft.Maui.Controls.ScrollView> handles a touch gesture or passes it to its content. It's consumed in XAML by setting the `ScrollView.ShouldDelayContentTouches` attached property to a `boolean` value:

```xaml
<FlyoutPage ...
            xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <FlyoutPage.Flyout>
        <ContentPage Title="Menu"
                     BackgroundColor="Blue" />
    </FlyoutPage.Flyout>
    <FlyoutPage.Detail>
        <ContentPage>
            <ScrollView x:Name="scrollView"
                        ios:ScrollView.ShouldDelayContentTouches="false">
                <StackLayout Margin="0,20">
                    <Slider />
                    <Button Text="Toggle ScrollView DelayContentTouches"
                            Clicked="OnButtonClicked" />
                </StackLayout>
            </ScrollView>
        </ContentPage>
    </FlyoutPage.Detail>
</FlyoutPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

scrollView.On<iOS>().SetShouldDelayContentTouches(false);
```

The `ScrollView.On<iOS>` method specifies that this platform-specific will only run on iOS. The `ScrollView.SetShouldDelayContentTouches` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether a <xref:Microsoft.Maui.Controls.ScrollView> handles a touch gesture or passes it to its content. In addition, the `SetShouldDelayContentTouches` method can be used to toggle delaying content touches by calling the `ShouldDelayContentTouches` method to return whether content touches are delayed:

```csharp
scrollView.On<iOS>().SetShouldDelayContentTouches(!scrollView.On<iOS>().ShouldDelayContentTouches());
```

The result is that a <xref:Microsoft.Maui.Controls.ScrollView> can disable delaying receiving content touches, so that in this scenario the <xref:Microsoft.Maui.Controls.Slider> receives the gesture rather than the `Detail` page of the <xref:Microsoft.Maui.Controls.FlyoutPage>:

:::image type="content" source="media/scrollview-content-touches/scrollview-delay-content-touches.png" alt-text="ScrollView Delay Content Touches Platform-Specific.":::

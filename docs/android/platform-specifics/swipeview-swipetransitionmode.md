---
title: "SwipeView swipe transition mode on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that controls the transition that's used when opening a SwipeView."
ms.date: 04/05/2022
---

# SwipeView swipe transition mode on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific controls the transition that's used when opening a <xref:Microsoft.Maui.Controls.SwipeView>. It's consumed in XAML by setting the `SwipeView.SwipeTransitionMode` bindable property to a value of the `SwipeTransitionMode` enumeration:

```xaml
<ContentPage ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls" >
    <StackLayout>
        <SwipeView android:SwipeView.SwipeTransitionMode="Drag">
            <SwipeView.LeftItems>
                <SwipeItems>
                    <SwipeItem Text="Delete"
                               IconImageSource="delete.png"
                               BackgroundColor="LightPink"
                               Invoked="OnDeleteSwipeItemInvoked" />
                </SwipeItems>
            </SwipeView.LeftItems>
            <!-- Content -->
        </SwipeView>
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

SwipeView swipeView = new Microsoft.Maui.Controls.SwipeView();
swipeView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetSwipeTransitionMode(SwipeTransitionMode.Drag);
// ...
```

The `SwipeView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `SwipeView.SetSwipeTransitionMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to control the transition that's used when opening a <xref:Microsoft.Maui.Controls.SwipeView>. The `SwipeTransitionMode` enumeration provides two possible values:

- `Reveal` indicates that the swipe items will be revealed as the <xref:Microsoft.Maui.Controls.SwipeView> content is swiped, and is the default value of the `SwipeView.SwipeTransitionMode` property.
- `Drag` indicates that the swipe items will be dragged into view as the <xref:Microsoft.Maui.Controls.SwipeView> content is swiped.

In addition, the `SwipeView.GetSwipeTransitionMode` method can be used to return the `SwipeTransitionMode` that's applied to the <xref:Microsoft.Maui.Controls.SwipeView>.

The result is that a specified `SwipeTransitionMode` value is applied to the <xref:Microsoft.Maui.Controls.SwipeView>, which controls the transition that's used when opening the <xref:Microsoft.Maui.Controls.SwipeView>:

:::image type="content" source="media/swipeview-swipetransitionmode/swipetransitionmode.png" alt-text="Screenshot of SwipeView SwipeTransitionModes, on Android.":::

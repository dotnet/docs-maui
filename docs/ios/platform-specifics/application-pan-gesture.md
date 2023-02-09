---
title: "Simultaneous pan gesture recognition on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that enables simultaneous pan gesture recognition to be used in an app."
ms.date: 04/05/2022
---

# Simultaneous pan gesture recognition on iOS

When a <xref:Microsoft.Maui.Controls.PanGestureRecognizer> is attached to a view inside a scrolling view, all of the pan gestures are captured by the <xref:Microsoft.Maui.Controls.PanGestureRecognizer> and aren't passed to the scrolling view. Therefore, the scrolling view will no longer scroll.

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific enables a <xref:Microsoft.Maui.Controls.PanGestureRecognizer> in a scrolling view to capture and share the pan gesture with the scrolling view. It's consumed in XAML by setting the `Application.PanGestureRecognizerShouldRecognizeSimultaneously` attached property to `true`:

```xaml
<Application ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Application.PanGestureRecognizerShouldRecognizeSimultaneously="true">
    ...
</Application>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

Application.Current.On<iOS>().SetPanGestureRecognizerShouldRecognizeSimultaneously(true);
```

The `Application.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Application.SetPanGestureRecognizerShouldRecognizeSimultaneously` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether a pan gesture recognizer in a scrolling view will capture the pan gesture, or capture and share the pan gesture with the scrolling view. In addition, the `Application.GetPanGestureRecognizerShouldRecognizeSimultaneously` method can be used to return whether the pan gesture is shared with the scrolling view that contains the <xref:Microsoft.Maui.Controls.PanGestureRecognizer>.

Therefore, with this platform-specific enabled, when a <xref:Microsoft.Maui.Controls.ListView> contains a <xref:Microsoft.Maui.Controls.PanGestureRecognizer>, both the <xref:Microsoft.Maui.Controls.ListView> and the <xref:Microsoft.Maui.Controls.PanGestureRecognizer> will receive the pan gesture and process it. However, with this platform-specific disabled, when a <xref:Microsoft.Maui.Controls.ListView> contains a <xref:Microsoft.Maui.Controls.PanGestureRecognizer>, the <xref:Microsoft.Maui.Controls.PanGestureRecognizer> will capture the pan gesture and process it, and the <xref:Microsoft.Maui.Controls.ListView> won't receive the pan gesture.

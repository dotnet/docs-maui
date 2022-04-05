---
title: "VisualElement first responder on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that enables a VisualElement object to become the first responder to touch events."
ms.date: 04/05/2022
---

# VisualElement first responder on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific enables a `VisualElement` object to become the first responder to touch events, rather than the page containing the element. It's consumed in XAML by setting the `VisualElement.CanBecomeFirstResponder` bindable property to `true`:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <Entry Placeholder="Enter text" />
        <Button ios:VisualElement.CanBecomeFirstResponder="True"
                Text="OK" />
    </StackLayout>
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

Entry entry = new Entry { Placeholder = "Enter text" };
Button button = new Button { Text = "OK" };
button.On<iOS>().SetCanBecomeFirstResponder(true);
```

The `VisualElement.On<iOS>` method specifies that this platform-specific will only run on iOS. The `VisualElement.SetCanBecomeFirstResponder` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to set the `VisualElement` to become the first responder for touch events. In addition, the `VisualElement.CanBecomeFirstResponder` method can be used to return whether the `VisualElement` is the first responder to touch events.

The result is that a `VisualElement` can become the first responder for touch events, rather than the page containing the element. This enables scenarios such as chat apps not dismissing a keyboard when a `Button` is tapped.

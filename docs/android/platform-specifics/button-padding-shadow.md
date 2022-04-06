---
title: "Button padding and shadows on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that uses the default padding and shadow values of Android buttons."
ms.date: 04/05/2022
---

# Button padding and shadows on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific controls whether .NET MAUI buttons use the default padding and shadow values of Android buttons. It's consumed in XAML by setting the `Button.UseDefaultPadding` and `Button.UseDefaultShadow` attached properties to `boolean` values:

```xaml
<ContentPage ...
            xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        ...
        <Button ...
                android:Button.UseDefaultPadding="true"
                android:Button.UseDefaultShadow="true" />         
    </StackLayout>
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

button.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetUseDefaultPadding(true).SetUseDefaultShadow(true);
```

The `Button.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `Button.SetUseDefaultPadding` and `Button.SetUseDefaultShadow` methods, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, are used to control whether .NET MAUI buttons use the default padding and shadow values of Android buttons. In addition, the `Button.UseDefaultPadding` and `Button.UseDefaultShadow` methods can be used to return whether a button uses the default padding value and default shadow value, respectively.

The result is that .NET MAUI buttons can use the default padding and shadow values of Android buttons:

:::image type="content" source="media/button-padding-shadow/button-padding-and-shadow.png" alt-text="Default Padding and Shadow Values on Android Buttons.":::

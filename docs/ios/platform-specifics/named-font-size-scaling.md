---
title: "Accessibility scaling for named font sizes on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that disables accessibility scaling for named font sizes."
ms.date: 04/05/2022
---

# Accessibility scaling for named font sizes on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific disables accessibility scaling for named font sizes. It's consumed in XAML by setting the `Application.EnableAccessibilityScalingForNamedFontSizes` bindable property to `false`:

```xaml
<Application ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Application.EnableAccessibilityScalingForNamedFontSizes="false">
    ...
</Application>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

Application.Current.On<iOS>().SetEnableAccessibilityScalingForNamedFontSizes(false);
```

The `Application.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Application.SetEnableAccessibilityScalingForNamedFontSizes` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to disable named font sizes being scaled by the iOS accessibility settings. In addition, the `Application.GetEnableAccessibilityScalingForNamedFontSizes` method can be used to return whether named font sizes are scaled by iOS accessibility settings.

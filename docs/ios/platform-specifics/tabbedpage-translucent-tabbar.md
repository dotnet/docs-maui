---
title: "TabbedPage translucent tab bar on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that sets the translucency mode of the tab bar on a TabbedPage."
ms.date: 04/05/2022
---

# TabbedPage translucent tab bar on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to set the translucency mode of the tab bar on a <xref:Microsoft.Maui.Controls.TabbedPage>. It's consumed in XAML by setting the `TabbedPage.TranslucencyMode` bindable property to a `TranslucencyMode` enumeration value:

```xaml
<TabbedPage ...
            xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
            ios:TabbedPage.TranslucencyMode="Opaque">
    ...
</TabbedPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

On<iOS>().SetTranslucencyMode(TranslucencyMode.Opaque);
```

The `TabbedPage.On<iOS>` method specifies that this platform-specific will only run on iOS. The `TabbedPage.SetTranslucencyMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to set the translucency mode of the tab bar on a <xref:Microsoft.Maui.Controls.TabbedPage> by specifying one of the following `TranslucencyMode` enumeration values:

- `Default`, which sets the tab bar to its default translucency mode. This is the default value of the `TabbedPage.TranslucencyMode` property.
- `Translucent`, which sets the tab bar to be translucent.
- `Opaque`, which sets the tab bar to be opaque.

In addition, the `GetTranslucencyMode` method can be used to retrieve the current value of the `TranslucencyMode` enumeration that's applied to the <xref:Microsoft.Maui.Controls.TabbedPage>.

The result is that the translucency mode of the tab bar on a <xref:Microsoft.Maui.Controls.TabbedPage> can be set:

:::image type="content" source="media/tabbedpage-translucent-tabbar/translucencymodes.png" alt-text="Screenshot of translucent and opaque tab bars on iOS.":::

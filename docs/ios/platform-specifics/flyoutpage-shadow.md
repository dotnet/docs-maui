---
title: "FlyoutPage shadow on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls whether the detail page of a FlyoutPage has shadow applied to it, when revealing the flyout page."
ms.date: 04/05/2022
---

# FlyoutPage shadow on iOS

This .NET Multi-platform App UI (.NET MAUI) platform-specific controls whether the detail page of a <xref:Microsoft.Maui.Controls.FlyoutPage> has shadow applied to it, when revealing the flyout page. It's consumed in XAML by setting the `FlyoutPage.ApplyShadow` bindable property to `true`:

```xaml
<FlyoutPage ...
            xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
            ios:FlyoutPage.ApplyShadow="true">
    ...
</FlyoutPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

public class iOSFlyoutPageCode : FlyoutPage
{
    public iOSFlyoutPageCode()
    {
        On<iOS>().SetApplyShadow(true);
    }
}
```

The `FlyoutPage.On<iOS>` method specifies that this platform-specific will only run on iOS. The `FlyoutPage.SetApplyShadow` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether the detail page of a <xref:Microsoft.Maui.Controls.FlyoutPage> has shadow applied to it, when revealing the flyout page. In addition, the `GetApplyShadow` method can be used to determine whether shadow is applied to the detail page of a <xref:Microsoft.Maui.Controls.FlyoutPage>.

The result is that the detail page of a <xref:Microsoft.Maui.Controls.FlyoutPage> can have shadow applied to it, when revealing the flyout page:

:::image type="content" source="media/flyoutpage-shadow/shadow.png" alt-text="Screenshot of a FlyoutPage with and without shadow.":::

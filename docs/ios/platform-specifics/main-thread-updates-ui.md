---
title: "Main thread control updates on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that enables control layout and rendering updates to be performed on the main thread."
ms.date: 04/05/2022
---

# Main thread control updates on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific enables control layout and rendering updates to be performed on the main thread, instead of being performed on a background thread. It should be rarely needed, but in some cases may prevent crashes. Its consumed in XAML by setting the `Application.HandleControlUpdatesOnMainThread` bindable property to `true`:

```xaml
<Application ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Application.HandleControlUpdatesOnMainThread="true">
    ...
</Application>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

Application.Current.On<iOS>().SetHandleControlUpdatesOnMainThread(true);
```

The `Application.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Application.SetHandleControlUpdatesOnMainThread` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether control layout and rendering updates are performed on the main thread, instead of being performed on a background thread. In addition, the `Application.GetHandleControlUpdatesOnMainThread` method can be used to return whether control layout and rendering updates are being performed on the main thread.

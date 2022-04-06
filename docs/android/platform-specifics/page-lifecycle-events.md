---
title: "Page lifecycle events on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that disables the Disappearing and Appearing page events on application pause and resume, respectively."
ms.date: 04/05/2022
---

# Page lifecycle events on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific is used to disable the `Disappearing` and `Appearing` page events on application pause and resume respectively, for applications that use AppCompat. In addition, it includes the ability to control whether the soft keyboard is displayed on resume, if it was displayed on pause, provided that the operating mode of the soft keyboard is set to `WindowSoftInputModeAdjust.Resize`.

> [!NOTE]
> Note that these events are enabled by default to preserve existing behavior for applications that rely on the events. Disabling these events makes the AppCompat event cycle match the pre-AppCompat event cycle.

This platform-specific can be consumed in XAML by setting the `Application.SendDisappearingEventOnPause`, `Application.SendAppearingEventOnResume`, and `Application.ShouldPreserveKeyboardOnResume` attached properties to `boolean` values:

```xaml
<Application ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls"
             xmlns:androidAppCompat="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Microsoft.Maui.Controls"
             android:Application.WindowSoftInputModeAdjust="Resize"
             androidAppCompat:Application.SendDisappearingEventOnPause="false"
             androidAppCompat:Application.SendAppearingEventOnResume="false"
             androidAppCompat:Application.ShouldPreserveKeyboardOnResume="true">
  ...
</Application>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp

using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;
...

Application.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
     .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize)
     .SendDisappearingEventOnPause(false)
     .SendAppearingEventOnResume(false)
     .ShouldPreserveKeyboardOnResume(true);
```

The `Application.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `Application.SendDisappearingEventOnPause` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat` namespace, is used to enable or disable firing the `Disappearing` page event, when the application enters the background. The `Application.SendAppearingEventOnResume` method is used to enable or disable firing the `Appearing` page event, when the application resumes from the background. The `Application.ShouldPreserveKeyboardOnResume` method is used control whether the soft keyboard is displayed on resume, if it was displayed on pause, provided that the operating mode of the soft keyboard is set to `WindowSoftInputModeAdjust.Resize`.

The result is that the `Disappearing` and `Appearing` page events won't be fired on application pause and resume respectively, and that if the soft keyboard was displayed when the application was paused, it will also be displayed when the application resumes:

:::image type="content" source="media/page-lifecycle-events/keyboard-on-resume.png" alt-text="Lifecycle Events Platform-Specific.":::

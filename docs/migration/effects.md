---
title: "Reuse effects in .NET MAUI"
description: "Learn how to adapt Xamarin.Forms effects to work in a .NET MAUI app."
ms.date: 02/15/2023
---

# Reuse effects in .NET MAUI

While there are many benefits to using .NET Multi-platform App UI (.NET MAUI) handlers to customize controls, it's still possible to use Xamarin.Forms effects in .NET MAUI apps. For more information about effects, see [Xamarin.Forms effects](/xamarin/xamarin-forms/app-fundamentals/effects/).

The process for migrating a Xamarin.Forms effect to .NET MAUI is to:

1. Remove the effect attributes from your effect classes. For more information, see [Remove effect attributes](#remove-effect-attributes).
1. Remove the effect `using` directives. For more information, see [Remove using directives](#remove-using-directives).
1. Add the effect code into the appropriate location in your .NET MAUI app project. For more information, see [Add the effect code](#add-the-effect-code).
1. Register the effect. For more information, see [Register the effect](#register-the-effect).
1. Consume your .NET MAUI effect. For more information, see [Consume the effect](#consume-the-effect).

## Remove effect attributes

Any <xref:Xamarin.Forms.ResolutionGroupNameAttribute> and <xref:Xamarin.Forms.ExportEffectAttribute> attributes should be removed from your effect classes.

## Remove using directives

Any references to the `Xamarin.Forms` and `Xamarin.Forms.Platform.*` namespaces should be removed from your effect classes.

## Add the effect code

If you're using a .NET MAUI multi-targeted project, your effect code should be combined into a single file and placed outside the *Platforms* folder. This requires you to combine your <xref:Microsoft.Maui.Controls.RoutingEffect> implementation and <xref:Microsoft.Maui.Controls.Platform.PlatformEffect> implementations into a single file, using conditional compilation around platform code. However, if your solution has separate projects per-platform, then you should move the platform-specific effect files into the corresponding projects.

In .NET MAUI, the <xref:Microsoft.Maui.Controls.RoutingEffect> class is in the `Microsoft.Maui.Controls` namespace. This namespace is one of .NET MAUI's implicit `global using` directives, and so you don't need to add a `using` directive for it. However, the <xref:Microsoft.Maui.Controls.Platform.PlatformEffect> class is in the `Microsoft.Maui.Controls.Platform` namespace, for which you must add a `using` directive.

The following code example shows a `FocusRoutingEffect` class and its platform implementations combined into a single file:

```csharp
using Microsoft.Maui.Controls.Platform;

namespace MyMauiApp.Effects;

internal class FocusRoutingEffect : RoutingEffect
{
}

#if ANDROID
internal class FocusPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        // Customize the control here
    }

    protected override void OnDetached()
    {
        // Cleanup the control customization here
    }
}
#elif IOS
internal class FocusPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        // Customize the control here
    }

    protected override void OnDetached()
    {
        // Cleanup the control customization here
    }
}
#elif WINDOWS
internal class FocusPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        // Customize the control here
    }

    protected override void OnDetached()
    {
        // Cleanup the control customization here
    }
}
#endif
```

## Register the effect

In your .NET MAUI app project, open *MauiProgram.cs* and call the <xref:Microsoft.Maui.Controls.Hosting.AppHostBuilderExtensions.ConfigureEffects%2A> method on the <xref:Microsoft.Maui.Hosting.MauiAppBuilder> object in the `CreateMauiApp` method:

```csharp
public static MauiApp CreateMauiApp()
{
  var builder = MauiApp.CreateBuilder();
  builder
    .UseMauiApp<App>()
    .ConfigureEffects(effects =>
    {
      effects.Add<FocusRoutingEffect, FocusPlatformEffect>();
    });

  return builder.Build();
}
```

The effect is registered with the <xref:Microsoft.Maui.Controls.Hosting.AppHostBuilderExtensions.ConfigureEffects%2A> method, whose `configureDelegate` registers the <xref:Microsoft.Maui.Controls.Platform.PlatformEffect> implementation against its <xref:Microsoft.Maui.Controls.RoutingEffect> implementation.

## Consume the effect

The effect can be consumed in a .NET MAUI app by adding it to the <xref:Microsoft.Maui.Controls.Element.Effects> collection of a control:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyMauiApp.Effects"
             x:Class="MyMauiApp.MainPage">
    <VerticalStackLayout>
        <Entry Text="Enter your text">
            <Entry.Effects>
                <local:FocusRoutingEffect />
            </Entry.Effects>
        </Entry>
    </VerticalStackLayout>
</ContentPage>
```

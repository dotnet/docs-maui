---
title: "Basic animation"
description: "The .NET MAUI ViewExtensions class, in the Microsoft.Maui.Controls namespace, provides extension methods that can be used to create and cancel basic animations."
ms.date: 06/05/2026
---

# Basic animation

::: moniker range="<=net-maui-9.0"

[!INCLUDE [Basic animation](../includes/basic-animation-dotnet9.md)]

::: moniker-end

::: moniker range=">=net-maui-10.0"

[!INCLUDE [Basic animation](../includes/basic-animation-dotnet10.md)]

::: moniker-end

## Canceling animations

The <xref:Microsoft.Maui.Controls.ViewExtensions.CancelAnimations%2A> extension method is used to cancel any animations, such as rotation, scaling, translation, and fading, that are running on a specific <xref:Microsoft.Maui.Controls.VisualElement>.

```csharp
image.CancelAnimations();
```

In this example, all animations that are running on the <xref:Microsoft.Maui.Controls.Image> instance are immediately canceled.

:::moniker range=">=net-maui-11.0"

### Cancel an animation with a CancellationToken

Starting in .NET MAUI 11, the `ViewExtensions` animation methods (`FadeToAsync`, `RotateToAsync`, `ScaleToAsync`, `TranslateToAsync`, and the relative variants) accept an optional <xref:System.Threading.CancellationToken>. Passing a token lets you cancel a specific awaited animation without having to call `CancelAnimations`, which cancels every animation on the element. This is useful when an element is running more than one animation, or when an animation should stop in response to a separate user action.

The cancellation-aware overloads return `Task<bool>`. The result is `true` when the animation was canceled and `false` when it ran to completion. The animation methods don't throw on cancellation, so there's no exception to catch.

The following example fades a label out, and cancels the fade if the user presses a button before it completes:

```csharp
CancellationTokenSource cts = new();

async void OnFadeClicked(object sender, EventArgs e)
{
    bool canceled = await label.FadeToAsync(0, 2000, Easing.SinIn, cts.Token);

    if (canceled)
    {
        // The fade was canceled before it completed; the label keeps
        // whatever opacity it had reached.
    }
}

void OnCancelClicked(object sender, EventArgs e)
{
    cts.Cancel();
}
```

When the token is canceled, the underlying animation is aborted and the element keeps whatever intermediate value it had reached. A typical pattern is to read the `bool` return value and either reset the element to a known state or animate it from where it stopped.

> [!NOTE]
> The non-`Async` versions of these methods (`FadeTo`, `RotateTo`, and so on) are now marked `[Obsolete]` in favor of the `Async`-suffixed equivalents. Migrate to the new methods to take advantage of `CancellationToken` support.

::: moniker-end

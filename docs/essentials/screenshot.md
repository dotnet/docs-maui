---
title: "Xamarin.Essentials: Screenshot"
description: "This document describes the Screenshot class in Xamarin.Essentials, which lets you take a capture of the current displayed screen of the app."
author: jamesmontemagno
ms.author: jamont
ms.date: 04/14/2021
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Screenshot

The **Screenshot** class lets you take a capture of the current displayed screen of the app.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Screenshot

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

Then call `CaptureAsync` to take a screenshot of the current screen of the running application. This will return back a `ScreenshotResult` that can be used to get the `Width`, `Height`, and a `Stream` of the screenshot taken.


```csharp
async Task CaptureScreenshot()
{
    var screenshot = await Screenshot.CaptureAsync();
    var stream = await screenshot.OpenReadAsync();

    Image = ImageSource.FromStream(() => stream);
}
```

## Limitations

Not all views support being captured at a screen level such as an OpenGL view.

## API

- [Screenshot source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Screenshot)
- [Screenshot API documentation](xref:Xamarin.Essentials.Screenshot)

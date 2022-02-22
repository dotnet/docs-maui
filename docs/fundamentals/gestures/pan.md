---
title: "Recognize a pan gesture"
description: "This article explains how to use a .NET MAUI pan gesture to horizontally and vertically pan an image, so that all of the image content can be viewed when it's being displayed in a viewport smaller than the image dimensions."
ms.date: 02/22/2022
---

# Recognize a pan gesture

A .NET Multi-platform App UI (.NET MAUI) pan gesture is used for detecting the movement of fingers around the screen and applying that movement to content, and is implemented with the `PanGestureRecognizer` class. A common scenario for the pan gesture is to horizontally and vertically pan an image, so that all of the image content can be viewed when it's being displayed in a viewport smaller than the image dimensions. This is accomplished by moving the image within the viewport.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In .NET MAUI, pan gesture recognition is provided by the `PanGestureRecognizer` class. This class defines the `TouchPoints` property, of type `int`, which represents the number of touch points in the gesture. The default value of this property is 1. This property is backed by a `BindableProperty` object, which means that it can be the target of data bindings, and styled.

The `PanGestureRecognizer` class also defines a `PanUpdated` event that's raised when XXXX. The `PanUpdatedEventArgs` object that accompanies this eevent defines the following properties:

- `GestureId`, of type `int`, which represents the id of the gesture that raised the event.
- `StatusType`, of type `GestureStatus`, which indicates if the event has fired for a newly started gesture, a running gesture, a completed gesture, or a canceled gesture.
- `TotalX`, of type `double`, which indicates the total change in the X direction since the beginning of the gesture.
- `TotalY`, of type `double`, which indicates the total change in the Y direction since the beginning of the gesture.

## Create a PanGestureRecognizer

To make a `View` recognize a pan gesture, create a `PanGestureRecognizer` object, handle the `PanUpdated` event, and add the new gesture recognizer to the `GestureRecognizers` collection on the view. The following code example shows a `PanGestureRecognizer` attached to an `Image`:

```xaml
<Image Source="monkey.jpg">
  <Image.GestureRecognizers>
    <PanGestureRecognizer PanUpdated="OnPanUpdated" />
  </Image.GestureRecognizers>
</Image>
```

The code for the `OnPanUpdated` event handler is then added to the code-behind file:

```csharp
void OnPanUpdated (object sender, PanUpdatedEventArgs e)
{
  // Handle the pan
}
```

The equivalent C# code is:

```csharp
PanGestureRecognizer panGesture = new PanGestureRecognizer();
panGesture.PanUpdated += (s, e) =>
{
    // Handle the pan
};
image.GestureRecognizers.Add(panGesture);
```

## Create a pan container

Freeform panning is typically suited to navigating within images and maps. The `PanContainer` class, which is shown in the following example, is a generalized helper class that performs freeform panning:

```csharp
public class PanContainer : ContentView
{
    double x, y;

    public PanContainer()
    {
        // Set PanGestureRecognizer.TouchPoints to control the
        // number of touch points needed to pan
        PanGestureRecognizer panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);
    }

    void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                Content.TranslationX = Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(Content.Width - DeviceDisplay.MainDisplayInfo.Width));
                Content.TranslationY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - DeviceDisplay.MainDisplayInfo.Height));
                break;

            case GestureStatus.Completed:
                // Store the translation applied during the pan
                x = Content.TranslationX;
                y = Content.TranslationY;
                break;
        }
    }
}
```

In this example, the `OnPanUpdated` method updates the viewable content of the wrapped view, based on the user's pan gesture. This is achieved by using the values of the `TotalX` and `TotalY` properties of the `PanUpdatedEventArgs` instance to calculate the direction and distance of the pan. The `DeviceDisplay.MainDisplayInfo.Width` and `DeviceDisplay.MainDisplayInfo.Height` properties provide the screen width and screen height values of the device. The wrapped user element is then panned by setting its `TranslationX` and `TranslationY` properties to the calculated values. When panning content in an element that does not occupy the full screen, the height and width of the viewport can be obtained from the element's `Height` and `Width` properties.

The `PanContainer` class can be wrapped around a `View` so that a recognized pan gesture will pan the wrapped view. The following XAML example shows the `PanContainer` wrapping an `Image`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:PanGesture"
             x:Class="PanGesture.MainPage">
    <AbsoluteLayout>
        <local:PanContainer>
            <Image Source="monkey.jpg" WidthRequest="1024" HeightRequest="768" />
        </local:PanContainer>
    </AbsoluteLayout>
</ContentPage>
```

In this example, when the `Image` element receives a pan gesture, the displayed image will be panned.

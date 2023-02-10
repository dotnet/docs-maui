---
title: "Recognize a pinch gesture"
description: "This article explains how to use the pinch gesture to perform interactive zoom of an image in .NET MAUI, at the pinch location."
ms.date: 02/21/2022
---

# Recognize a pinch gesture

A .NET Multi-platform App UI (.NET MAUI) pinch gesture recognizer is used for performing interactive zoom. A common scenario for the pinch gesture is to perform interactive zoom of an image at the pinch location. This is accomplished by scaling the content of the viewport.

In .NET MAUI, pinch gesture recognition is provided by the <xref:Microsoft.Maui.Controls.PinchGestureRecognizer> class, which defines a <xref:Microsoft.Maui.Controls.PinchGestureRecognizer.PinchUpdated> event that's raised when the detected pinch gesture changes. The <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.PinchGestureRecognizer.PinchUpdated> event defines the following properties:

- <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs.Scale>, of type `double`, which indicates the relative size of the pinch gesture since the last update was received.
- <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs.ScaleOrigin>, of type `Point`, which indicates the updated origin of the pinch's gesture.
- <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs.Status>, of type <xref:Microsoft.Maui.GestureStatus>, which indicates if the event has been raised for a newly started gesture, a running gesture, a completed gesture, or a canceled gesture.

## Create a PinchGestureRecognizer

To make a <xref:Microsoft.Maui.Controls.View> recognize a pinch gesture, create a <xref:Microsoft.Maui.Controls.PinchGestureRecognizer> object, handle the <xref:Microsoft.Maui.Controls.PinchGestureRecognizer.PinchUpdated> event, and add the new gesture recognizer to the `GestureRecognizers` collection on the view. The following code example shows a <xref:Microsoft.Maui.Controls.PinchGestureRecognizer> attached to an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image Source="waterfront.jpg">
    <Image.GestureRecognizers>
        <PinchGestureRecognizer PinchUpdated="OnPinchUpdated" />
    </Image.GestureRecognizers>
</Image>
```

The code for the `OnPinchUpdated` event handler should be added to the code-behind file:

```csharp
void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
{
    // Handle the pinch
}
```

The equivalent C# code is:

```csharp
PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
pinchGesture.PinchUpdated += (s, e) =>
{
    // Handle the pinch
};
image.GestureRecognizers.Add(pinchGesture);
```

## Create a pinch container

The `PinchToZoomContainer` class, which is shown in the following example, is a generalized helper class that can be used to interactively zoom a <xref:Microsoft.Maui.Controls.View>:

```csharp
public class PinchToZoomContainer : ContentView
{
    double currentScale = 1;
    double startScale = 1;
    double xOffset = 0;
    double yOffset = 0;

    public PinchToZoomContainer()
    {
        PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;
        GestureRecognizers.Add(pinchGesture);
    }

    void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the current scale factor applied to the wrapped user interface element,
            // and zero the components for the center point of the translate transform.
            startScale = Content.Scale;
            Content.AnchorX = 0;
            Content.AnchorY = 0;
        }
        if (e.Status == GestureStatus.Running)
        {
            // Calculate the scale factor to be applied.
            currentScale += (e.Scale - 1) * startScale;
            currentScale = Math.Max(1, currentScale);

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the X pixel coordinate.
            double renderedX = Content.X + xOffset;
            double deltaX = renderedX / Width;
            double deltaWidth = Width / (Content.Width * startScale);
            double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the Y pixel coordinate.
            double renderedY = Content.Y + yOffset;
            double deltaY = renderedY / Height;
            double deltaHeight = Height / (Content.Height * startScale);
            double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

            // Calculate the transformed element pixel coordinates.
            double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
            double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

            // Apply translation based on the change in origin.
            Content.TranslationX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
            Content.TranslationY = Math.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

            // Apply scale factor
            Content.Scale = currentScale;
        }
        if (e.Status == GestureStatus.Completed)
        {
            // Store the translation delta's of the wrapped user interface element.
            xOffset = Content.TranslationX;
            yOffset = Content.TranslationY;
        }
    }
}
```

In this example, the `OnPinchUpdated` method updates the zoom level of the wrapped view, based on the user's pinch gesture. This is achieved by using the values of the <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs.Scale>, <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs.ScaleOrigin> and <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs.Status> properties of the <xref:Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs> object to calculate the scale factor to be applied at the origin of the pinch gesture. The wrapped view is then zoomed at the origin of the pinch gesture by setting its `TranslationX`, `TranslationY`, and `Scale` properties to the calculated values.

The `PinchToZoomContainer` class can be wrapped around a <xref:Microsoft.Maui.Controls.View> so that a recognized pinch gesture will zoom the wrapped view. The following XAML example shows the `PinchToZoomContainer` wrapping an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:PinchGesture;assembly=PinchGesture"
             x:Class="PinchGesture.HomePage">
    <Grid>
        <local:PinchToZoomContainer>
            <Image Source="waterfront.jpg" />
        </local:PinchToZoomContainer>
    </Grid>
</ContentPage>
```

In this example, when the <xref:Microsoft.Maui.Controls.Image> receives a pinch gesture, the displayed image will be zoomed-in or out.

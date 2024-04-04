---
title: "Recognize a pointer gesture"
description: "Learn how to use the PointerGestureRecognizer class, to detect when the pointer enters, exits, and moves within a view on iPadOS, Mac Catalyst, and Windows."
ms.date: 10/16/2023
---

# Recognize a pointer gesture

A .NET Multi-platform App UI (.NET MAUI) pointer gesture recognizer detects when the pointer enters, exits, and moves within a view and is implemented with the <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> class. This class defines the following properties:

::: moniker range="=net-maui-7.0"

- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEnteredCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer enters the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEnteredCommandParameter>, of type `object`, which is the parameter that's passed to <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEnteredCommand>.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExitedCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer that's in the view's bounding area leaves that bounding area.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExitedCommandParameter>, of type `object`, which is the parameter that's passed to <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExitedCommand>.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMovedCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer moves while remaining within the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMovedCommandParameter>, of type `object`, which is the parameter that's passed to <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMovedCommand>.

::: moniker-end

::: moniker range=">=net-maui-8.0"

- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEnteredCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer enters the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEnteredCommandParameter>, of type `object`, which is the parameter that's passed to <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEnteredCommand>.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExitedCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer that's in the view's bounding area leaves that bounding area.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExitedCommandParameter>, of type `object`, which is the parameter that's passed to <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExitedCommand>.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMovedCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer moves while remaining within the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMovedCommandParameter>, of type `object`, which is the parameter that's passed to <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMovedCommand>.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressedCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer initiates a press within the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressedCommandParameter>, of type `object`, which is the parameter that's passed to the <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressedCommand>.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleasedCommand>, of type <xref:System.Windows.Input.ICommand>, which is the command to invoke when the pointer that has previously initiated a press is released, while within the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleasedCommandParameter>, of type `object`, which is the parameter that's passed to the <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleasedCommand>.

::: moniker-end

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> class also defines the following events:

::: moniker range="=net-maui-7.0"

- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEntered>, that's raised when the pointer enters the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExited>, that's raised when the pointer that's in the view's bounding area leaves that bounding area.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMoved>, that's raised when the pointer moves while remaining within the bounding area of the view.

A <xref:Microsoft.Maui.Controls.PointerEventArgs> object accompanies the events, and defines a <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method that returns a `Point?` object that represents the position of the pointer when the gesture was detected. For more information about the <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method, see [Get the gesture position](#get-the-gesture-position).

::: moniker-end

::: moniker range=">=net-maui-8.0"

- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerEntered>, that's raised when the pointer enters the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerExited>, that's raised when the pointer that's in the view's bounding area leaves that bounding area.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerMoved>, that's raised when the pointer moves while remaining within the bounding area of the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressed>, that's raised when the pointer initiates a press within the view.
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleased>, that's raised when the pointer that has previously initiated a press is released, while within the view.

A <xref:Microsoft.Maui.Controls.PointerEventArgs> object accompanies the events, and defines a <xref:Microsoft.Maui.Controls.PointerEventArgs.PlatformArgs> property of type <xref:Microsoft.Maui.Controls.PlatformPointerEventArgs> that provides access to platform-specific arguments for the event.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

On Android, the <xref:Microsoft.Maui.Controls.PlatformPointerEventArgs> class defines the following properties:

- `Sender`, of type <xref:Android.Views.View>, represents the native view attached to the event.
- `MotionEvent`, of type <xref:Android.Views.MotionEvent>, indicates the native event or handler attached to the view.

# [iOS/Mac Catalyst](#tab/macios)

On iOS and Mac Catalyst, the <xref:Microsoft.Maui.Controls.PlatformPointerEventArgs> class defines the following properties:

- `Sender`, of type <xref:UIKit.UIView>, represents the native view attached to the event.
- `GestureRecognizer`, of type <xref:UIKit.UIGestureRecognizer>, indicates the native event or handler attached to the view.

# [Windows](#tab/windows)

On Windows, the <xref:Microsoft.Maui.Controls.PlatformPointerEventArgs> class defines the following properties:

- `Sender`, of type <xref:Microsoft.UI.Xaml.FrameworkElement>, represents the native view attached to the event.
- `PointerRoutedEventArgs`, of type <xref:Microsoft.UI.Xaml.Input.PointerRoutedEventArgs>, indicates the native event or handler attached to the view.

---

<!-- markdownlint-enable MD025 -->

In addition, the <xref:Microsoft.Maui.Controls.PointerEventArgs> object defines a <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method that returns a `Point?` object that represents the position of the pointer when the gesture was detected. For more information about the <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method, see [Get the gesture position](#get-the-gesture-position).

::: moniker-end

> [!IMPORTANT]
> Pointer gesture recognition is supported on Android, iPadOS, Mac Catalyst, and Windows.

.NET MAUI also defines a `PointerOver` visual state. This state can change the visual appearance of a view when it has a mouse cursor hovering over it, but isn't pressed. For more information, see [Visual states](~/user-interface/visual-states.md).

## Create a PointerGestureRecognizer

To make a <xref:Microsoft.Maui.Controls.View> recognize pointer gestures, create a <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> object, handle the required events, and add the gesture recognizer to the <xref:Microsoft.Maui.Controls.View.GestureRecognizers> collection on the view.
Alternatively, create a <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> object, and bind the required commands to <xref:System.Windows.Input.ICommand> implementations, and add the gesture recognizer to the <xref:Microsoft.Maui.Controls.View.GestureRecognizers> collection on the view.

The following code example shows a <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> attached to an <xref:Microsoft.Maui.Controls.Image>. The <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> uses events to respond to the detection of pointer gestures:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <PointerGestureRecognizer PointerEntered="OnPointerEntered"
                                  PointerExited="OnPointerExited"
                                  PointerMoved="OnPointerMoved" />
  </Image.GestureRecognizers>
</Image>
```

The code for the event handlers should be added to the code-behind file:

```csharp
void OnPointerEntered(object sender, PointerEventArgs e)
{
    // Handle the pointer entered event
}

void OnPointerExited(object sender, PointerEventArgs e)
{
    // Handle the pointer exited event
}

void OnPointerMoved(object sender, PointerEventArgs e)
{
    // Handle the pointer moved event
}
```

The equivalent C# code is:

```csharp
PointerGestureRecognizer pointerGestureRecognizer = new PointerGestureRecognizer();
pointerGestureRecognizer.PointerEntered += (s, e) =>
{
    // Handle the pointer entered event
};
pointerGestureRecognizer.PointerExited += (s, e) =>
{
    // Handle the pointer exited event
};
pointerGestureRecognizer.PointerMoved += (s, e) =>
{
    // Handle the pointer moved event
};

Image image = new Image();
image.GestureRecognizers.Add(pointerGestureRecognizer);
```

## Get the gesture position

The position at which a pointer gesture occurred can be obtained by calling the <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method on a <xref:Microsoft.Maui.Controls.PointerEventArgs> object. The <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method accepts an `Element?` argument, and returns a position as a `Point?` object:

```csharp
void OnPointerExited(object sender, PointerEventArgs e)
{
    // Position inside window
    Point? windowPosition = e.GetPosition(null);

    // Position relative to an Image
    Point? relativeToImagePosition = e.GetPosition(image);

    // Position relative to the container view
    Point? relativeToContainerPosition = e.GetPosition((View)sender);
}
```

The `Element?` argument defines the element the position should be obtained relative to. Supplying a `null` value as this argument means that the <xref:Microsoft.Maui.Controls.PointerEventArgs.GetPosition%2A> method returns a `Point?` object that defines the position of the pointer gesture inside the window.

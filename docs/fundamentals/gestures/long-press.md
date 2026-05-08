---
title: "Recognize a long press gesture"
description: "This article explains how to recognize a long press gesture in a .NET MAUI app, using LongPressGestureRecognizer."
ms.date: 03/04/2026
---

# Recognize a long press gesture

::: moniker range=">=net-maui-11.0"

A .NET Multi-platform App UI (.NET MAUI) long press gesture recognizer is used to detect when the user presses and holds on an element for a specified duration. It is implemented with the `LongPressGestureRecognizer` class.

The `LongPressGestureRecognizer` class defines the following properties:

- `LongPressGestureRecognizer.Command`, of type <xref:System.Windows.Input.ICommand>, which is executed when a long press gesture is recognized.
- `LongPressGestureRecognizer.CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.
- `LongPressGestureRecognizer.MinimumPressDuration`, of type `int`, which represents the minimum duration in milliseconds the user must press before the gesture is recognized. The default value is 500.
- `LongPressGestureRecognizer.NumberOfTouchesRequired`, of type `int`, which represents the number of fingers required for the gesture to be recognized. The default value is 1. This property is only supported on iOS and Mac Catalyst.
- `LongPressGestureRecognizer.AllowableMovement`, of type `double`, which represents the maximum distance in pixels the touch can move before the gesture is cancelled. The default value is 10.
- `LongPressGestureRecognizer.State`, of type <xref:Microsoft.Maui.GestureStatus>, which gets the current state of the gesture.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The `LongPressGestureRecognizer` class also defines two events:

- `LongPressGestureRecognizer.LongPressed`, which is raised when the long press gesture is completed (the user releases after the minimum duration has elapsed). The `LongPressedEventArgs` object provides a `Parameter` property and a `GetPosition` method.
- `LongPressGestureRecognizer.LongPressing`, which is raised when the gesture state changes. The `LongPressingEventArgs` object provides a `Status` property of type <xref:Microsoft.Maui.GestureStatus> and a `GetPosition` method. This event is primarily useful on iOS and Mac Catalyst, where it provides real-time `Started`, `Running`, `Completed`, and `Canceled` state updates.

## Create a LongPressGestureRecognizer

To make a <xref:Microsoft.Maui.Controls.View> recognize a long press gesture, create a `LongPressGestureRecognizer` object, handle the `LongPressGestureRecognizer.LongPressed` event, and add the gesture recognizer to the `GestureRecognizers` collection on the view. The following XAML example shows a `LongPressGestureRecognizer` attached to an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <LongPressGestureRecognizer LongPressed="OnLongPressed" />
    </Image.GestureRecognizers>
</Image>
```

The code for the `OnLongPressed` event handler should be added to the code-behind file:

```csharp
void OnLongPressed(object sender, LongPressedEventArgs args)
{
    // Handle the long press
}
```

The equivalent C# code is:

```csharp
var longPressGesture = new LongPressGestureRecognizer();
longPressGesture.LongPressed += (s, e) =>
{
    // Handle the long press
};
image.GestureRecognizers.Add(longPressGesture);
```

## Use a command

The `LongPressGestureRecognizer.Command` property provides an alternative to the event-based approach. The command is executed when the long press is recognized:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <LongPressGestureRecognizer Command="{Binding LongPressCommand}"
                                    CommandParameter="dotnet_bot" />
    </Image.GestureRecognizers>
</Image>
```

## Configure the press duration

The `LongPressGestureRecognizer.MinimumPressDuration` property specifies how long (in milliseconds) the user must press before the gesture is recognized. The default value is 500 milliseconds:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <LongPressGestureRecognizer LongPressed="OnLongPressed"
                                    MinimumPressDuration="1000" />
    </Image.GestureRecognizers>
</Image>
```

> [!NOTE]
> On Android, the `LongPressGestureRecognizer.MinimumPressDuration` property is not configurable. The platform uses the system-default long press timeout (typically 400ms), which cannot be changed per gesture recognizer. The property value is ignored on Android.

## Track gesture state changes

The `LongPressGestureRecognizer.LongPressing` event provides real-time state updates during the gesture. The `LongPressingEventArgs.Status` property indicates the current <xref:Microsoft.Maui.GestureStatus>:

| Status | Description |
|--------|-------------|
| `Started` | The press has been held long enough for the gesture to be recognized. |
| `Running` | The touch is still being held down after recognition. |
| `Completed` | The user has released the touch after a successful long press. |
| `Canceled` | The gesture was cancelled, for example because the touch moved beyond the `LongPressGestureRecognizer.AllowableMovement` threshold. |

The following example shows how to handle state changes:

```csharp
var longPressGesture = new LongPressGestureRecognizer();
longPressGesture.LongPressing += (s, e) =>
{
    switch (e.Status)
    {
        case GestureStatus.Started:
            statusLabel.Text = "Long press started!";
            break;
        case GestureStatus.Running:
            statusLabel.Text = "Still pressing...";
            break;
        case GestureStatus.Completed:
            statusLabel.Text = "Long press completed!";
            break;
        case GestureStatus.Canceled:
            statusLabel.Text = "Long press cancelled.";
            break;
    }
};
image.GestureRecognizers.Add(longPressGesture);
```

> [!NOTE]
> On Android and Windows, the `LongPressing` event fires when the gesture completes or is cancelled, but does not provide `Started` or `Running` state updates. Full state tracking is available on iOS and Mac Catalyst.

## Get the gesture position

The position at which the long press gesture occurred can be obtained by calling the `GetPosition` method on a `LongPressedEventArgs` or `LongPressingEventArgs` object. The `GetPosition` method accepts an `Element?` argument, and returns the position as a `Point?` object:

```csharp
void OnLongPressed(object sender, LongPressedEventArgs e)
{
    // Position inside the window
    Point? windowPosition = e.GetPosition(null);

    // Position relative to an Image
    Point? relativeToImagePosition = e.GetPosition(image);

    // Position relative to the container view
    Point? relativeToContainerPosition = e.GetPosition((View)sender);
}
```

Supplying `null` as the argument means that the `GetPosition` method returns a `Point?` object that defines the position of the long press gesture inside the window.

## Combine with other gestures

A `LongPressGestureRecognizer` can be combined with other gesture recognizers on the same view. For example, you can add both a tap gesture and a long press gesture to handle different interaction types:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTapped" />
        <LongPressGestureRecognizer LongPressed="OnLongPressed" />
    </Image.GestureRecognizers>
</Image>
```

On iOS and Mac Catalyst, the long press gesture recognizer is configured to require the tap gesture to fail first, so that short taps are correctly routed to the tap gesture recognizer.

## Platform differences

The following table summarizes the platform-specific behavior of the `LongPressGestureRecognizer`:

| Feature | Android | iOS / Mac Catalyst | Windows |
|---------|---------|-------------------|---------|
| `LongPressGestureRecognizer.MinimumPressDuration` | Ignored (uses system default ~400ms) | ✅ Configurable | ✅ Configurable |
| `LongPressGestureRecognizer.NumberOfTouchesRequired` | Ignored (always 1) | ✅ Configurable | Ignored (always 1) |
| `LongPressGestureRecognizer.AllowableMovement` | ✅ Supported | ✅ Supported | ✅ Supported |
| `LongPressing` state updates | Completed/Canceled only | Started, Running, Completed, Canceled | Completed/Canceled only |
| `LongPressed` event | ✅ | ✅ | ✅ |
| `GetPosition` | ✅ | ✅ | ✅ |

::: moniker-end

::: moniker range="<=net-maui-10.0"

The long press gesture recognizer is available starting in .NET MAUI 11. For earlier versions, consider using a <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> with custom timing logic, or implementing a platform-specific effect.

::: moniker-end

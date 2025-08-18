---
title: "Recognize a tap gesture"
description: "This article explains how to recognize a tap gesture in a .NET MAUI app."
ms.date: 08/18/2025
---

# Recognize a tap gesture

A .NET Multi-platform App UI (.NET MAUI) tap gesture recognizer is used for tap detection and is implemented with the <xref:Microsoft.Maui.Controls.TapGestureRecognizer> class.

::: moniker range=">=net-maui-10.0"
> [!IMPORTANT]
> In .NET 10, <xref:Microsoft.Maui.Controls.ClickGestureRecognizer> is deprecated. Use <xref:Microsoft.Maui.Controls.TapGestureRecognizer> for tap/click interactions, and <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> for pointer hover/move/press scenarios. The <xref:Microsoft.Maui.Controls.TapGestureRecognizer> supports primary and secondary buttons via the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Buttons> property and can be used to handle single and double tap gestures.
:::

This class defines the following properties:

- <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Buttons>, of type <xref:Microsoft.Maui.Controls.ButtonsMask>, which defines whether the primary or secondary mouse button, or both, triggers the gesture on Android, Mac Catalyst, and Windows. For more information, see [Define the button masks](#define-the-button-mask).
- <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Command>, of type <xref:System.Windows.Input.ICommand>, which is executed when a tap is recognized.
- <xref:Microsoft.Maui.Controls.TapGestureRecognizer.CommandParameter>, of type `object`, which is the parameter that's passed to the `Command`.
- <xref:Microsoft.Maui.Controls.TapGestureRecognizer.NumberOfTapsRequired>, of type `int`, which represents the number of taps required to recognize a tap gesture. The default value of this property is 1.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.TapGestureRecognizer> class also defines a <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Tapped> event that's raised when a tap is recognized. The <xref:Microsoft.Maui.Controls.TappedEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Tapped> event defines a <xref:Microsoft.Maui.Controls.TappedEventArgs.Parameter> property of type `object` that indicates the value passed by the `CommandParameter` property, if defined. The <xref:Microsoft.Maui.Controls.TappedEventArgs> object also defines a <xref:Microsoft.Maui.Controls.TappedEventArgs.Buttons> property, and a `GetPosition` method. The <xref:Microsoft.Maui.Controls.TappedEventArgs.Buttons> property is of type <xref:Microsoft.Maui.Controls.ButtonsMask>, and can be used to determine whether the primary or secondary mouse button triggered the gesture recognizer on Android, Mac Catalyst, and Windows. The `GetPosition` method returns a `Point?` object that represents the position at which the tap gesture was detected. For more information about button masks, see [Define the button mask](#define-the-button-mask). For more information about the `GetPosition` method, see [Get the gesture position](#get-the-gesture-position).

> [!WARNING]
> A <xref:Microsoft.Maui.Controls.TapGestureRecognizer> can't recognize more than a double tap on Windows.

## Create a TapGestureRecognizer

To make a <xref:Microsoft.Maui.Controls.View> recognize a tap gesture, create a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> object, handle the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Tapped> event, and add the new gesture recognizer to the `GestureRecognizers` collection on the view. The following code example shows a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> attached to an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTapGestureRecognizerTapped"
                              NumberOfTapsRequired="2" />
  </Image.GestureRecognizers>
</Image>
```

The code for the `OnTapGestureRecognizerTapped` event handler should be added to the code-behind file:

```csharp
void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
{
    // Handle the tap
}
```

The equivalent C# code is:

```csharp
TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
tapGestureRecognizer.Tapped += (s, e) =>
{
    // Handle the tap
};
Image image = new Image();
image.GestureRecognizers.Add(tapGestureRecognizer);
```

By default the <xref:Microsoft.Maui.Controls.Image> will respond to single taps. When the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.NumberOfTapsRequired> property is set to greater than one, the event handler will only be executed if the taps occur within a set period of time. If the second (or subsequent) taps don't occur within that period, they're effectively ignored.

::: moniker range=">=net-maui-10.0"

## Migrate from ClickGestureRecognizer (>= .NET 10)

In previous versions, mouse click interactions could be handled with <xref:Microsoft.Maui.Controls.ClickGestureRecognizer>. In .NET 10, this recognizer is deprecated. Migrate to <xref:Microsoft.Maui.Controls.TapGestureRecognizer> for click/tap interactions, and use <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> for pointer enter/exit/move/press/release scenarios.

The following example shows how to migrate from `ClickGestureRecognizer` to `TapGestureRecognizer` while maintaining support for primary/secondary buttons:

```xaml
<!-- Before (.NET 9 and earlier) -->
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <ClickGestureRecognizer Clicked="OnClicked"
                                                        NumberOfClicksRequired="1"
                                                        Buttons="Primary,Secondary" />
    </Image.GestureRecognizers>
</Image>

<!-- After (.NET 10) -->
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTapped"
                                                    NumberOfTapsRequired="1"
                                                    Buttons="Primary,Secondary" />
    </Image.GestureRecognizers>
</Image>
```

In code-behind, handle the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Tapped> event and inspect <xref:Microsoft.Maui.Controls.TappedEventArgs.Buttons> to distinguish the mouse button:

```csharp
void OnTapped(object sender, TappedEventArgs e)
{
        if (e.Buttons == ButtonsMask.Secondary)
        {
                // Handle secondary/right click
        }
        else
        {
                // Handle primary/left click
        }
}
```

If you need pointer hover or press/release details (without a completed tap), use <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> and its events such as `PointerEntered`, `PointerMoved`, `PointerPressed`, and `PointerReleased`. For more information, see [Recognize a pointer gesture](pointer.md).
:::

## Define the button mask

A <xref:Microsoft.Maui.Controls.TapGestureRecognizer> object has a <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Buttons> property, of type <xref:Microsoft.Maui.Controls.ButtonsMask>, that defines whether the primary or secondary mouse button, or both, triggers the gesture on Android, Mac Catalyst, and Windows. The <xref:Microsoft.Maui.Controls.ButtonsMask> enumeration defines the following members:

- <xref:Microsoft.Maui.Controls.ButtonsMask.Primary> represents the primary mouse button, which is typically the left mouse button.
- <xref:Microsoft.Maui.Controls.ButtonsMask.Secondary> represents the secondary mouse button, which is typically the right mouse button.

The following example shows a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> that detects taps with the secondary mouse button:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTapGestureRecognizerTapped"
                              Buttons="Secondary" />
  </Image.GestureRecognizers>
</Image>
```

The event handler for the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.Tapped> event can determine which button triggered the gesture:

```csharp
void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
{
    // Handle the tap
    if (args.Buttons == ButtonsMask.Secondary)
    {
        // Do something
    }
}
```

The equivalent C# code is:

```csharp
TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer
{
    Buttons = ButtonsMask.Secondary
};
tapGestureRecognizer.Tapped += (s, e) =>
{
    // Handle the tap
    if (args.Buttons == ButtonsMask.Secondary)
    {
        // Do something
    }
};
Image image = new Image();
image.GestureRecognizers.Add(tapGestureRecognizer);
```

> [!WARNING]
> On Windows, a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> that sets the `Buttons` property to `Secondary` doesn't respect the <xref:Microsoft.Maui.Controls.TapGestureRecognizer.NumberOfTapsRequired> property when it's greater than one.

In addition, a <xref:Microsoft.Maui.Controls.TapGestureRecognizer> can be defined so that either the primary or secondary mouse button triggers the gesture:

```xaml
<TapGestureRecognizer Tapped="OnTapGestureRecognizerTapped"
                      Buttons="Primary,Secondary" />
```

The equivalent C# code is:

```csharp
TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer
{
    Buttons = ButtonsMask.Primary | ButtonsMask.Secondary
};
```

## Get the gesture position

The position at which a tap gesture occurred can be obtained by calling the `GetPosition` method on a <xref:Microsoft.Maui.Controls.TappedEventArgs> object. The `GetPosition` method accepts an `Element?` argument, and returns a position as a `Point?` object:

```csharp
void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
{
    // Position inside window
    Point? windowPosition = e.GetPosition(null);

    // Position relative to an Image
    Point? relativeToImagePosition = e.GetPosition(image);

    // Position relative to the container view
    Point? relativeToContainerPosition = e.GetPosition((View)sender);
}
```

The `Element?` argument defines the element the position should be obtained relative to. Supplying a `null` value as this argument means that the `GetPosition` method returns a `Point?` object that defines the position of the tap gesture inside the window.

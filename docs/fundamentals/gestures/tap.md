---
title: "Recognize a tap gesture"
description: "This article explains how to recognize a tap gesture in a .NET MAUI app."
ms.date: 10/03/2022
---

# Recognize a tap gesture

A .NET Multi-platform App UI (.NET MAUI) tap gesture recognizer is used for tap detection and is implemented with the `TapGestureRecognizer` class. This class defines the following properties:

::: moniker range="=net-maui-6.0"
- `Command`, of type `ICommand`, which is executed when a tap is recognized.
- `CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.
- `NumberOfTapsRequired`, of type `int`, which represents the number of taps required to recognize a tap gesture. The default value of this property is 1.
::: moniker-end

::: moniker range=">=net-maui-7.0"
- `Buttons`, of type `ButtonsMask`, which defines whether the primary or secondary mouse button, or both, triggers the gesture on Mac Catalyst and Windows. For more information, see [Button masks](#button-masks).
- `Command`, of type `ICommand`, which is executed when a tap is recognized.
- `CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.
- `NumberOfTapsRequired`, of type `int`, which represents the number of taps required to recognize a tap gesture. The default value of this property is 1.
::: moniker-end

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

The `TapGestureRecognizer` class also defines a `Tapped` event that's raised when a tap is recognized. The `TappedEventArgs` object that accompanies the `Tapped` event defines a `Parameter` property of type `object` that indicates the value passed by the `CommandParameter` property, if defined.

::: moniker range=">=net-maui-7.0"

The `TappedEventArgs` object also defines a `Buttons` property, and a `GetPosition` method. The `Buttons` property is of type `ButtonsMask`, and can be used to determine whether the primary or secondary mouse button triggered the gesture recognizer on Windows. The `GetPosition` method returns a `Point?` object that represents the position at which the tap gesture was detected. For more information about button masks, see [Button masks](#button-masks). For more information about the `GetPosition` method, see [Get the gesture position](#get-the-pointer-position).

::: moniker-end

## Create a TapGestureRecognizer

To make a `View` recognize a tap gesture, create a `TapGestureRecognizer` object, handle the `Tapped` event, and add the new gesture recognizer to the `GestureRecognizers` collection on the view. The following code example shows a `TapGestureRecognizer` attached to an `Image`:

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

By default the `Image` will respond to single taps. When the `NumberOfTapsRequired` property is set to above one, the event handler will only be executed if the taps occur within a set period of time. If the second (or subsequent) taps don't occur within that period, they're effectively ignored.

::: moniker range=">=net-maui-7.0"

## Button masks

A `TapGestureRecognizer` object has a `Buttons` property, of type `ButtonsMask`, which defines whether the primary or secondary mouse button, or both, triggers the gesture on Mac Catalyst and Windows. The `ButtonsMask` enumeration defines the following members:

- `Primary` represents the primary mouse button, which is typically the left mouse button.
- `Secondary` represents the secondary mouse button, which is typically the right mouse button.

The following example shows a `TapGestureRecognizer` that detects taps with the secondary mouse button:

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTapGestureRecognizerTapped"
                              Buttons="Secondary" />
  </Image.GestureRecognizers>
</Image>
```

The code for the `OnTapGestureRecognizerTapped` event handler can determine which button triggered the gesture:

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
> A `TapGestureRecognizer` that sets the `Buttons` property to `Secondary` currently doesn't work when the `NumberOfTapsRequired` required property is set to above one.

In addition, a `TapGestureRecognizer` can be defined so that either the primary or secondary mouse button triggers the gesture on Windows:

```xaml
<TapGestureRecognizer Tapped="OnTapGestureRecognizerTapped"
                      Buttons="Primary,Secondary" />
```

The equivalent C# code is:

```csharp
TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer
{
    Buttons = ButtonsMask.Secondary | ButtonsMask.Primary
};
```

## Get the gesture position

The position at which a tap gesture occurred can be obtained by calling the `GetPosition` method on a `TappedEventArgs` object. The `GetPosition` method accepts an `Element?` argument, and returns a position as a `Point?` object:

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

::: moniker-end

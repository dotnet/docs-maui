---
title: "Recognize a tap gesture"
description: "This article explains how to recognize a tap gesture in a .NET MAUI app."
ms.date: 02/23/2022
---

# Recognize a tap gesture

A .NET Multi-platform App UI (.NET MAUI) tap gesture recognizer is used for tap detection and is implemented with the `TapGestureRecognizer` class. This class defines the following properties:

- `Command`, of type `ICommand`, which is executed when a tap is recognized.
- `CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.
- `NumberOfTapsRequired`, of type `int`, which represents the number of taps required to recognize a tap gesture. The default value of this property is 1.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

The `TapGestureRecognizer` class also defines a `Tapped` event that's raised when a tap is recognized. The `TappedEventArgs` object that accompanies the `Tapped` event defines a `Parameter` property of type `object` that indicates the value passed by the `CommandParameter` property, if defined.

## Create a TapGestureRecognizer

To make a `View` recognize a tap gesture, create a `TapGestureRecognizer` object, handle the `Tapped` event, and add the new gesture recognizer to the `GestureRecognizers` collection on the view. The following code example shows a `TapGestureRecognizer` attached to an `Image`:

```xaml
<Image Source="tapped.jpg">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Tapped="OnTapGestureRecognizerTapped"
                              NumberOfTapsRequired="2" />
  </Image.GestureRecognizers>
</Image>
```

The code for the `OnTapGestureRecognizerTapped` event handler should be added to the code-behind file:

```csharp
void OnTapGestureRecognizerTapped(object sender, EventArgs args)
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
image.GestureRecognizers.Add(tapGestureRecognizer);
```

By default the `Image` will respond to single taps. When the `NumberOfTapsRequired` property is set above one, the event handler will only be executed if the taps occur within a set period of time. If the second (or subsequent) taps don't occur within that period, they're effectively ignored.

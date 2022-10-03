---
title: "Recognize a pointer gesture"
description: "Learn how to use the PointerGestureRecognizer class, to detect when the pointer enters, exits, and moves within a view on iPadOS, Mac Catalyst, and Windows."
monikerRange: ">= net-maui-7.0"
ms.date: 10/03/2022
---

# Recognize a pointer gesture

A .NET Multi-platform App UI (.NET MAUI) pointer gesture recognizer detects when the pointer enters, exits, and moves within a view and is implemented with the `PointerGestureRecognizer` class. This class defines the following properties:

- `PointerEnteredCommand`, of type `ICommand`, which is the command to invoke when the pointer enters the bounding area of the view.
- `PointerEnteredCommandParameter`, of type `object`, which is the parameter that's passed to `PointerEnteredCommand`.
- `PointerExitedCommand`, of type `ICommand`, which is the command to invoke when the pointer that's in the view's bounding area leaves that bounding area.
- `PointerExitedCommandParameter`, of type `object`, which is the parameter that's passed to `PointerExitedCommand`.
- `PointerMovedCommand`, of type `ICommand`, which is the command to invoke when the pointer moves while remaining within the bounding area of the view.
- `PointerMovedCommandParameter`, of type `object`, which is the parameter that's passed to `PointerMovedCommand`.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

The `PointerGestureRecognizer` class also defines the following events:

- `PointerEntered`, that's raised when the pointer enters the bounding area of the view.
- `PointerExited`, that's raised when the pointer that's in the view's bounding area leaves that bounding area.
- `PointerMoved`, that's raised when the pointer moves while remaining within the bounding area of the view.

A `PointerEventArgs` object accompanies all three events, and defines a `GetPosition` method that returns a `Point?` object that represents the position of the pointer when the gesture was detected.

> [!IMPORTANT]
> Pointer gesture recognition is only supported on iPadOS, Mac Catalyst, and Windows.

## Create a PointerGestureRecognizer

To make a `View` recognize pointer gestures, create a `PointerGestureRecognizer` object, handle the required events, and add the gesture recognizer to the `GestureRecognizers` collection on the view.
Alternatively, create a `PointerGestureRecognizer` object, and bind the required commands to `ICommand` implementations, and add the gesture recognizer to the `GestureRecognizers` collection on the view.

The following code example shows a `PointerGestureRecognizer` attached to an `Image`, that uses events to respond to the detection of pointer gestures:

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

## Get the pointer position

The position at which a pointer gesture occurred can be obtained by calling the `GetPosition` method on a `PointerEventArgs` object:

```csharp
void OnPointerExited(object sender, PointerEventArgs e)
{
    Point? position = e.GetPosition((View)sender);
}
```

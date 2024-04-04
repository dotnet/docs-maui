---
title: "Recognize a swipe gesture"
description: "This article explains how to recognize a swipe gesture occurring on a view in .NET MAUI."
ms.date: 02/22/2022
---

# Recognize a swipe gesture

A .NET Multi-platform App UI (.NET MAUI) swipe gesture recognizer detects when a finger is moved across the screen in a horizontal or vertical direction, and is often used to initiate navigation through content.

In .NET MAUI, drag gesture recognition is provided by the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> class. This class defines the following properties:

- <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Command>, of type <xref:System.Windows.Input.ICommand>, which is executed when a swipe gesture is recognized.
- <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.CommandParameter>, of type `object`, which is the parameter that's passed to the `Command`.
- <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Direction>, of type <xref:Microsoft.Maui.SwipeDirection>, which defines the direction
- <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Threshold>, of type `uint`, which represents the minimum swipe distance that must be achieved for a swipe to be recognized, in device-independent units. The default value of this property is 100, which means that any swipes that are less than 100 device-independent units will be ignored.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> also defines a <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Swiped> event that's raised when a swipe is recognized. The <xref:Microsoft.Maui.Controls.SwipedEventArgs> object that accompanies the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Swiped> event defines the following properties:

- <xref:Microsoft.Maui.Controls.SwipedEventArgs.Direction>, of type <xref:Microsoft.Maui.SwipeDirection>, indicates the direction of the swipe gesture.
- <xref:Microsoft.Maui.Controls.SwipedEventArgs.Parameter>, of type `object`, indicates the value passed by the `CommandParameter` property, if defined.

## Create a SwipeGestureRecognizer

To make a <xref:Microsoft.Maui.Controls.View> recognize a swipe gesture, create a <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> object, set the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Direction> property to a <xref:Microsoft.Maui.SwipeDirection> enumeration value (`Left`, `Right`, `Up`, or `Down`), optionally set the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Threshold> property, handle the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Swiped> event, and add the new gesture recognizer to the `GestureRecognizers` collection on the view. The following example shows a <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> attached to a <xref:Microsoft.Maui.Controls.BoxView>:

```xaml
<BoxView Color="Teal" ...>
    <BoxView.GestureRecognizers>
        <SwipeGestureRecognizer Direction="Left" Swiped="OnSwiped"/>
    </BoxView.GestureRecognizers>
</BoxView>
```

The equivalent C# code is:

```csharp
BoxView boxView = new BoxView { Color = Colors.Teal, ... };
SwipeGestureRecognizer leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
leftSwipeGesture.Swiped += OnSwiped;

boxView.GestureRecognizers.Add(leftSwipeGesture);
```

## Recognize the swipe direction

The `SwipeGestureRecognizer.Direction` property can be set to a single value from the <xref:Microsoft.Maui.SwipeDirection> enumeration, or multiple values. This enables the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Swiped> event to be raised in response to a swipe in more than one direction. However, the constraint is that a single <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> can only recognize swipes that occur on the same axis. Therefore, swipes that occur on the horizontal axis can be recognized by setting the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Direction> property to `Left` and `Right`:

```xaml
<SwipeGestureRecognizer Direction="Left,Right" Swiped="OnSwiped"/>
```

Similarly, swipes that occur on the vertical axis can be recognized by setting the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Direction> property to `Up` and `Down`:

```csharp
SwipeGestureRecognizer swipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Up | SwipeDirection.Down };
```

Alternatively, a <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> for each swipe direction can be created to recognize swipes in every direction:

```xaml
<BoxView Color="Teal" ...>
    <BoxView.GestureRecognizers>
        <SwipeGestureRecognizer Direction="Left" Swiped="OnSwiped"/>
        <SwipeGestureRecognizer Direction="Right" Swiped="OnSwiped"/>
        <SwipeGestureRecognizer Direction="Up" Swiped="OnSwiped"/>
        <SwipeGestureRecognizer Direction="Down" Swiped="OnSwiped"/>
    </BoxView.GestureRecognizers>
</BoxView>
```

The equivalent C# code is:

```csharp
BoxView boxView = new BoxView { Color = Colors.Teal, ... };
SwipeGestureRecognizer leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
leftSwipeGesture.Swiped += OnSwiped;
SwipeGestureRecognizer  rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
rightSwipeGesture.Swiped += OnSwiped;
SwipeGestureRecognizer  upSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Up };
upSwipeGesture.Swiped += OnSwiped;
SwipeGestureRecognizer  downSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
downSwipeGesture.Swiped += OnSwiped;

boxView.GestureRecognizers.Add(leftSwipeGesture);
boxView.GestureRecognizers.Add(rightSwipeGesture);
boxView.GestureRecognizers.Add(upSwipeGesture);
boxView.GestureRecognizers.Add(downSwipeGesture);
```

## Respond to a swipe

A recognized swipe can be responded to by a handler for the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Swiped> event:

```csharp
void OnSwiped(object sender, SwipedEventArgs e)
{
    switch (e.Direction)
    {
        case SwipeDirection.Left:
            // Handle the swipe
            break;
        case SwipeDirection.Right:
            // Handle the swipe
            break;
        case SwipeDirection.Up:
            // Handle the swipe
            break;
        case SwipeDirection.Down:
            // Handle the swipe
            break;
    }
}
```

The <xref:Microsoft.Maui.Controls.SwipedEventArgs> can be examined to determine the direction of the swipe, with custom logic responding to the swipe as required. The direction of the swipe can be obtained from the <xref:Microsoft.Maui.Controls.SwipedEventArgs.Direction> property of the event arguments, which will be set to one of the values of the <xref:Microsoft.Maui.SwipeDirection> enumeration. In addition, the event arguments also have a <xref:Microsoft.Maui.Controls.SwipedEventArgs.Parameter> property that will be set to the value of the `CommandParameter` property, if defined.

## Create a swipe container

The `SwipeContainer` class, which is shown in the following example, is a generalized swipe recognition class that be wrapped around a <xref:Microsoft.Maui.Controls.View> to perform swipe gesture recognition:

```csharp
public class SwipeContainer : ContentView
{
    public event EventHandler<SwipedEventArgs> Swipe;

    public SwipeContainer()
    {
        GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Left));
        GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Right));
        GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Up));
        GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Down));
    }

    SwipeGestureRecognizer GetSwipeGestureRecognizer(SwipeDirection direction)
    {
        SwipeGestureRecognizer swipe = new SwipeGestureRecognizer { Direction = direction };
        swipe.Swiped += (sender, e) => Swipe?.Invoke(this, e);
        return swipe;
    }
}
```

The `SwipeContainer` class creates <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> objects for all four swipe directions, and attaches `Swipe` event handlers. These event handlers invoke the `Swipe` event defined by the `SwipeContainer`.

The following XAML code example shows the `SwipeContainer` class wrapping a <xref:Microsoft.Maui.Controls.BoxView>:

```xaml
<StackLayout>
    <local:SwipeContainer Swipe="OnSwiped" ...>
        <BoxView Color="Teal" ... />
    </local:SwipeContainer>
</StackLayout>
```

In this example, when the <xref:Microsoft.Maui.Controls.BoxView> receives a swipe gesture, the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer.Swiped> event in the <xref:Microsoft.Maui.Controls.SwipeGestureRecognizer> is raised. This is handled by the `SwipeContainer` class, which raises its own `Swipe` event. This `Swipe` event is handled on the page. The <xref:Microsoft.Maui.Controls.SwipedEventArgs> can then be examined to determine the direction of the swipe, with custom logic responding to the swipe as required.

The equivalent C# code is:

```csharp
BoxView boxView = new BoxView { Color = Colors.Teal, ... };
SwipeContainer swipeContainer = new SwipeContainer { Content = boxView, ... };
swipeContainer.Swipe += (sender, e) =>
{
  // Handle the swipe
};

StackLayout stackLayout = new StackLayout();
stackLayout.Add(swipeContainer);
```

---
title: "Data binding and MVVM"
description: "The Model-View-ViewModel (MVVM) pattern enforces a separation between three software layers — the XAML user interface, called the view, the underlying data, called the model, and an intermediary between the view and the model, called the viewmodel."
ms.date: 06/09/2022
---

# Data binding and MVVM

The Model-View-ViewModel (MVVM) pattern enforces a separation between three software layers — the XAML user interface, called the view, the underlying data, called the model, and an intermediary between the view and the model, called the viewmodel. The view and the viewmodel are often connected through data bindings defined in XAML. The `BindingContext` for the view is usually an instance of the viewmodel.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Simple MVVM

In [XAML markup extensions](markup-extensions.md) you saw how to define a new XML namespace declaration to allow a XAML file to reference classes in other assemblies. The following example uses the `x:Static` markup extension to obtain the current date and time from the static `DateTime.Now` property in the `System` namespace:

:::code language="xaml" source="snippets/mvvm/csharp/MainPage.xaml":::

In this example, the retrieved `DateTime` value is set as the `BindingContext` on a `StackLayout`. When you set the `BindingContext` on an element, it is inherited by all the children of that element. This means that all the children of the `StackLayout` have the same `BindingContext`, and they can contain bindings to properties of that object:

:::image type="content" source="media/mvvm/oneshotdatetime.png" alt-text="Screenshot of a page displaying the date and time.":::

However, the problem is that the date and time are set once when the page is constructed and initialized, and never change.

A XAML page can display a clock that always shows the current time, but it requires additional code. The MVVM pattern is a natural choice for .NET MAUI apps when data binding from properties between visual objects and the underlying data. When thinking in terms of MVVM, the model and viewmodel are classes written entirely in code. The view is often a XAML file that references properties defined in the viewmodel through data bindings. In MVVM, a model is ignorant of the viewmodel, and a viewmodel is ignorant of the view. However, often you tailor the types exposed by the viewmodel to the types associated with the UI.

> [!NOTE]
> In simple examples of MVVM, such as those shown here, often there is no model at all, and the pattern involves just a view and viewmodel linked with data bindings.

The following example shows a viewmodel for a clock, with a single property named `DateTime` that's updated every second:

:::code language="csharp" source="snippets/mvvm/csharp/ClockViewModel.cs":::

Viewmodels typically implement the `INotifyPropertyChanged` interface, which provides the ability for a class to raise the `PropertyChanged` event whenever one of its properties changes. The data binding mechanism in .NET MAUI attaches a handler to this `PropertyChanged` event so it can be notified when a property changes and keep the target updated with the new value. In the previous code example, the `OnPropertyChanged` method handles raising the event while automatically determining the property source name: `DateTime`.

The following example shows XAML that consumes `ClockViewModel`:

:::code language="xaml" source="snippets/mvvm/csharp/ClockPage.xaml":::

In this example, `ClockViewModel` is set to the `BindingContext` of the `ContentPage` using property element tags. Alternatively, the code-behind file could instantiate the viewmodel.

The `Binding` markup extension on the `Text` property of the `Label` formats the `DateTime` property. The following screenshot shows the result:

:::image type="content" source="media/mvvm/clock.png" alt-text="Screenshot of a page displaying the date and time via a viewmodel.":::

In addition, it’s possible to access individual properties of the `DateTime` property of the viewmodel by separating the properties with periods:

```xaml
<Label Text="{Binding DateTime.Second, StringFormat='{0}'}" … >
```

## Interactive MVVM

MVVM is often used with two-way data bindings for an interactive view based on an underlying data model.

The following example shows the `HslViewModel` that converts a `Color` value into `Hue`, `Saturation`, and `Luminosity` values, and back again:

:::code language="csharp" source="snippets/mvvm/csharp/HslViewModel.cs":::

In this example, changes to the `Hue`, `Saturation`, and `Luminosity` properties cause the `Color` property to change, and changes to the `Color` property causes the other three properties to change. This might seem like an infinite loop, except that the viewmodel doesn't invoke the `PropertyChanged` event unless the property has changed.

The following XAML example contains a `BoxView` whose `Color` property is bound to the `Color` property of the viewmodel, and three `Slider` and three `Label` views bound to the `Hue`, `Saturation`, and `Luminosity` properties:

:::code language="xaml" source="snippets/mvvm/csharp/HslColorPage.xaml":::

The binding on each `Label` is the default `OneWay`. It only needs to display the value. However, the default binding on each `Slider` is `TwoWay`. This allows the `Slider` to be initialized from the viewmodel. When the viewmodel is instantiated it's `Color` property is set to `Aqua`. A change in a `Slider` sets a new value for the property in the viewmodel, which then calculates a new color:

:::image type="content" source="media/mvvm/hslcolorscroll.png" alt-text="MVVM using two-way data bindings.":::

## Commanding

Sometimes an app has needs that go beyond property bindings by requiring the user to initiate commands that affect something in the viewmodel. These commands are generally signaled by button clicks or finger taps, and traditionally they are processed in the code-behind file in a handler for the `Clicked` event of the `Button` or the `Tapped` event of a `TapGestureRecognizer`.

The commanding interface provides an alternative approach to implementing commands that is much better suited to the MVVM architecture. The viewmodel can contain commands, which are methods that are executed in reaction to a specific activity in the view such as a `Button` click. Data bindings are defined between these commands and the `Button`.

To allow a data binding between a `Button` and a viewmodel, the `Button` defines two properties:

- `Command` of type [`System.Windows.Input.ICommand`](xref:System.Windows.Input.ICommand)
- `CommandParameter` of type `Object`

> [!NOTE]
> Many other controls also define `Command` and `CommandParameter` properties.

The [`ICommand`](xref:System.Windows.Input.ICommand) interface is defined in the [System.Windows.Input](xref:System.Windows.Input) namespace, and consists of two methods and one event:

- `void Execute(object arg)`
- `bool CanExecute(object arg)`
- `event EventHandler CanExecuteChanged`

The viewmodel can define properties of type `ICommand`. You can then bind these properties to the `Command` property of each `Button` or other element, or perhaps a custom view that implements this interface. You can optionally set the `CommandParameter` property to identify individual `Button` objects (or other elements) that are bound to this viewmodel property. Internally, the `Button` calls the `Execute` method whenever the user taps the `Button`, passing to the `Execute` method its `CommandParameter`.

The `CanExecute` method and `CanExecuteChanged` event are used for cases where a `Button` tap might be currently invalid, in which case the `Button` should disable itself. The `Button` calls `CanExecute` when the `Command` property is first set and whenever the `CanExecuteChanged` event is raised. If `CanExecute` returns `false`, the `Button` disables itself and doesn’t generate `Execute` calls.

You can use the `Command` or `Command<T>` class included in .NET MAUI to implement the `ICommand` interface. These two classes define several constructors plus a `ChangeCanExecute` method that the viewmodel can call to force the `Command` object to raise the `CanExecuteChanged` event.

The following example shows a viewmodel for a simple keypad that is intended for entering telephone numbers:

:::code language="csharp" source="snippets/mvvm/csharp/KeypadViewModel.cs":::

In this example, the `Execute` and `CanExecute` methods for the commands are defined as lambda functions in the constructor. The viewmodel assumes that the `AddCharCommand` property is bound to the `Command` property of several buttons (or anything other controls that have a command interface), each of which is identified by the `CommandParameter`. These buttons add characters to an `InputString` property, which is then formatted as a phone number for the `DisplayText` property. There's also a second property of type `ICommand` named `DeleteCharCommand`. This is bound to a back-spacing button, but the button should be disabled if there are no characters to delete.

The following example shows the XAML that consumes the `KeypadViewModel`:

:::code language="xaml" source="snippets/mvvm/csharp/KeypadPage.xaml":::

In this example, the `Command` property of the first `Button` that is bound to the `DeleteCharCommand`. The other buttons are bound to the `AddCharCommand` with a `CommandParameter` that's the same as the character that appears on the `Button`:

:::image type="content" source="media/mvvm/keypad.png" alt-text="Screenshot of a calculator using MVVM and commands.":::

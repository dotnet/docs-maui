---
title: "Data binding and MVVM"
description: "The Model-View-ViewModel (MVVM) pattern enforces a separation between three software layers — the XAML user interface, called the view, the underlying data, called the model, and an intermediary between the view and the model, called the viewmodel."
ms.date: 01/26/2022
---

# Data binding and MVVM

The Model-View-ViewModel (MVVM) pattern enforces a separation between three software layers — the XAML user interface, called the view, the underlying data, called the model, and an intermediary between the view and the model, called the viewmodel. The view and the viewmodel are often connected through data bindings defined in XAML. The `BindingContext` for the view is usually an instance of the viewmodel.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Simple MVVM

In [XAML markup extensions](markup-extensions.md) you saw how to define a new XML namespace declaration to allow a XAML file to reference classes in other assemblies. The following example uses the `x:Static` markup extension to obtain the current date and time from the static `DateTime.Now` property in the `System` namespace:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:sys="clr-namespace:System;assembly=netstandard"
             x:Class="XamlSamples.OneShotDateTimePage"
             Title="One-Shot DateTime Page">
    <StackLayout BindingContext="{x:Static sys:DateTime.Now}"
                 HorizontalOptions="Center"
                 VerticalOptions="Center">
        <Label Text="{Binding Year, StringFormat='The year is {0}'}" />
        <Label Text="{Binding StringFormat='The month is {0:MMMM}'}" />
        <Label Text="{Binding Day, StringFormat='The day is {0}'}" />
        <Label Text="{Binding StringFormat='The time is {0:T}'}" />
    </StackLayout>
</ContentPage>
```

In this example, the retrieved `DateTime` value is set as the `BindingContext` on a `StackLayout`. When you set the `BindingContext` on an element, it is inherited by all the children of that element. This means that all the children of the `StackLayout` have the same `BindingContext`, and they can contain bindings to properties of that object:

:::image type="content" source="media/mvvm/oneshotdatetime.png" alt-text="Screenshot of a page displaying the date and time.":::

However, the problem is that the date and time are set once when the page is constructed and initialized, and never change.

A XAML page can display a clock that always shows the current time, but it requires additional code. The MVVM pattern is a natural choice for .NET MAUI apps when data binding from properties between visual objects and the underlying data. When thinking in terms of MVVM, the model and viewmodel are classes written entirely in code. The view is often a XAML file that references properties defined in the viewmodel through data bindings. In MVVM, a model is ignorant of the viewmodel, and a viewmodel is ignorant of the view. However, often you tailor the types exposed by the viewmodel to the types associated with the UI.

> [!NOTE]
> In simple examples of MVVM, such as those shown here, often there is no model at all, and the pattern involves just a view and viewmodel linked with data bindings.

Here’s a ViewModel for a clock with just a single property named `DateTime`, which updates that `DateTime` property every second:

```csharp
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace XamlSamples
{
    class ClockViewModel : INotifyPropertyChanged
    {
        DateTime dateTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public ClockViewModel()
        {
            this.DateTime = DateTime.Now;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    this.DateTime = DateTime.Now;
                    return true;
                });
        }

        public DateTime DateTime
        {
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateTime"));
                    }
                }
            }
            get
            {
                return dateTime;
            }
        }
    }
}
```

ViewModels generally implement the `INotifyPropertyChanged` interface, which means that the class fires a `PropertyChanged` event whenever one of its properties changes. The data binding mechanism in .NET MAUI attaches a handler to this `PropertyChanged` event so it can be notified when a property changes and keep the target updated with the new value.

A clock based on this ViewModel can be as simple as this:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:XamlSamples;assembly=XamlSamples"
             x:Class="XamlSamples.ClockPage"
             Title="Clock Page">

    <Label Text="{Binding DateTime, StringFormat='{0:T}'}"
           FontSize="Large"
           HorizontalOptions="Center"
           VerticalOptions="Center">
        <Label.BindingContext>
            <local:ClockViewModel />
        </Label.BindingContext>
    </Label>
</ContentPage>
```

Notice how the `ClockViewModel` is set to the `BindingContext` of the `Label` using property element tags. Alternatively, you can instantiate the `ClockViewModel` in a `Resources` collection and set it to the `BindingContext` via a `StaticResource` markup extension. Or, the code-behind file can instantiate the ViewModel.

The `Binding` markup extension on the `Text` property of the `Label` formats the `DateTime` property. Here’s the display:

:::image type="content" source="media/mvvm/clock.png" alt-text="Screenshot of a page displaying the date and time via a viewmodel.":::

It’s also possible to access individual properties of the `DateTime` property of the ViewModel by separating the properties with periods:

```xaml
<Label Text="{Binding DateTime.Second, StringFormat='{0}'}" … >
```

## Interactive MVVM

MVVM is often used with two-way data bindings for an interactive view based on an underlying data model.

Here’s a class named `HslViewModel` that converts a `Color` value into `Hue`, `Saturation`, and `Luminosity` values, and vice versa:

```csharp
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace XamlSamples
{
    public class HslViewModel : INotifyPropertyChanged
    {
        double hue, saturation, luminosity;
        Color color;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Hue
        {
            set
            {
                if (hue != value)
                {
                    hue = value;
                    OnPropertyChanged("Hue");
                    SetNewColor();
                }
            }
            get
            {
                return hue;
            }
        }

        public double Saturation
        {
            set
            {
                if (saturation != value)
                {
                    saturation = value;
                    OnPropertyChanged("Saturation");
                    SetNewColor();
                }
            }
            get
            {
                return saturation;
            }
        }

        public double Luminosity
        {
            set
            {
                if (luminosity != value)
                {
                    luminosity = value;
                    OnPropertyChanged("Luminosity");
                    SetNewColor();
                }
            }
            get
            {
                return luminosity;
            }
        }

        public Color Color
        {
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged("Color");

                    Hue = value.Hue;
                    Saturation = value.Saturation;
                    Luminosity = value.Luminosity;
                }
            }
            get
            {
                return color;
            }
        }

        void SetNewColor()
        {
            Color = Color.FromHsla(Hue, Saturation, Luminosity);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

Changes to the `Hue`, `Saturation`, and `Luminosity` properties cause the `Color` property to change, and changes to `Color` causes the other three properties to change. This might seem like an infinite loop, except that the class doesn't invoke the `PropertyChanged` event unless the property has changed. This puts an end to the otherwise uncontrollable feedback loop.

The following XAML file contains a `BoxView` whose `Color` property is bound to the `Color` property of the ViewModel, and three `Slider` and three `Label` views bound to the `Hue`, `Saturation`, and `Luminosity` properties:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:XamlSamples;assembly=XamlSamples"
             x:Class="XamlSamples.HslColorScrollPage"
             Title="HSL Color Scroll Page">
    <ContentPage.BindingContext>
        <local:HslViewModel Color="Aqua" />
    </ContentPage.BindingContext>

    <StackLayout Padding="10, 0">
        <BoxView Color="{Binding Color}"
                 VerticalOptions="FillAndExpand" />

        <Label Text="{Binding Hue, StringFormat='Hue = {0:F2}'}"
               HorizontalOptions="Center" />

        <Slider Value="{Binding Hue, Mode=TwoWay}" />

        <Label Text="{Binding Saturation, StringFormat='Saturation = {0:F2}'}"
               HorizontalOptions="Center" />

        <Slider Value="{Binding Saturation, Mode=TwoWay}" />

        <Label Text="{Binding Luminosity, StringFormat='Luminosity = {0:F2}'}"
               HorizontalOptions="Center" />

        <Slider Value="{Binding Luminosity, Mode=TwoWay}" />
    </StackLayout>
</ContentPage>
```

The binding on each `Label` is the default `OneWay`. It only needs to display the value. But the binding on each `Slider` is `TwoWay`. This allows the `Slider` to be initialized from the ViewModel. Notice that the `Color` property is set to `Aqua` when the ViewModel is instantiated. But a change in the `Slider` also needs to set a new value for the property in the ViewModel, which then calculates a new color.

:::image type="content" source="media/mvvm/hslcolorscroll.png" alt-text="MVVM using two-way data bindings.":::

## Commanding with ViewModels

In many cases, the MVVM pattern is restricted to the manipulation of data items: User-interface objects in the View parallel data objects in the ViewModel.

However, sometimes the View needs to contain buttons that trigger various actions in the ViewModel. But the ViewModel must not contain `Clicked` handlers for the buttons because that would tie the ViewModel to a particular user-interface paradigm.

To allow ViewModels to be more independent of particular user interface objects but still allow methods to be called within the ViewModel, a *command* interface exists. This command interface is supported by the following elements in .NET MAUI:

- `Button`
- `MenuItem`
- `ToolbarItem`
- `SearchBar`
- `TextCell` (and hence also `ImageCell`
- `ListView`
- `TapGestureRecognizer`

With the exception of the `SearchBar` and `ListView` element, these elements define two properties:

- `Command` of type  `System.Windows.Input.ICommand`
- `CommandParameter` of type  `Object`

The `SearchBar` defines `SearchCommand` and `SearchCommandParameter` properties, while the `ListView` defines a `RefreshCommand` property of type `ICommand`.

The `ICommand` interface defines two methods and one event:

- `void Execute(object arg)`
- `bool CanExecute(object arg)`
- `event EventHandler CanExecuteChanged`

The ViewModel can define properties of type `ICommand`. You can then bind these properties to the `Command` property of each `Button` or other element, or perhaps a custom view that implements this interface. You can optionally set the `CommandParameter` property to identify individual `Button` objects (or other elements) that are bound to this ViewModel property. Internally, the `Button` calls the `Execute` method whenever the user taps the `Button`, passing to the `Execute` method its `CommandParameter`.

The `CanExecute` method and `CanExecuteChanged` event are used for cases where a `Button` tap might be currently invalid, in which case the `Button` should disable itself. The `Button` calls `CanExecute` when the `Command` property is first set and whenever the `CanExecuteChanged` event is fired. If `CanExecute` returns `false`, the `Button` disables itself and doesn’t generate `Execute` calls.

For help with adding commanding to your ViewModels, .NET MAUI defines two classes that implement `ICommand`: `Command` and `Command<T>` where `T` is the type of the arguments to `Execute` and `CanExecute`. These two classes define several constructors plus a `ChangeCanExecute` method that the ViewModel can call to force the `Command` object to fire the `CanExecuteChanged` event.

Here is a ViewModel for a simple keypad that is intended for entering telephone numbers. Notice that the `Execute` and `CanExecute` method are defined as lambda functions right in the constructor:

```csharp
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamlSamples
{
    class KeypadViewModel : INotifyPropertyChanged
    {
        string inputString = "";
        string displayText = "";
        char[] specialChars = { '*', '#' };

        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public KeypadViewModel()
        {
            AddCharCommand = new Command<string>((key) =>
                {
                    // Add the key to the input string.
                    InputString += key;
                });

            DeleteCharCommand = new Command(() =>
                {
                    // Strip a character from the input string.
                    InputString = InputString.Substring(0, InputString.Length - 1);
                },
                () =>
                {
                    // Return true if there's something to delete.
                    return InputString.Length > 0;
                });
        }

        // Public properties
        public string InputString
        {
            protected set
            {
                if (inputString != value)
                {
                    inputString = value;
                    OnPropertyChanged("InputString");
                    DisplayText = FormatText(inputString);

                    // Perhaps the delete button must be enabled/disabled.
                    ((Command)DeleteCharCommand).ChangeCanExecute();
                }
            }

            get { return inputString; }
        }

        public string DisplayText
        {
            protected set
            {
                if (displayText != value)
                {
                    displayText = value;
                    OnPropertyChanged("DisplayText");
                }
            }
            get { return displayText; }
        }

        // ICommand implementations
        public ICommand AddCharCommand { protected set; get; }

        public ICommand DeleteCharCommand { protected set; get; }

        string FormatText(string str)
        {
            bool hasNonNumbers = str.IndexOfAny(specialChars) != -1;
            string formatted = str;

            if (hasNonNumbers || str.Length < 4 || str.Length > 10)
            {
            }
            else if (str.Length < 8)
            {
                formatted = String.Format("{0}-{1}",
                                          str.Substring(0, 3),
                                          str.Substring(3));
            }
            else
            {
                formatted = String.Format("({0}) {1}-{2}",
                                          str.Substring(0, 3),
                                          str.Substring(3, 3),
                                          str.Substring(6));
            }
            return formatted;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

This ViewModel assumes that the `AddCharCommand` property is bound to the `Command` property of several buttons (or anything else that has a command interface), each of which is identified by the `CommandParameter`. These buttons add characters to an `InputString` property, which is then formatted as a phone number for the `DisplayText` property.

There is also a second property of type `ICommand` named `DeleteCharCommand`. This is bound to a back-spacing button, but the button should be disabled if there are no characters to delete.

The following keypad is not as visually sophisticated as it could be. Instead, the markup has been reduced to a minimum to demonstrate more clearly the use of the command interface:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:XamlSamples;assembly=XamlSamples"
             x:Class="XamlSamples.KeypadPage"
             Title="Keypad Page">

    <Grid HorizontalOptions="Center"
          VerticalOptions="Center">
        <Grid.BindingContext>
            <local:KeypadViewModel />
        </Grid.BindingContext>

        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="80" />
            <ColumnDefinition Width="80" />
            <ColumnDefinition Width="80" />
        </Grid.ColumnDefinitions>

        <!-- Internal Grid for top row of items -->
        <Grid Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="3">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*" />
                <ColumnDefinition Width="Auto" />
            </Grid.ColumnDefinitions>

            <Frame Grid.Column="0"
                   OutlineColor="Accent">
                <Label Text="{Binding DisplayText}" />
            </Frame>

            <Button Text="&#x21E6;"
                    Command="{Binding DeleteCharCommand}"
                    Grid.Column="1"
                    BorderWidth="0" />
        </Grid>

        <Button Text="1"
                Command="{Binding AddCharCommand}"
                CommandParameter="1"
                Grid.Row="1" Grid.Column="0" />

        <Button Text="2"
                Command="{Binding AddCharCommand}"
                CommandParameter="2"
                Grid.Row="1" Grid.Column="1" />

        <Button Text="3"
                Command="{Binding AddCharCommand}"
                CommandParameter="3"
                Grid.Row="1" Grid.Column="2" />

        <Button Text="4"
                Command="{Binding AddCharCommand}"
                CommandParameter="4"
                Grid.Row="2" Grid.Column="0" />

        <Button Text="5"
                Command="{Binding AddCharCommand}"
                CommandParameter="5"
                Grid.Row="2" Grid.Column="1" />

        <Button Text="6"
                Command="{Binding AddCharCommand}"
                CommandParameter="6"
                Grid.Row="2" Grid.Column="2" />

        <Button Text="7"
                Command="{Binding AddCharCommand}"
                CommandParameter="7"
                Grid.Row="3" Grid.Column="0" />

        <Button Text="8"
                Command="{Binding AddCharCommand}"
                CommandParameter="8"
                Grid.Row="3" Grid.Column="1" />

        <Button Text="9"
                Command="{Binding AddCharCommand}"
                CommandParameter="9"
                Grid.Row="3" Grid.Column="2" />

        <Button Text="*"
                Command="{Binding AddCharCommand}"
                CommandParameter="*"
                Grid.Row="4" Grid.Column="0" />

        <Button Text="0"
                Command="{Binding AddCharCommand}"
                CommandParameter="0"
                Grid.Row="4" Grid.Column="1" />

        <Button Text="#"
                Command="{Binding AddCharCommand}"
                CommandParameter="#"
                Grid.Row="4" Grid.Column="2" />
    </Grid>
</ContentPage>
```

The `Command` property of the first `Button` that appears in this markup is bound to the `DeleteCharCommand`; the rest are bound to the `AddCharCommand` with a `CommandParameter` that is the same as the character that appears on the `Button` face. Here’s the program in action:

:::image type="content" source="media/mvvm/keypad.png" alt-text="Screenshot of a calculator using MVVM and commands.":::

### Invoke asynchronous methods

Commands can also invoke asynchronous methods. This is achieved by using the `async` and `await` keywords when specifying the `Execute` method:

```csharp
DownloadCommand = new Command (async () => await DownloadAsync ());
```

This indicates that the `DownloadAsync` method is a `Task` and should be awaited:

```csharp
async Task DownloadAsync ()
{
    await Task.Run (() => Download ());
}

void Download ()
{
    ...
}
```

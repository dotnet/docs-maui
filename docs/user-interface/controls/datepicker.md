---
title: "DatePicker"
description: "The .NET MAUI DatePicker is a view that allows you to select a date."
ms.date: 02/09/2022
---

# DatePicker

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.DatePicker> invokes the platform's date-picker control and allows you to select a date.

<xref:Microsoft.Maui.Controls.DatePicker> defines eight properties:

- `MinimumDate` of type [`DateTime`](xref:System.DateTime), which defaults to the first day of the year 1900.
- `MaximumDate` of type `DateTime`, which defaults to the last day of the year 2100.
- `Date` of type `DateTime`, the selected date, which defaults to the value [`DateTime.Today`](xref:System.DateTime.Today).
- `Format` of type `string`, a [standard](/dotnet/standard/base-types/standard-date-and-time-format-strings/) or [custom](/dotnet/standard/base-types/custom-date-and-time-format-strings/) .NET formatting string, which defaults to "D", the long date pattern.
- `TextColor` of type <xref:Microsoft.Maui.Graphics.Color>, the color used to display the selected date.
- `FontAttributes` of type `FontAttributes`, which defaults to `FontAtributes.None`.
- `FontFamily` of type `string`, which defaults to `null`.
- `FontSize` of type `double`, which defaults to -1.0.
- `CharacterSpacing`, of type `double`, is the spacing between characters of the <xref:Microsoft.Maui.Controls.DatePicker> text.

All eight properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be styled, and the properties can be targets of data bindings. The `Date` property has a default binding mode of `BindingMode.TwoWay`, which means that it can be a target of a data binding in an application that uses the Model-View-ViewModel (MVVM) pattern.

> [!WARNING]
> When setting `MinimumDate` and `MaximumDate`, make sure that `MinimumDate` is always less than or equal to `MaximumDate`. Otherwise, <xref:Microsoft.Maui.Controls.DatePicker> will raise an exception.

The <xref:Microsoft.Maui.Controls.DatePicker> ensures that `Date` is between `MinimumDate` and `MaximumDate`, inclusive. If `MinimumDate` or `MaximumDate` is set so that `Date` is not between them, <xref:Microsoft.Maui.Controls.DatePicker> will adjust the value of `Date`.

The <xref:Microsoft.Maui.Controls.DatePicker> fires a `DateSelected` event when the user selects a date.

## Create a DatePicker

When a `DateTime` value is specified in XAML, the XAML parser uses the `DateTime.Parse` method with a `CultureInfo.InvariantCulture` argument to convert the string to a `DateTime` value. The dates must be specified in a precise format: two-digit months, two-digit days, and four-digit years separated by slashes:

```xaml
<DatePicker MinimumDate="01/01/2022"
            MaximumDate="12/31/2022"
            Date="06/21/2022" />
```

If the `BindingContext` property of <xref:Microsoft.Maui.Controls.DatePicker> is set to an instance of a viewmodel containing properties of type `DateTime` named `MinDate`, `MaxDate`, and `SelectedDate` (for example), you can instantiate the <xref:Microsoft.Maui.Controls.DatePicker> like this:

```xaml
<DatePicker MinimumDate="{Binding MinDate}"
            MaximumDate="{Binding MaxDate}"
            Date="{Binding SelectedDate}" />
```

In this example, all three properties are initialized to the corresponding properties in the viewmodel. Because the `Date` property has a binding mode of `TwoWay`, any new date that the user selects is automatically reflected in the viewmodel.

If the <xref:Microsoft.Maui.Controls.DatePicker> does not contain a binding on its `Date` property, your app should attach a handler to the `DateSelected` event to be informed when the user selects a new date.

In code, you can initialize the `MinimumDate`, `MaximumDate`, and `Date` properties to values of type `DateTime`:

```csharp
DatePicker datePicker = new DatePicker
{
    MinimumDate = new DateTime(2018, 1, 1),
    MaximumDate = new DateTime(2018, 12, 31),
    Date = new DateTime(2018, 6, 21)
};
```

For information about setting font properties, see [Fonts](~/user-interface/fonts.md).

## DatePicker and layout

It's possible to use an unconstrained horizontal layout option such as `Center`, `Start`, or `End` with <xref:Microsoft.Maui.Controls.DatePicker>:

```xaml
<DatePicker ···
            HorizontalOptions="Center" />
```

However, this is not recommended. Depending on the setting of the `Format` property, selected dates might require different display widths. For example, the "D" format string causes `DateTime` to display dates in a long format, and "Wednesday, September 12, 2018" requires a greater display width than "Friday, May 4, 2018". Depending on the platform, this difference might cause the `DateTime` view to change width in layout, or for the display to be truncated.

> [!TIP]
> It's best to use the default `HorizontalOptions` setting of `Fill` with <xref:Microsoft.Maui.Controls.DatePicker>, and not to use a width of `Auto` when putting <xref:Microsoft.Maui.Controls.DatePicker> in a <xref:Microsoft.Maui.Controls.Grid> cell.

<!--
> [!TIP]
> On Android, the <xref:Microsoft.Maui.Controls.DatePicker> dialog can be customized by overriding the `CreateDatePickerDialog` method in a custom renderer. This allows, for example, additional buttons to be added to the dialog. -->

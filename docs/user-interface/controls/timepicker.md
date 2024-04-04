---
title: "TimePicker"
description: "The .NET MAUI TimePicker is a view that allows the user to select a time."
ms.date: 02/09/2022
---

# TimePicker

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.TimePicker> invokes the platform's time-picker control and allows you to select a time.

<xref:Microsoft.Maui.Controls.TimePicker> defines the following properties:

- `Time` of type `TimeSpan`, the selected time, which defaults to a `TimeSpan` of 0. The `TimeSpan` type indicates a duration of time since midnight.
- `Format` of type `string`, a [standard](/dotnet/standard/base-types/standard-date-and-time-format-strings/) or [custom](/dotnet/standard/base-types/custom-date-and-time-format-strings/) .NET formatting string, which defaults to "t", the short time pattern.
- `TextColor` of type <xref:Microsoft.Maui.Graphics.Color>, the color used to display the selected time.
- `FontAttributes` of type `FontAttributes`, which defaults to `FontAtributes.None`.
- `FontFamily` of type `string`, which defaults to `null`.
- `FontSize` of type `double`, which defaults to -1.0.
- `CharacterSpacing`, of type `double`, is the spacing between characters of the <xref:Microsoft.Maui.Controls.TimePicker> text.

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be styled, and the properties can be targets of data bindings. The `Time` property has a default binding mode of `BindingMode.TwoWay`, which means that it can be a target of a data binding in an application that uses the Model-View-ViewModel (MVVM) pattern.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.TimePicker> doesn't include an event to indicate a new selected `Time` value. If you need to be notified of this, you can add an event handler for the `PropertyChanged` event.

## Create a TimePicker

When the `Time` property is specified in XAML, the value is converted to a `TimeSpan` and validated to ensure that the number of milliseconds is greater than or equal to 0, and that the number of hours is less than 24. The time components should be separated by colons:

```xaml
<TimePicker Time="4:15:26" />
```

If the `BindingContext` property of <xref:Microsoft.Maui.Controls.TimePicker> is set to an instance of a viewmodel containing a property of type `TimeSpan` named `SelectedTime` (for example), you can instantiate the <xref:Microsoft.Maui.Controls.TimePicker> like this:

```xaml
<TimePicker Time="{Binding SelectedTime}" />
```

In this example, the `Time` property is initialized to the `SelectedTime` property in the viewmodel. Because the `Time` property has a binding mode of `TwoWay`, any new time that the user selects is automatically propagated to the viewmodel.

In code, you can initialize the `Time` property to a value of type `TimeSpan`:

```csharp
TimePicker timePicker = new TimePicker
{
  Time = new TimeSpan(4, 15, 26) // Time set to "04:15:26"
};
```

For information about setting font properties, see [Fonts](~/user-interface/fonts.md).

## TimePicker and layout

It's possible to use an unconstrained horizontal layout option such as `Center`, `Start`, or `End` with <xref:Microsoft.Maui.Controls.TimePicker>:

```xaml
<TimePicker ···
            HorizontalOptions="Center" />
```

However, this is not recommended. Depending on the setting of the `Format` property, selected times might require different display widths. For example, the "T" format string causes the <xref:Microsoft.Maui.Controls.TimePicker> view to display times in a long format, and "4:15:26 AM" requires a greater display width than the short time format ("t") of "4:15 AM". Depending on the platform, this difference might cause the <xref:Microsoft.Maui.Controls.TimePicker> view to change width in layout, or for the display to be truncated.

> [!TIP]
> It's best to use the default `HorizontalOptions` setting of `Fill` with <xref:Microsoft.Maui.Controls.TimePicker>, and not to use a width of `Auto` when putting <xref:Microsoft.Maui.Controls.TimePicker> in a <xref:Microsoft.Maui.Controls.Grid> cell.

<!--
> [!TIP]
> On Android, the <xref:Microsoft.Maui.Controls.TimePicker> dialog can be customized by overriding the `CreateTimePickerDialog` method in a custom renderer. This allows, for example, additional buttons to be added to the dialog. -->

## Platform differences

This section describes the platform-specific differences with the <xref:Microsoft.Maui.Controls.TimePicker> control.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

On Android, the `Format` property is respected and displayed by the control. However, when the picker control is shown by pressing on the control, only the hour, minute, and time of day can be changed.

# [iOS/Mac Catalyst](#tab/macios)

On iOS, the `Format` property is respected and displayed by the control.

On macOS, the `Format` property has no effect on the control.

# [Windows](#tab/windows)

On Windows, the `Format` property only affects whether the hour is formatted for 12-hours or 24-hours. Other settings from the `Format` property are ignored. When the picker is shown by pressing on the control, only the hour, minute, and time of day can be changed. For more information about the Windows control, see [Time picker - Windows apps](/windows/apps/design/controls/time-picker).

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

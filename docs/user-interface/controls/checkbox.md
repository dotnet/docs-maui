---
title: "CheckBox"
description: "The .NET MAUI CheckBox is a type of button that can either be checked or empty. When a checkbox is checked, it's considered to be on. When a checkbox is empty, it's considered to be off."
ms.date: 02/09/2022
---

# CheckBox

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-checkbox)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.CheckBox> is a type of button that can either be checked or empty. When a checkbox is checked, it's considered to be on. When a checkbox is empty, it's considered to be off.

<xref:Microsoft.Maui.Controls.CheckBox> defines the following properties:

- `IsChecked`, of type `bool`, which indicates whether the <xref:Microsoft.Maui.Controls.CheckBox> is checked. This property has a default binding mode of `TwoWay`.
- `Color`, of type <xref:Microsoft.Maui.Graphics.Color>, which indicates the color of the <xref:Microsoft.Maui.Controls.CheckBox>.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be styled, and be the target of data bindings.

<xref:Microsoft.Maui.Controls.CheckBox> defines a `CheckedChanged` event that's raised when the `IsChecked` property changes, either through user manipulation or when an application sets the `IsChecked` property. The `CheckedChangedEventArgs` object that accompanies the `CheckedChanged` event has a single property named `Value`, of type `bool`. When the event is raised, the value of the `Value` property is set to the new value of the `IsChecked` property.

## Create a CheckBox

The following example shows how to instantiate a <xref:Microsoft.Maui.Controls.CheckBox> in XAML:

```xaml
<CheckBox />
```

This XAML results in the appearance shown in the following screenshot:

:::image type="content" source="media/checkbox/checkbox-empty.png" alt-text="Screenshot of an empty CheckBox.":::

By default, the <xref:Microsoft.Maui.Controls.CheckBox> is empty. The <xref:Microsoft.Maui.Controls.CheckBox> can be checked by user manipulation, or by setting the `IsChecked` property to `true`:

```xaml
<CheckBox IsChecked="true" />
```

This XAML results in the appearance shown in the following screenshot:

:::image type="content" source="media/checkbox/checkbox-checked.png" alt-text="Screenshot of a checked CheckBox.":::

Alternatively, a <xref:Microsoft.Maui.Controls.CheckBox> can be created in code:

```csharp
CheckBox checkBox = new CheckBox { IsChecked = true };
```

## Respond to a CheckBox changing state

When the `IsChecked` property changes, either through user manipulation or when an application sets the `IsChecked` property, the `CheckedChanged` event fires. An event handler for this event can be registered to respond to the change:

```xaml
<CheckBox CheckedChanged="OnCheckBoxCheckedChanged" />
```

The code-behind file contains the handler for the `CheckedChanged` event:

```csharp
void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
{
    // Perform required operation after examining e.Value
}
```

The `sender` argument is the <xref:Microsoft.Maui.Controls.CheckBox> responsible for this event. You can use this to access the <xref:Microsoft.Maui.Controls.CheckBox> object, or to distinguish between multiple <xref:Microsoft.Maui.Controls.CheckBox> objects sharing the same `CheckedChanged` event handler.

Alternatively, an event handler for the `CheckedChanged` event can be registered in code:

```csharp
CheckBox checkBox = new CheckBox { ... };
checkBox.CheckedChanged += (sender, e) =>
{
    // Perform required operation after examining e.Value
};
```

## Data bind a CheckBox

The `CheckedChanged` event handler can be eliminated by using data binding and triggers to respond to a <xref:Microsoft.Maui.Controls.CheckBox> being checked or empty:

```xaml
<CheckBox x:Name="checkBox" />
<Label Text="Lorem ipsum dolor sit amet, elit rutrum, enim hendrerit augue vitae praesent sed non, lorem aenean quis praesent pede.">
    <Label.Triggers>
        <DataTrigger TargetType="Label"
                     Binding="{Binding Source={x:Reference checkBox}, Path=IsChecked}"
                     Value="true">
            <Setter Property="FontAttributes"
                    Value="Italic, Bold" />
            <Setter Property="FontSize"
                    Value="18" />
        </DataTrigger>
    </Label.Triggers>
</Label>
```

In this example, the <xref:Microsoft.Maui.Controls.Label> uses a binding expression in a data trigger to monitor the `IsChecked` property of the <xref:Microsoft.Maui.Controls.CheckBox>. When this property becomes `true`, the `FontAttributes` and `FontSize` properties of the <xref:Microsoft.Maui.Controls.Label> change. When the `IsChecked` property returns to `false`, the `FontAttributes` and `FontSize` properties of the <xref:Microsoft.Maui.Controls.Label> are reset to their initial state.

The following screenshot shows the <xref:Microsoft.Maui.Controls.Label> formatting when the <xref:Microsoft.Maui.Controls.CheckBox> is checked:

:::image type="content" source="media/checkbox/checkbox-databinding.png" alt-text="Screenshot of a data bound CheckBox.":::

For more information about triggers, see [Triggers](~/fundamentals/triggers.md).

## Disable a Checkbox

Sometimes an application enters a state where a <xref:Microsoft.Maui.Controls.CheckBox> being checked is not a valid operation. In such cases, the <xref:Microsoft.Maui.Controls.CheckBox> can be disabled by setting its `IsEnabled` property to `false`.

## CheckBox appearance

In addition to the properties that <xref:Microsoft.Maui.Controls.CheckBox> inherits from the <xref:Microsoft.Maui.Controls.View> class, <xref:Microsoft.Maui.Controls.CheckBox> also defines a `Color` property that sets its color to a <xref:Microsoft.Maui.Graphics.Color>:

```xaml
<CheckBox Color="Red" />
```

The following screenshot shows a series of checked <xref:Microsoft.Maui.Controls.CheckBox> objects, where each object has its `Color` property set to a different <xref:Microsoft.Maui.Graphics.Color>:

:::image type="content" source="media/checkbox/checkbox-colors.png" alt-text="Screenshot of colored CheckBoxes.":::

## CheckBox visual states

<xref:Microsoft.Maui.Controls.CheckBox> has an `IsChecked` <xref:Microsoft.Maui.Controls.VisualState> that can be used to initiate a visual change to the <xref:Microsoft.Maui.Controls.CheckBox> when it becomes checked.

The following XAML example shows how to define a visual state for the `IsChecked` state:

```xaml
<CheckBox ...>
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup x:Name="CommonStates">
            <VisualState x:Name="Normal">
                <VisualState.Setters>
                    <Setter Property="Color"
                            Value="Red" />
                </VisualState.Setters>
            </VisualState>

            <VisualState x:Name="IsChecked">
                <VisualState.Setters>
                    <Setter Property="Color"
                            Value="Green" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</CheckBox>
```

In this example, the `IsChecked` <xref:Microsoft.Maui.Controls.VisualState> specifies that when the <xref:Microsoft.Maui.Controls.CheckBox> is checked, its `Color` property will be set to green. The `Normal` <xref:Microsoft.Maui.Controls.VisualState> specifies that when the <xref:Microsoft.Maui.Controls.CheckBox> is in a normal state, its `Color` property will be set to red. Therefore, the overall effect is that the <xref:Microsoft.Maui.Controls.CheckBox> is red when it's empty, and green when it's checked.

For more information about visual states, see [Visual states](~/user-interface/visual-states.md).
